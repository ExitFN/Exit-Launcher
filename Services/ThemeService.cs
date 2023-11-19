using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteLauncher.Services
{
    public class ThemeService
    {
        public static void ChangeTheme(ElementTheme theme)
        {
            if (Globals.m_window.Content is FrameworkElement frameworkElement)
            {
                //because of the last- minute change of the colors of the launcher, i have to do this as it breaks the color
                //frameworkElement.RequestedTheme = theme;
            }
        }
    }
}
