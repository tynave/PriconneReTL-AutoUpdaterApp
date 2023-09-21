using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;

namespace LoggerFunctions 
{
    public class Logger
    {
        private string logFilePath;
        private Dictionary<string, Color> colors = new Dictionary<string, Color>
            {
                { "info", Color.Black },
                { "error", Color.Red },
                { "success", Color.Green },
                { "add", Color.Blue },
                { "remove", Color.Red },
            };
        private Label statusLabel;

        public Logger(string logFilePath, Label statusLabel)
        {
            this.logFilePath = logFilePath;
            this.statusLabel = statusLabel;
        }

        public void StartSession()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, false))
                {
                    writer.WriteLine($"[PriconneReTL AutoUpdaterApp version: {String.Format(System.Windows.Forms.Application.ProductVersion)}]");
                    writer.WriteLine($"[Log file created at: {DateTime.Now}]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing log file: {ex.Message}");
            }
        }

        public void Log(string message, string level, bool writeToStatus = false)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) writer.WriteLine($"[{DateTime.Now}] - {message}");

                if (writeToStatus)
                {
                    statusLabel.ForeColor = colors[level];
                    statusLabel.Text = message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        public void Error(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true)) writer.WriteLine($"[{DateTime.Now}] - ERROR: {message}");

                statusLabel.ForeColor = colors["error"];
                statusLabel.Text = $"ERROR! - See log for details.";


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

    }
}