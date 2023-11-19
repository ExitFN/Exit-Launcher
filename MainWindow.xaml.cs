using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using FortniteLauncher.Pages;
using System;
using System.ComponentModel;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        public static Frame ShellFrame { get; private set; }
        public static Button TitleBarPaneToggleButton { get; private set; }
        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

            this.SetWindowSize(1200, 725);
            this.CenterOnScreen();
            this.SetIsResizable(false);

            //this might be a bug, but trying to disable maximize makes it even worse
            //this.SetIsMaximizable(false);

            this.Title = "Exit Launcher";

            MicaBackdrop backdrop = new MicaBackdrop();
            backdrop.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;
            this.SystemBackdrop = backdrop;

            ShellFrame = MainWindowFrame;
            TitleBarPaneToggleButton = PaneToggleBtn;

            MainWindowFrame.Navigate(typeof(LoginPage));

            this.SetIcon("Assets\\ACTUALL ICON.ico");
        }
        private void PaneToggleBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainShellPage.STATIC_MainNavigation.IsPaneOpen)
                {

                    MainShellPage.STATIC_MainNavigation.IsPaneOpen = false;
                }
                else
                {

                    MainShellPage.STATIC_MainNavigation.IsPaneOpen = true;
                }
            } catch
            {
                // no.
            }
        }
    }
}
