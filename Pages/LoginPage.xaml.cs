using DnsClient;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using FortniteLauncher.Core;
using FortniteLauncher.Interop;
using FortniteLauncher.MongoDB;
using FortniteLauncher.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public static string RootDirectory = Definitions.RootDirectory;
        public LoginPage()
        {
            this.InitializeComponent();
            Globals.ChangePaneToggleBtnVisibility(false);
        }
        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            await loginAsync(sender, e);
        }

        public async Task loginAsync(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MailBox.Text) || string.IsNullOrWhiteSpace(MailBox.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                //this is a terrible way of doing it, but it works
                StackPanel panel = new StackPanel();
                panel.Spacing = 2;

                TextBlock title = new TextBlock();
                title.Text = "Access Denied";
                title.FontWeight = FontWeights.Medium;

                TextBlock desc = new TextBlock();
                desc.Text = "Email/Password is required.";

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

                ErrorNotification.Show(g, 2500);
            }
            else
            {
                Settings.SaveSettings();
                LoginBtn.IsEnabled = false;
                ProgressRing loading = new ProgressRing();
                loading.IsIndeterminate = true;
                loading.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
                loading.HorizontalAlignment = HorizontalAlignment.Center;
                LoginBtn.Content = loading;
                string Login = await MongoDBAuthenticator.CheckLogin(MailBox.Text, PasswordBox.Password);
                if (Login == "Invalid")
                {
                    ShowPasswordIncorrectError();
                    ((Button)sender).IsEnabled = true;
                    LoginBtn.Content = "Login";

                } else if (Login == "Deny")
                {
                    ToManyReports();
                    ((Button)sender).IsEnabled = true;
                    LoginBtn.Content = "Login";
                }
                else if (Login == "Error")
                {
                    ConnectionFailure();
                    ((Button)sender).IsEnabled = true;
                    LoginBtn.Content = "Login";
                }
                else if (Login == "Banned")
                {
                    UserBanned();
                    ((Button)sender).IsEnabled = true;
                    LoginBtn.Content = "Login";
                }
                else if (Login == "Success")
                {
                    if (Convert.ToBoolean(RememberMeCheckBox.IsChecked))
                    {
                        Globals.m_config.Email = MailBox.Text;
                        Globals.m_config.Password = PasswordBox.Password; // fixed the save settings here

                        Settings.SaveSettings();
                    }
                    await CheckForUpdatesAPI.Start();
                    MainWindow.ShellFrame.Navigate(typeof(MainShellPage), null, new DrillInNavigationTransitionInfo());
                }
                else
                {
                    ConnectionFailure();
                    ((Button)sender).IsEnabled = true;
                    LoginBtn.Content = "Login";
                }
            }
        }

        #region Errors
        public void ShowPasswordIncorrectError()
        {
            //this is a terrible way of doing it, but it works
            StackPanel panel = new StackPanel();
            panel.Spacing = 2;

            TextBlock title = new TextBlock();
            title.Text = "Access Denied";
            title.FontWeight = FontWeights.Medium;

            TextBlock desc = new TextBlock();
            desc.Text = "Your e-mail and/or password are incorrect. Please check them and try again.";

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

            ErrorNotification.Show(g, 2500);
        }

        public void UserBanned()
        {
            try // it crashes so lets put it in a try ig
            {
                //this is a terrible way of doing it, but it works
                StackPanel panel = new StackPanel();
                panel.Spacing = 2;

                TextBlock title = new TextBlock();
                title.Text = "Banned";
                title.FontWeight = FontWeights.Medium;

                TextBlock desc = new TextBlock();
                desc.Text = "Your account has been banned appeal at discord.gg/exitfn";

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

                ErrorNotification.Show(g, 2500);
            }
            catch
            {
                // ignore the error maybe?
            }
        }

        public void ConnectionFailure()
        {
            //this is a terrible way of doing it, but it works
            StackPanel panel = new StackPanel();
            panel.Spacing = 2;

            TextBlock title = new TextBlock();
            title.Text = "Connection Failed";
            title.FontWeight = FontWeights.Medium;

            TextBlock desc = new TextBlock();
            desc.Text = "Exit API is not responding please try again later.";

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

            ErrorNotification.Show(g, 2500);
        }

        public void ToManyReports()
        {
            //this is a terrible way of doing it, but it works
            StackPanel panel = new StackPanel();
            panel.Spacing = 2;

            TextBlock title = new TextBlock();
            title.Text = "Account Locked";
            title.FontWeight = FontWeights.Medium;

            TextBlock desc = new TextBlock();
            desc.Text = "Sorry your account has to many pending reports please contact the support team for further assistance.";

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

            ErrorNotification.Show(g, 2500);
        }

        #endregion

        private void BannerImg_Loaded(object sender, RoutedEventArgs e)
        {
            BannerImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/Banner-dark.png"));
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //MainWindow.ShellFrame.Navigate(typeof(MainShellPage));
            try
            {
                if (Definitions.outdated == true && Definitions.limitguestfeatures == true)
                {
                    LoginBtn.IsEnabled = false;
                    guestlogin();
                } else
                if (Convert.ToBoolean(RememberMeCheckBox.IsChecked))
                {
                    if (Definitions.LoggedOut == true)
                    {
                        MailBox.Text = Globals.m_config.Email;
                        PasswordBox.Password = Globals.m_config.Password;
                    }
                    else if (Definitions.RefreshToken == true)
                    {
                        // not the best way to do it works
                        MailBox.Text = Globals.m_config.Email;
                        PasswordBox.Password = Globals.m_config.Password;
                        LoginBtn.IsEnabled = false;
                        ProgressRing loading = new ProgressRing();
                        loading.IsIndeterminate = true;
                        loading.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
                        loading.HorizontalAlignment = HorizontalAlignment.Center;
                        LoginBtn.Content = loading;
                        await Task.Delay(2000);
                        Definitions.LoggedOut = false;
                        string Login = await MongoDBAuthenticator.CheckLogin(MailBox.Text, PasswordBox.Password);
                        if (Login == "Invalid")
                        {
                            Definitions.LoggedOut = true;
                            DialogService.ShowSimpleDialog("Failed to refresh Account Token.", "Error");
                            ShowPasswordIncorrectError();
                            ((Button)sender).IsEnabled = true;
                            LoginBtn.Content = "Login";

                        }
                    else if (Login == "Deny")
                    {
                        ToManyReports();
                        ((Button)sender).IsEnabled = true;
                        LoginBtn.Content = "Login";
                    }
                    else if (Login == "Error")
                        {
                            Definitions.LoggedOut = true;
                            DialogService.ShowSimpleDialog("Failed to refresh Account Token.", "Error");
                            ConnectionFailure();
                            ((Button)sender).IsEnabled = true;
                            LoginBtn.Content = "Login";
                        }
                        else if (Login == "Banned")
                        {
                            Definitions.LoggedOut = true;
                            DialogService.ShowSimpleDialog("Failed to refresh Account Token.", "Error");
                            UserBanned();
                            LoginBtn.IsEnabled = true;
                            LoginBtn.Content = "Login";
                        }
                        else if (Login == "Success")
                        {
                            DialogService.ShowSimpleDialog("Launcher was successfully refreshed.", "Success");
                            Definitions.RefreshToken = false;
                            if (Convert.ToBoolean(RememberMeCheckBox.IsChecked))
                            {
                                Globals.m_config.Email = MailBox.Text;
                                Globals.m_config.Password = PasswordBox.Password; // fixed the save settings here

                                Settings.SaveSettings();
                            }
                            await CheckForUpdatesAPI.Start();
                            MainWindow.ShellFrame.Navigate(typeof(MainShellPage));
                        }
                        else
                            Definitions.LoggedOut = true;
                        {
                            DialogService.ShowSimpleDialog("Failed to refresh Account Token.", "Error");
                            ConnectionFailure();
                            LoginBtn.IsEnabled = true;
                            LoginBtn.Content = "Login";
                        }
                    }
                    else
                    {
                        if (Globals.m_config.Email != null && Globals.m_config.Password != null)
                        {
                            // not the best way to do it works
                            MailBox.Text = Globals.m_config.Email;
                            PasswordBox.Password = Globals.m_config.Password;
                            LoginBtn.IsEnabled = false;
                            ProgressRing loading = new ProgressRing();
                            loading.IsIndeterminate = true;
                            loading.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
                            loading.HorizontalAlignment = HorizontalAlignment.Center;
                            LoginBtn.Content = loading;
                            await Task.Delay(2000);
                            Definitions.LoggedOut = false;
                            string Login = await MongoDBAuthenticator.CheckLogin(MailBox.Text, PasswordBox.Password);
                            if (Login == "Invalid")
                            {
                                Definitions.LoggedOut = true;
                                ShowPasswordIncorrectError();
                                LoginBtn.IsEnabled = true;
                                LoginBtn.Content = "Login";

                            }
                        else if (Login == "Deny")
                        {
                            ToManyReports();
                            ((Button)sender).IsEnabled = true;
                            LoginBtn.Content = "Login";
                        }
                        else if (Login == "Error")
                            {
                                Definitions.LoggedOut = true;
                                ConnectionFailure();
                                LoginBtn.IsEnabled = true;
                                LoginBtn.Content = "Login";
                            }
                            else if (Login == "Banned")
                            {
                                Definitions.LoggedOut = true;
                                UserBanned();
                                LoginBtn.IsEnabled = true;
                                LoginBtn.Content = "Login";
                            }
                            else if (Login == "Success")
                            {
                                string api = await API.Connect();
                                while (api == null) { await Task.Delay(2000); }
                                if (api == "Success")
                                {
                                if (Convert.ToBoolean(RememberMeCheckBox.IsChecked))
                                {
                                    Globals.m_config.Email = MailBox.Text;
                                    Globals.m_config.Password = PasswordBox.Password; // fixed the save settings here

                                    Settings.SaveSettings();
                                }
                                await CheckForUpdatesAPI.Start();
                                MainWindow.ShellFrame.Navigate(typeof(MainShellPage));
                                } else
                                {
                                    ConnectionFailure();
                                }
                            }
                            else
                            {
                                Definitions.LoggedOut = true;
                                ConnectionFailure();
                                LoginBtn.IsEnabled = true;
                                LoginBtn.Content = "Login";
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        bool bypassWaitTime = false;
        private async void LoginGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            guestlogin();
        }
        void guestlogin()
        {
            //just so i do not need to wait like 8 seconds while testing
            if (MailBox.Text == "CanBypassWaitTimes")
            {
                bypassWaitTime = true;
            }

            ProgressRing loading = new ProgressRing();
            loading.IsIndeterminate = true;
            loading.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
            loading.HorizontalAlignment = HorizontalAlignment.Center;
            LoginGuestBtn.Content = loading;
            Definitions.outdated = true;
            MainShellPage.DisplayUsername = "Guest";

            if (Convert.ToBoolean(RememberMeCheckBox.IsChecked))
            {
                Globals.m_config.Email = MailBox.Text;
                Globals.m_config.Password = PasswordBox.Password; // fixed the save settings here

                Settings.SaveSettings();
            }

            if (bypassWaitTime)
            {

            }
            else
            {
                //await ShowUselessMessagesTask();
            }

            MainWindow.ShellFrame.Navigate(typeof(MainShellPage));

            bypassWaitTime = false;
        }

        private void RevealPasswordCheck_Checked(object sender, RoutedEventArgs e)
        {
            PasswordBox.PasswordRevealMode = PasswordRevealMode.Visible;
        }

        private void RevealPasswordCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
        }
    }
}
