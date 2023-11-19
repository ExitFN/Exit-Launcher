using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using FortniteLauncher.Core;
using FortniteLauncher.Helpers;
using FortniteLauncher.Services;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace FortniteLauncher.Pages
{
    public sealed partial class DownloadsPage : Page
    {
        private string _buildPath = string.Empty;
        private string _downloadPathFull;
        private DateTime _lastUpdate;
        private long _lastBytes = 0;
        private WebClient _webClient;
        private string _downloadPath;
        int _downloadPrecentageProgress;
        public DownloadsPage()
        {
            this.InitializeComponent();
            InitializeBuildPath();
        }

        private void InitializeBuildPath()
        {
            if (Globals.m_config.FortnitePath == null || !PathHelper.IsPathValid(Globals.m_config.FortnitePath))
            {
                _buildPath = "Path must contain FortniteGame and Engine folders!";
            }
            else
            {
                Definitions.FortnitePath = Globals.m_config.FortnitePath;
                _buildPath = Globals.m_config.FortnitePath;
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
        public static async Task WaitForFileToBeReleased(string filePath)
        {
            while (IsFileLocked(filePath))
            {
                await Task.Delay(100); // Delay to avoid busy-waiting
            }
        }
        private async void ChangeInstallPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderPicker openPicker = CreateFolderPicker();
                StorageFolder folder = await openPicker.PickSingleFolderAsync();

                if (folder != null && PathHelper.IsPathValid(folder.Path))
                {
                    Definitions.FortnitePath = Globals.m_config.FortnitePath;
                    Globals.m_config.FortnitePath = folder.Path;
                    _buildPath = folder.Path;

                    Settings.SaveSettings();

                    // Refresh the page
                    Frame.Navigate(typeof(DownloadsPage), "Downloads");
                }
                else
                {
                    DialogService.ShowSimpleDialog("The selected path is invalid.", "Path Invalid");
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleDialog("Unknown error: " + ex.Message, "Error");
            }
        }

        private FolderPicker CreateFolderPicker()
        {
            FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(Globals.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");
            return openPicker;
        }

        public async void DownloadBuild_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DownloadBuild.Content == "Cancel")
                {
                    _webClient?.CancelAsync();
                    if (_webClient != null)
                    {
                        _webClient.DownloadProgressChanged -= Wc_DownloadProgressChanged;
                        _webClient.DownloadFileCompleted -= Wc_DownloadFileCompleted;
                        _webClient.Dispose();
                    }
                    await Task.Delay(5000);
                    if (File.Exists(_downloadPathFull))
                    {
                        _webClient = null;
                        try
                        {
                            File.Delete(_downloadPathFull);
                            Frame.Navigate(typeof(DownloadsPage), "Downloads");
                        }
                        catch (Exception ex)
                        {
                            DialogService.ShowSimpleDialog("Failed to download 14.60 build: " + ex.Message, "Error");
                        }
                    }

                    DownloadInProgressInfoBar.IsOpen = false;
                    DownloadBuild.Content = "Download";
                    return;
                }

                _webClient = new WebClient();

                FolderPicker openPicker = CreateFolderPicker();
                StorageFolder folder = await openPicker.PickSingleFolderAsync();

                if (folder != null)
                {
                    long requiredSpace = GetRequiredSpace();
                    long availableSpace = CheckDiskSpace(folder.Path);

                    if (availableSpace >= requiredSpace)
                    {
                        string path = folder.Path;
                        _downloadPathFull = Path.Combine(path, "14.60.rar");
                        _downloadPath = path;
                        _webClient.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        _webClient.DownloadFileCompleted += Wc_DownloadFileCompleted;
                        _webClient.DownloadFileAsync(new Uri("https://cdn.fnbuilds.services/14.60.rar"), _downloadPathFull);

                        DownloadInProgressInfoBar.IsOpen = true;
                        DownloadBuild.Content = "Cancel";
                    }
                    else
                    {
                        DialogService.ShowSimpleDialog("Not enough disk space available for the download.", "Disk Space Error");
                    }
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleDialog("Failed to download 14.60 build: " + ex.Message, "Error");
            }
        }

        private long GetRequiredSpace()
        {
            return 46900000000; // Adjust this value as needed
        }

        private long CheckDiskSpace(string folderPath)
        {
            DriveInfo driveInfo = new DriveInfo(new DirectoryInfo(folderPath).Root.FullName);
            return driveInfo.AvailableFreeSpace;
        }

        private async void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                await WaitForFileToBeReleased(_downloadPathFull);
                DownloadProgress.IsIndeterminate = true;
                using (Stream stream = File.OpenRead(_downloadPathFull))
                {
                    using (var reader = ReaderFactory.Open(stream))
                    {
                        while (reader.MoveToNextEntry())
                        {
                            if (!reader.Entry.IsDirectory)
                            {
                                reader.WriteEntryToDirectory(_downloadPath, new ExtractionOptions()
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                            }
                        }
                    }
                }
            
            
                 File.Delete(_downloadPathFull);
                Definitions.FortnitePath = Globals.m_config.FortnitePath;
                Globals.m_config.FortnitePath = _downloadPath;
                Settings.SaveSettings();
                Frame.Navigate(typeof(DownloadsPage), "Downloads");
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleDialog(ex.Message, "An Error Occurred");
            }

            DownloadInProgressInfoBar.IsOpen = false;
            DownloadBuild.Content = "Download";
        }

        private async void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                _downloadPrecentageProgress = e.ProgressPercentage; // Update the progress
                DownloadProgress.IsIndeterminate = false;
                DownloadProgress.Value = _downloadPrecentageProgress;
                DownloadProgressValue.Text = _downloadPrecentageProgress.ToString() + "%";

                DownloadedGB.Text = $"{StorageSizesConverter.BytesToGigabytes(e.BytesReceived)} GB / 0 GB"; // we really dont know the actual size so lets leave it null

                if (_lastBytes == 0)
                {
                    _lastUpdate = DateTime.Now;
                    _lastBytes = e.BytesReceived;
                    return;
                }

                var now = DateTime.Now;
                var timeSpan = now - _lastUpdate;

                // Check if timeSpan.Seconds is zero to avoid division by zero
                var bytesChange = e.BytesReceived - _lastBytes;
                var bytesPerSecond = timeSpan.Seconds != 0 ? bytesChange / timeSpan.Seconds : 0;

                _lastBytes = e.BytesReceived;
                _lastUpdate = now;
                if (bytesPerSecond == 0)
                {
                    // idk maybe recheck?
                    SpeedBox.Text = $"ERROR MB/s";
                }
                SpeedBox.Text = $"{StorageSizesConverter.BytesToMegabytes(bytesPerSecond)} MB/s";
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleDialog("Failed to download 14.60 build: " + ex.Message, "Error");
            }
        }
    }
}
