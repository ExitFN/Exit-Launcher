using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.WindowsAppSDK.Runtime.Packages;
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
using Windows.System;
using WinUIEx.Messaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayPage : Page
    {
        bool _isLaunchingTheGame = false;
        string _username = string.Empty;

        public static SettingsCard STATIC_MainLaunchCard;

        public PlayPage()
        {
            this.InitializeComponent();
            _username = "Hi, " + MainShellPage.DisplayUsername + "!";
            STATIC_MainLaunchCard = MainLaunchCard;
            if (personPicture.ProfilePicture == null) // dont work as expected
            {
                personPicture.ProfilePicture = new BitmapImage(new Uri("ms-appx:///Assets/PFP.png"));
            }
            //API.StartClient(); // to many risks on adding this
            image();
        }

        /*
        public static void ShowNotification(string titlee, string text)
        {
            try // it crashes so lets put it in a try ig
            {
                //this is a terrible way of doing it, but it works
                StackPanel panel = new StackPanel();
                panel.Spacing = 2;

                TextBlock title = new TextBlock();
                title.Text = titlee;
                title.FontWeight = FontWeights.Medium;

                TextBlock desc = new TextBlock();
                desc.Text = text;

                panel.Children.Add(title);
                panel.Children.Add(desc);

                Grid g = new Grid();
                ColumnDefinition def1 = new ColumnDefinition();
                ColumnDefinition def2 = new ColumnDefinition();
                def1.Width = GridLength.Auto;
                def2.Width = GridLength.Auto;
                g.ColumnDefinitions.Add(def1);
                g.ColumnDefinitions.Add(def2);

                FontIcon icon = new FontIcon();
                icon.VerticalAlignment = VerticalAlignment.Center;
                icon.Glyph = "\uE72E";
                icon.FontSize = 28;
                icon.Margin = new Thickness(0, 0, 12, 0);

                g.Children.Add(icon);
                g.Children.Add(panel);

                Grid.SetColumn(icon, 0);
                Grid.SetColumn(panel, 1);

                Notification.Show(g, 2500);
            }
            catch
            {
                // ignore the error maybe?
            }
        }
        */
        async void image()
        {
            while (Definitions.Email != null)
            {
                string url = await API.GetSkin();
                try
                {
                    if (url == "Error")
                    {
                        BitmapImage image = new BitmapImage();
                        image.UriSource = new Uri("http://fortnite-api.com/images/cosmetics/br/CID_001_Athena_Commando_F_Default/smallicon.png");

                        personPicture.ProfilePicture = image;
                    } else
                    {
                    BitmapImage image = new BitmapImage();
                    image.UriSource = new Uri(url);

                    personPicture.ProfilePicture = image;
                    }

                }
                catch (Exception ex)
                {
                    // idek
                    DialogService.ShowSimpleDialog("Failed to get Profile Picture: " + ex.Message + " url: " + url, "Error");
                }
                await Task.Delay(10000);
            }
        }
        public async void LaunchBtn_Click(object sender, RoutedEventArgs e)
        {
            // idk
            /*
            while (Definitions.FinishedAPIRequest == false)
            {
                if (Definitions.FinishedAPIRequest == true) { break; }
                ((SettingsCard)sender).IsEnabled = false;
                ProgressRing r = new ProgressRing();
                r.IsIndeterminate = true;

                ((SettingsCard)sender).Content = r;
                await Task.Delay(3000);
            }
            */
            if (Definitions.BindPlayButton)
            {
                if (!Definitions.ConnectionFailedToAPI)
                {
                    string RootDirectory = Definitions.RootDirectory;
                    if (MainLaunchCard.Tag.ToString().Contains("Launch"))
                    {
                        if (Definitions.outdated == true && Definitions.limitguestfeatures == true)
                        {
                            DialogService.ShowSimpleDialog("Please update Exit Launcher to use this feature.", "Error");
                        }
                        else
                        {
                            if (_isLaunchingTheGame)
                            {
                                return;
                            }

                            _isLaunchingTheGame = true;

                            ProgressRing r = new ProgressRing();
                            r.IsIndeterminate = true;

                            ((SettingsCard)sender).Content = r;
                            string basefn = Definitions.FortnitePath;
                            if (basefn == null)
                            {
                                if (Globals.m_config.FortnitePath != null)
                                {
                                    basefn = Globals.m_config.FortnitePath;
                                }
                                else
                                {
                                    DialogService.ShowSimpleDialog("Fortnite path is not set please check your installation and try again.", "Path Invalid");
                                }
                            }
                            if (basefn != null)
                            {
                                if (!Definitions.AllowLaunchingAnyVersion)
                                {
                                    string CheckFiles = await Anticheat.CheckForMods(basefn);
                                    bool VerifyClient = await Anticheat.VerifyClient(basefn);
                                    try
                                    {
                                        if (CheckFiles == "Success")
                                        {
                                            if (VerifyClient)
                                            {
                                            await Task.Delay(600); //delay for a little
                                            await Fortnite.Launch(basefn, Definitions.Email, Definitions.Password);                                           
                                            _isLaunchingTheGame = false;
                                            }
                                        }
                                        else
                                        {
                                            _isLaunchingTheGame = false;
                                            ((SettingsCard)sender).Content = null;
                                            return;
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        DialogService.ShowSimpleDialog("Failed to launch Fortnite: " + ex.Message, "Error");
                                    }
                                }
                                else
                                { 
                                    await Task.Delay(600); //delay for a little

                                    _isLaunchingTheGame = false;
                                    await Fortnite.Launch(basefn, Definitions.Email, Definitions.Password);

                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            Process[] gameProcesses = Process.GetProcessesByName("FortniteClient-Win64-Shipping");
                            foreach (Process process in gameProcesses)
                            {
                                process.Kill();
                            }

                        }
                        catch
                        {
                            // ignore
                        }
                        MainLaunchCard.Header = "Launch Season 4";
                        MainLaunchCard.Description = "Launch Fortnite Chapter 2 Season 4 powered by Exit";

                        FontIcon icon = new FontIcon();
                        icon.Glyph = "\uE768";
                        MainLaunchCard.HeaderIcon = icon;

                        MainLaunchCard.Tag = "Launch";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to launch Fortnite the connection to the API failed, please restart the launcher and try again.", "Error");
                }
            }
        }
        public static void KillProcessByName(string processName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(processName);

                foreach (Process process in processes)
                {
                    process.Kill(); // Kill the process
                    process.WaitForExit(); // Wait for the process to exit
                    process.Dispose(); // Release resources associated with the process
                }
            }
            catch
            {
                // idk
            }
        }
        private void DiscordBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DonationsCard_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://Exitfn.sellix.io/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }

        private void DiscordCard_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://discord.gg/exitfn";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }
    }
}
