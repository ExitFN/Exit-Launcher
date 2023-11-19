using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using FortniteLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Microsoft.UI.Xaml;
using FortniteLauncher.Services;
using System.Runtime.CompilerServices;

namespace FortniteLauncher.Core.Loops
{
    public class AccountCheck
    {
        public static async Task CheckAccount()
        {
            try
            {
                while (true)
                {
                    MongoDB.User user = new MongoDB.User();
                    if (user != null)
                    {
                        if (user.banned)
                        {
                            DialogService.ShowSimpleDialog("You have been banned.", "Error");
                            Fortnite.KillFnProc();

                            await Task.Delay(250);

                            MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                        }
                    } else
                    {
                        DialogService.ShowSimpleDialog("Error user is null", "Error");
                        Fortnite.KillFnProc();
                        await Task.Delay(250);

                        MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                    }
                    await Task.Delay(1000); 
                }
            } catch { }
        }
    }
}
