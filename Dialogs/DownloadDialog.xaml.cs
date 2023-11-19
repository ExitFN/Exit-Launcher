using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using FortniteLauncher.Core;
using FortniteLauncher.Helpers;
using FortniteLauncher.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownloadDialog : Page
    {
        int _downloadPrecentageProgress;

        ContentDialog _presenterContentDialog;
        string _downloadPathFull;
        string _downloadPath;
        DateTime lastUpdate;
        long lastBytes = 0;

        public DownloadDialog(ContentDialog PresenterDialog)
        {
            this.InitializeComponent();

            _presenterContentDialog = PresenterDialog;

            PresenterDialog.CloseButtonText = "Cancel";
            PresenterDialog.CloseButtonClick += PresenterDialog_CloseButtonClick;
        }

        private void PresenterDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();

                //get the path
                FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();

                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                var window = Globals.m_window;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                // Initialize the folder picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

                // Set options for your file picker
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add("*");

                // Open the picker for the user to pick a file
                StorageFolder folder = await openPicker.PickSingleFolderAsync();

                if (folder == null)
                {
                    _presenterContentDialog.Hide();
                    return;
                }

                string path = folder.Path;
                _downloadPathFull = path + "\\8.20.zip";

                //does this actually improve download speed?
                //System.Net.ServicePointManager.DefaultConnectionLimit = 

                DownloadState.Text = "Downloading";

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(new Uri("https://cdn.fnbuilds.services/14.60.rar"), _downloadPathFull);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occured");
                throw;
            }
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadProgress.IsIndeterminate = true;
            DownloadState.Text = "Extracting";
            ZipFile.ExtractToDirectory(_downloadPathFull, _downloadPath);

            DownloadState.Text = "Cleaning Up";
            File.Delete(_downloadPathFull);

            try
            {
                Definitions.FortnitePath = Globals.m_config.FortnitePath;
                Globals.m_config.FortnitePath = _downloadPath;

                Settings.SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
                throw;
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                DownloadProgress.IsIndeterminate = false;
                DownloadProgress.Value = e.ProgressPercentage;
                DownloadProgressValue.Text = e.ProgressPercentage.ToString() + "%";

            _presenterContentDialog.Title = (Helpers.StorageSizesConverter.BytesToGigabytes(e.BytesReceived).ToString() + "GB / " + Helpers.StorageSizesConverter.BytesToGigabytes(e.TotalBytesToReceive).ToString() + "GB");

                if (lastBytes == 0)
                {
                    lastUpdate = DateTime.Now;
                    lastBytes = e.BytesReceived;
                    return;
                }

                var now = DateTime.Now;
                var timeSpan = now - lastUpdate;
                var bytesChange = e.BytesReceived - lastBytes;
                var bytesPerSecond = bytesChange / timeSpan.Seconds;

                lastBytes = e.BytesReceived;
                lastUpdate = now;

                SpeedBox.Text = (StorageSizesConverter.BytesToMegabytes(bytesPerSecond).ToString()) + " MB/s";
            } catch
            {
                // fix crash??
            }
        }
    }
}
