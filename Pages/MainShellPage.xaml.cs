using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using FortniteLauncher.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;
using static FortniteLauncher.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainShellPage : Page
    {
        public static string DisplayUsername;

        string _username;

        public static NavigationView STATIC_MainNavigation;
        public MainShellPage()
        {
            this.InitializeComponent();
            InitializeNavigationService(MainNavigation, MainBreadcrumb, RootFrame);

            _username = DisplayUsername;

            MainNavigation.SelectedItem = PlayPageItem;

            Globals.ChangePaneToggleBtnVisibility(true);

        }

    private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigationService.Navigate(typeof(SettingsPage), "Settings", true);
            }
            if ((args.SelectedItem as NavigationViewItem) == PlayPageItem)
            {
                NavigationService.Navigate(typeof(PlayPage), "Play", true);
                ChangeBreadcrumbVisibility(false);
            }
            if ((args.SelectedItem as NavigationViewItem) == DownloadsItem)
            {
                NavigationService.Navigate(typeof(DownloadsPage), "Downloads", true);
            }
            if ((args.SelectedItem as NavigationViewItem) == ItemShopItem)
            {
                NavigationService.Navigate(typeof(ItemShopPage), "Item Shop", true);
            }
            if ((args.SelectedItem as NavigationViewItem) == ServerStatusItem)
            {
                NavigationService.Navigate(typeof(ServerStatusPage), "Server Status", true);
            }
            ElementSoundPlayer.Play(ElementSoundKind.Invoke);
        }

        private void MainBreadcrumb_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            if (args.Index < NavigationService.BreadCrumbs.Count - 1)
            {
                var crumb = (Breadcrumb)args.Item;
                crumb.NavigateToFromBreadcrumb(args.Index);
            }
        }

        private void PaneContent_Click(object sender, RoutedEventArgs e)
        {
            MainNavigation.SelectedItem = null;
            NavigationService.Navigate(typeof(SettingsPage), "Settings", true);
        }

        private void MainNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            STATIC_MainNavigation = MainNavigation;
        }
    }
}
