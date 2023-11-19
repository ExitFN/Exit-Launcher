using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using FortniteLauncher.Core;
using FortniteLauncher.Interop;
using FortniteLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        string _version = Globals.Version;
        bool _isInitializingPage = false;
        public SettingsPage()
        {
            this.InitializeComponent();
            if (Definitions.outdated == true && Definitions.limitguestfeatures == true)
            {
                LogoutBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Definitions.outdated == true && Definitions.limitguestfeatures == true)
            {
                Refresh.Visibility = Visibility.Collapsed;
            }

            //set sound
            if (Globals.m_config.IsSoundEnabled)
            {
                SoundSwitch.IsOn = true;
            }
        }

        private void SoundSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (_isInitializingPage)
            {
                return;
            }

            if (((ToggleSwitch)sender).IsOn)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                Globals.m_config.IsSoundEnabled = true;
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                Globals.m_config.IsSoundEnabled = false;
            }

            Settings.SaveSettings();
        }

        private async void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Definitions.LoggedOut = true;
            ((Button)sender).IsEnabled = false;
            ProgressRing r =new ProgressRing();
            r.IsIndeterminate = true;
            r.Foreground = new SolidColorBrush(Colors.White);

            ((Button)sender).Content = r;

            //((Button)sender).Background = new Button().Background;

            //here clear variables and anything else that is needed

            //await Task.Delay(250);//temporary

            MainWindow.ShellFrame.Navigate(typeof(LoginPage));
            ((Button)sender).IsEnabled = true;
        }

        private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            if (Definitions.outdated == true && Definitions.limitguestfeatures == true)
            {
                DialogService.ShowSimpleDialog("Please update Exit Launcher to use this feature.", "Error");
            }
            else
            {
                await CheckForUpdatesAPI.Start();
                if (Definitions.APIversion != null)
                {
                    if (Definitions.APIversion != Definitions.CurrentVersion)
                    {
                        DialogService.ShowSimpleDialog("New update detected current version: " + Definitions.CurrentVersion + " Latest version: " + Definitions.APIversion + " please update to continue using Exit launcher.", "New update");
                        Environment.Exit(1);
                    }
                    else
                    {
                        DialogService.ShowSimpleDialog("You are up to date!", "No new update");
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get latest version error: null.", "Error");
                }
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Definitions.RefreshToken = true;
            ((Button)sender).IsEnabled = false;
            ProgressRing r = new ProgressRing();
            r.IsIndeterminate = true;

            ((Button)sender).Content = r;

            //here clear variables and anything else that is needed

            await Task.Delay(250);//temporary

            MainWindow.ShellFrame.Navigate(typeof(LoginPage));
            ((Button)sender).IsEnabled = true;
        }

        private async void RemovePatch_Click(object sender, RoutedEventArgs e)
        {
            string FNPath = Globals.m_config.FortnitePath;

            try
            {
                KillProcessByName("FortniteClient-Win64-Shipping");
                KillProcessByName("ExitClient-Win64-Shipping");
                KillProcessByName("FortniteClient-Win64-Shipping_EAC");
                KillProcessByName("FortniteClient-Win64-Shipping_BE");
                KillProcessByName("FortniteLauncher");

                string easyAntiCheatPath = System.IO.Path.Combine(FNPath, "EasyAntiCheat");
                if (Directory.Exists(easyAntiCheatPath))
                {
                    Directory.Delete(easyAntiCheatPath, true);
                }

                string ExitEACPath = System.IO.Path.Combine(FNPath, "ExitClient-Win64-Shipping.exe");
                if (File.Exists(ExitEACPath))
                {
                    File.Delete(ExitEACPath);
                }

                string aftermathDllPath = System.IO.Path.Combine(FNPath, "Engine", "Binaries", "ThirdParty", "NVIDIA", "NVaftermath", "Win64", "GFSDK_Aftermath_Lib.x64.dll");
                if (File.Exists(aftermathDllPath))
                {
                    File.Delete(aftermathDllPath);
                    using (WebClient webClient = new WebClient())
                    {
                        //await webClient.DownloadFileTaskAsync(Definitions.GFSDKUrl, aftermathDllPath);
                    }
                    DialogService.ShowSimpleDialog("Patch was removed successfully.", "Success");
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowSimpleDialog("An error occurred: " + ex.Message, "Error");
            }
        }

        static void KillProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }
        }
    }
}
