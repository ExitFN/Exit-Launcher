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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            ExitRPC.Start();
            bool createdNew;
            Mutex mutex = new Mutex(true, "ExitLauncher", out createdNew);

            if (createdNew)
            {
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Exit Launcher is already running. Please close it before opening a new instance.", "Already Running");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();

            Globals.m_window = m_window;

            Settings.LoadSettings();

            ThemeService.ChangeTheme(ElementTheme.Dark);//force the dark theme
            if (Globals.m_config.IsSoundEnabled)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
            }
        }

        private Window m_window;

    }
}
