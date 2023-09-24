using HelperFunctions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PriconneReTLAutoUpdaterApp;
using PriconneReTLAutoUpdaterApp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstallerFunctions
{
    class Installer
    {
        Helper helper = new Helper();

        private string tempFile = Path.GetTempFileName();
        private bool removeSuccess = true;
        private bool downloadSuccess = true;
        private bool extractSuccess = true;
        private bool removeProgress = false;
        public event Action<double, double, string> DownloadProgress;
        public event Action<string, string, bool> Log;
        public event Action<string> ErrorLog;
        public event Action ProcessStart;
        public event Action ProcessFinish;
        public event Func<string, Task> StartCountdown;

        public async Task DownloadPatchFiles(string assetLink)
        {
            try
            {
                Log?.Invoke("Downloading compressed patch files...", "info", true);

                using (HttpClient client = new HttpClient())
                {
                    using (var response = await client.GetAsync(assetLink, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        long? totalBytesResponse = response.Content.Headers.ContentLength;
                        long totalBytes = totalBytesResponse ?? -1;

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                        {
                            var buffer = new byte[4096];
                            long downloadedBytes = 0;
                            int bytesRead;

                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);

                                downloadedBytes += bytesRead;
                                DownloadProgress?.Invoke(downloadedBytes, totalBytes, "Downloading compressed patch files...");
                            }
                        }
                    }
                }

                Log?.Invoke("Download completed.", "info", true);
                downloadSuccess = true;

            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke("Error downloading patch files: " + ex.Message);
                downloadSuccess = false;
            }
        }

        public async Task ExtractPatchFiles(string priconnePath)
        {
            try
            {

                if (!removeSuccess || !downloadSuccess) return;

                int counter = 0;
                using (var zip = ZipFile.OpenRead(tempFile))
                {
                    Log?.Invoke("Extracting files to game folder...", "add", true);

                    // Keep config files if already present
                    string[] ignoreFiles = helper.SetIgnoreFiles(priconnePath, addconfig: helper.IsConfigPresent(priconnePath));
                    foreach (var entry in zip.Entries)
                    {
                        counter++;
                        string fileName = entry.FullName;

                        DownloadProgress?.Invoke(counter, zip.Entries.Count, "Extracting files to game folder...");

                        if (!ignoreFiles.Contains(fileName) && !helper.IsInIgnoredDirectory(fileName))
                        {
                            string destinationPath = Path.Combine(priconnePath, Path.GetDirectoryName(fileName));
                            if (!Directory.Exists(destinationPath))
                                Directory.CreateDirectory(destinationPath);

                            Log?.Invoke("Extracting: " + entry.FullName, "add", false);
                            await Task.Run(() => ExtractZipEntry(entry, Path.Combine(priconnePath, fileName)));
                        }
                    }
                }
                Log?.Invoke("File extraction completed.", "info", true);
                extractSuccess = true;

            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke("Error extracting all files: " + ex.Message);
                extractSuccess = false;
            }
        }

        public void ExtractZipEntry(ZipArchiveEntry entry, string destinationPath)
        {
            try
            {
                if (entry.Name != "")
                {
                    var destinationDirectory = Path.GetDirectoryName(destinationPath);
                    Directory.CreateDirectory(destinationDirectory);

                    entry.ExtractToFile(destinationPath, true);
                }
            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke("Error extracting file: " + ex.Message);
            }
        }

        public async Task<string[]> ProcessTree(string priconnePath, string releaseTag)
        {
            string[] ignoreFiles = helper.SetIgnoreFiles(priconnePath, addconfig: true);
            List<string> filePathsList = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PriconneReTLInstaller");

                string treeUrl = $"{Settings.Default.githubApi}/git/trees/{releaseTag}?recursive=1";

                HttpResponseMessage response = await client.GetAsync(treeUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic treeJson = JObject.Parse(responseBody);

                    foreach (var item in treeJson.tree)
                    {
                        string fileType = item.type;
                        string filePath = item.path;

                        if (fileType == "blob" && filePath.StartsWith("src/"))
                        {
                            string trimmedPath = filePath.Substring("src/".Length);
                            if (!ignoreFiles.Contains(trimmedPath) && !helper.IsInIgnoredDirectory(trimmedPath))
                            {
                                filePathsList.Add(trimmedPath);
                                Console.WriteLine($"File in 'src' Path: {trimmedPath}");
                            }
                        }
                    }
                }
                else
                {
                    ErrorLog?.Invoke($"Failed to fetch tree for tag '{releaseTag}'. Status code: {response.StatusCode}");
                    return null;
                }

                return filePathsList.ToArray();
            }
        }

        public async Task RemovePatchFiles(string priconnePath, string localVersion)

        {
            await Task.Run(() =>
            {
                try
                {

                    if (downloadSuccess == false)
                    {
                        extractSuccess = false;
                        return;
                    }

                    removeProgress = true;

                    string[] currentFiles = ProcessTree(priconnePath, localVersion).GetAwaiter().GetResult();

                    if (currentFiles == null)
                    {
                        removeSuccess = false;
                        throw new Exception("Failed to get list of files to remove! Cannot continue.");
                    }

                    Log?.Invoke("Removing old patch files...", "remove", true);

                    int counter = 0;

                    foreach (var file in currentFiles)
                    {
                        string filePath = Path.Combine(priconnePath, file);
                        string directory = Path.GetDirectoryName(filePath);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Log?.Invoke($"Removed file: {file}", "remove", false);

                            DeleteEmptyDirectories(directory);

                            counter++;
                            double percentage = ((double)counter / currentFiles.Length) * 100;
                            DownloadProgress?.Invoke(counter, currentFiles.Length, "Removing old patch files...");

                        }
                    }

                    Log?.Invoke("File removal completed.", "info", true);
                    removeSuccess = true;
                    removeProgress = false;

                }
                catch (Exception ex)
                {
                    removeSuccess = false;
                    removeProgress = false;
                    ErrorLog?.Invoke("Error removing files: " + ex.Message);
                }
            });
        }

        private void DeleteEmptyDirectories(string directoryPath)
        {
            if (!Directory.EnumerateFileSystemEntries(directoryPath).Any())
            {
                Directory.Delete(directoryPath);
                Log?.Invoke($"Removed directory: {directoryPath}", "remove", false);

                string parentDirectory = Path.GetDirectoryName(directoryPath);
                if (!string.IsNullOrEmpty(parentDirectory))
                {
                    DeleteEmptyDirectories(parentDirectory);
                }
            }
        }

        public async void ProcessOperation(string priconnePath, string localVersion, string latestVersion, string assetLink)
        {
            try
            {        
                Log?.Invoke("Found new version! Starting update...", "info", true);

                var task = StartCountdown?.Invoke("Cancel Update");
                if (task != null) await task;

                ProcessStart?.Invoke();

                await DownloadPatchFiles(assetLink);
                await RemovePatchFiles(priconnePath, localVersion);
                await ExtractPatchFiles(priconnePath);
                return;
            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke("Error completing process: " + ex.Message);
            }

            finally
            {
                bool processSuccess = removeSuccess && downloadSuccess && extractSuccess;

                if (!processSuccess)
                {
                    ErrorLog?.Invoke($"Update failed!");
                }
                else 
                {
                    Log?.Invoke($"Update complete!", "success", true);
                    ProcessFinish?.Invoke();
                }
                    
                var task = StartCountdown?.Invoke("Exit Updater");
                if (task != null) await task;

                Application.Exit();
            }
        }

        public void HandleFormClosing(MainForm form, FormClosingEventArgs e)
        {
            try
            {
                if (removeProgress)
                {
                    helper.CannotExitNotification(e, "file removal");
                }
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
            catch (IOException)
            {
                helper.CannotExitNotification(e, "file download / extraction");
            }
        }

    }
}