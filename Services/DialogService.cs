using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using System.Threading;

namespace FortniteLauncher.Services
{
    public class DialogService
    {
        public static async void ShowSimpleDialog(object Content, string Title)
        {
            try { 
            //there can only be one dialog open at once, so if it exist just close it
            var openedpopups = VisualTreeHelper.GetOpenPopups(Globals.m_window);
            foreach (var popup in openedpopups)
            {
                if (popup.Child is ContentDialog)
                {
                    return;
                }
            }
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.m_window.Content.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = Title;
            dialog.Content = Content;

            dialog.CloseButtonText = "OK";

             await dialog.ShowAsync();
            } catch
            {
                // ignore the error maybe?
            }
        }

        public static async void CurcleLoading(string message)
        {
            try
            {
                // Close any existing dialogs
                var openedpopups = VisualTreeHelper.GetOpenPopups(Globals.m_window);
                foreach (var popup in openedpopups)
                {
                    if (popup.Child is ContentDialog)
                    {
                        return;
                    }
                }

                // Create a ContentDialog
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = Globals.m_window.Content.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.CloseButtonText = ""; // No close button

                // Create a StackPanel to hold the ProgressRing and TextBlock
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;

                // Add a ProgressRing
                ProgressRing progressRing = new ProgressRing();
                progressRing.IsActive = true;
                progressRing.HorizontalAlignment = HorizontalAlignment.Center;
                progressRing.Margin = new Thickness(0, 0, 0, 10); // Adjust margin as needed
                stackPanel.Children.Add(progressRing);

                // Add a TextBlock with the specified message
                TextBlock textBlock = new TextBlock();
                textBlock.Text = message;
                textBlock.Margin = new Thickness(0, 0, 0, 0); // Adjust margin as needed
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel.Children.Add(textBlock);

                // Set the Content of the dialog to the StackPanel
                dialog.Content = stackPanel;

                // Measure and arrange the dialog's content to fit its size
                dialog.UpdateLayout();

                // Calculate the desired width and height based on content size
                double desiredWidth = stackPanel.DesiredSize.Width;
                double desiredHeight = stackPanel.DesiredSize.Height;

                // Set the dialog's size based on the desired width and height
                dialog.Width = desiredWidth;
                dialog.Height = desiredHeight;

                // Show the dialog
                await dialog.ShowAsync();
            }
            catch
            {
                // Ignore the error maybe?
            }
        }


        public static async Task<bool> YesOrNoDialog(object Content, string Title)
        {
            try
            {
                // There can only be one dialog open at once, so if it exists, just close it
                var openedPopups = VisualTreeHelper.GetOpenPopups(Globals.m_window);
                foreach (var popup in openedPopups)
                {
                    if (popup.Child is ContentDialog)
                    {
                        return false; // Return false to indicate the dialog wasn't shown
                    }
                }

                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = Globals.m_window.Content.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = Title;
                dialog.Content = Content;

                dialog.PrimaryButtonText = "Yes";
                dialog.CloseButtonText = "No";

                ContentDialogResult result = await dialog.ShowAsync();
                return result == ContentDialogResult.Primary; // Return true for "Yes" and false for "No"
            }
            catch
            {
                return false;
            }
        }
        
        /*
        public static async Task<bool> DownloadProgressDialog(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                var openedPopups = VisualTreeHelper.GetOpenPopups(Globals.m_window);
                foreach (var popup in openedPopups)
                {
                    if (popup.Child is ContentDialog)
                    {
                        return false; // Return false to indicate the dialog wasn't shown
                    }
                }

                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = Globals.m_window.Content.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Downloading";

                StackPanel contentPanel = new StackPanel();
                contentPanel.VerticalAlignment = VerticalAlignment.Stretch;

                ProgressBar progressBar = new ProgressBar();
                progressBar.IsIndeterminate = true; // You can set this to false and update the value for a determinate progress bar
                contentPanel.Children.Add(progressBar);

                Button cancelButton = new Button();
                cancelButton.Content = "Cancel";
                cancelButton.Style = Application.Current.Resources["CancelButtonStyle"] as Style; // Create a style in your application resources
                cancelButton.Click += (sender, args) =>
                {
                    cancellationTokenSource.Cancel(); // Cancel the download operation
                    dialog.Hide();
                };
                contentPanel.Children.Add(cancelButton);

                dialog.Content = contentPanel;

                ContentDialogResult result = await dialog.ShowAsync();
                return result == ContentDialogResult.Primary; // Return true for "Yes" (download completed) and false for "No" (download canceled)
            }
            catch
            {
                return false;
            }
        }
        */

        public static ContentDialog CreateStyledContentDialog()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = Globals.m_window.Content.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

            return dialog;
        }
    }
}
