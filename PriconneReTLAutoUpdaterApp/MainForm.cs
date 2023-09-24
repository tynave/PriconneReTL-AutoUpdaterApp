using HelperFunctions;
using InstallerFunctions;
using LoggerFunctions;
using PriconneReTLAutoUpdaterApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriconneReTLAutoUpdaterApp
{
    public partial class MainForm : Form
    {
        private Logger logger;
        private string priconnePath;
        private string latestVersion;
        private string localVersion;
        private string assetLink;
        PrivateFontCollection priconnefont = new PrivateFontCollection();
        private bool mouseDown;
        private Point lastLocation;
        private string[] appArgs;
     

        Helper helper = new Helper();
        Installer installer = new Installer();
        public MainForm(string[] args)
        {
            InitializeComponent();
            appArgs = args;

            this.StartPosition = FormStartPosition.CenterScreen;

            installer.Log += OnLog;
            installer.ErrorLog += OnErrorLog;
            installer.DownloadProgress += OnDownloadProgress;
            installer.ProcessStart += OnProcessStart;
            installer.ProcessFinish += OnProcessFinish;
            installer.StartCountdown += StartCountdown;
            helper.Log += OnLog;

            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;

            logger = new Logger("ReTLAutoUpdater.log", statusLabel);
            logger.StartSession();

            helper.PriconneFont(priconnefont);
            helper.SetFontForAllControls(priconnefont, Controls);
        }
        // Functions
        private void InitializeUI()
        {
            Icon = Resources.jewel;
            helper.PriconneFont(priconnefont);
            helper.SetFontForAllControls(priconnefont, Controls);

            if (appArgs.Length == 0)
            {
                logger.Log("This application cannot be run standalone!", "error", true);
                button1.Visible = true;
                return;
            }

            else
            {
                priconnePath = appArgs[0];
                gamePathLinkLabel.Text = priconnePath;

                latestVersion = appArgs[2];
                latestVersionLinkLabel.Text = latestVersion;

                assetLink = appArgs[3];

                localVersion = appArgs[1];
                localVersionLabel.Text = "Current (Local) Version: " + localVersion;

                newPictureBox.Visible = true;
            }

        }

        // Events
        private void OnLog(string message, string color, bool writeToStatus = false)
        {
            statusLabel.Invoke((Action)(() =>
            {
                logger.Log(message, color, writeToStatus);
            }));
        }

        private void OnErrorLog(string message)
        {
            statusLabel.Invoke((Action)(() =>
            {
                logger.Error(message);
            }));
        }

        public void OnDownloadProgress(double currentValue, double maxValue, string status /*Image image*/)
        {
            double percentage = ((double)currentValue / (double)maxValue) * 100;
            statusLabel.Invoke((Action)(() =>
            {
                statusLabel.Text = status + $" {Math.Truncate(percentage)}%";
            }));
        }
        private async void OnMouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;

            await Task.Run(() =>
            {
                while (mouseDown)
                {
                    this.Invoke((Action)(() =>
                    {
                        this.Location = new Point(
                            (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                        this.Update();
                    }));
                }
            });
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {

            if (mouseDown)
            {
                this.Invoke((Action)(() =>
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                    this.Update();
                }));
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private async Task StartCountdown(string baseText)
        {
            int countdown = 3;
            var tcs = new TaskCompletionSource<bool>();

            // Create a new Timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (sender, e) =>
            {
                button1.Visible = true;
                if (countdown > 0)
                {
                    // Update the button text with the current countdown value
                    button1.Text = $"{baseText}... ({countdown})";
                    countdown--;
                }
                else
                {
                    // Countdown has reached zero; stop the timer
                    timer.Stop();
                    button1.Visible = false;
                    tcs.TrySetResult(true); // Signal that the countdown is complete
                }
            };

            // Start the timer
            timer.Start();

            await tcs.Task;
            
        }

        private void OnProcessStart()
        {
            // Placeholder event for future updates
        }

        private void OnProcessFinish()
        {
            localVersionLabel.Text = "Current (Local) Version: " + latestVersion;
            newPictureBox.Visible = false;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Activate();
            InitializeUI();
            if (appArgs.Length > 0) installer.ProcessOperation(priconnePath, localVersion, latestVersion, assetLink);

        }

        private void gamePathLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             ProcessStartInfo startInfo = new ProcessStartInfo("explorer.exe");
             startInfo.Arguments = priconnePath;
             Process.Start(startInfo);
        }

        private void latestVersionLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (latestVersion != null) Process.Start("https://github.com/ImaterialC/PriconneRe-TL/releases/latest");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            installer.HandleFormClosing(this, e);
        }
    }
}
