using FortniteLauncher.Pages;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace FortniteLauncher.Core.Mods
{
    internal class DownloadFile
    {
        public static async Task DownloadFiles(string URL, string path, IProgress<double> progress = null)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    if (progress != null)
                    {
                        webClient.DownloadProgressChanged += (sender, e) =>
                        {
                            double percentage = (double)e.BytesReceived / e.TotalBytesToReceive * 100;
                            progress.Report(percentage);
                        };
                    }

                    webClient.DownloadFileAsync(new Uri(URL), path);
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Error while installing required files: " + ex.Message, "WebClient Error");
            }
        }

        public static async Task WaitForFileToBeReleased(string filePath)
        {
            while (IsFileLocked(filePath))
            {
                await Task.Delay(100); // Delay to avoid busy-waiting
            }
        }

        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // If the file can be opened with no sharing, it's not locked
                }
            }
            catch (IOException)
            {
                // The file is locked
                return true;
            }

            // The file is not locked
            return false;
        }
    }
}
