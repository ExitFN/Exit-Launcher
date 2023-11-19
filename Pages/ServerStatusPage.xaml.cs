using CommunityToolkit.Labs.WinUI;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FortniteLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServerStatusPage : Page
    {
        public enum Status
        {
            Online,
            Down,
            Unknown,
            Outdated
        }
        public enum ServerType //idk a better name
        {
            Backend,
            API,
            GameServer
        }
        public ServerStatusPage()
        {
            this.InitializeComponent();

            SetServerStatsAsync();
        }

        public async Task SetServerStatsAsync()
        {
            await GetStatus();   
        }

        public async Task GetStatus()
        {
            if (Definitions.outdated == true && Definitions.limitguestfeatures == true)    
            {
                DialogService.ShowSimpleDialog("Please update Exit Launcher to use this feature.", "Error");
            }
            else
            {
                string Connection = await API.Connect();
                string BackendStatus = await API.BackendStatus();
                bool hasUpdates = await CheckForUpdatesAPI.checkforupdatesAsync();
                if (Connection == "Success")
                {
                    if (hasUpdates == false)
                    {
                        SetStatus(ServerType.API, Status.Outdated);
                    }
                    else
                    {
                        SetStatus(ServerType.API, Status.Online);
                    }
                    if (BackendStatus == "Success")
                    {
                        SetStatus(ServerType.Backend, Status.Online);
                    }
                    else if (BackendStatus == "Error")
                    {
                        SetStatus(ServerType.Backend, Status.Down);
                    }
                    else
                    {
                        DialogService.ShowSimpleDialog("Unknown Error occured while checking backend status.", "Error");
                    }
                }
            }
        }

        void SetStatus(ServerType serverType, Status Status) 
        {
            switch (serverType)
            {
                case ServerType.Backend:
                    switch (Status)
                    {
                        case Status.Online:
                            BackendStatusCard.Description = "Online";
                            BackendBadge.Style = Application.Current.Resources["SuccessDotInfoBadgeStyle"] as Style;
                            break;
                        case Status.Unknown:
                            BackendStatusCard.Description = "Unknown";
                            BackendBadge.Style = Application.Current.Resources["InformationalDotInfoBadgeStyle"] as Style;
                            break;
                        case Status.Down:
                            BackendStatusCard.Description = "Down";
                            BackendBadge.Style = Application.Current.Resources["CriticalDotInfoBadgeStyle"] as Style;
                            break;
                    }
                break;
            }
        }
        private async void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            await GetStatus();
        }
    }
}
