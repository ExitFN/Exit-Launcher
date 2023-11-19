using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FortniteLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteLauncher
{
    public class Globals
    {
        public static Window m_window;

        public static Config m_config;

        public static string Version = Definitions.CurrentVersion;

        public static void ChangePaneToggleBtnVisibility(bool IsVisible)
        {
            Button TitleBarPaneToggleBtn = MainWindow.TitleBarPaneToggleButton;

            if (IsVisible)
            {
                TitleBarPaneToggleBtn.Visibility = Visibility.Visible;
            }
            else
            {
                TitleBarPaneToggleBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
