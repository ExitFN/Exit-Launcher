using FortniteLauncher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class CheckForUpdatesAPI
{
    public static string CurrentVersion = Definitions.CurrentVersion;

    public static async Task Start()
    {
        string connection = await API.Connect();
        string apiver = await API.Version();
        if (connection == "Success")
        {
            Definitions.APIversion = apiver;
            checkforupdatesAsync();
        } else if (connection == "Error")
        {
            DialogService.ShowSimpleDialog("An Error occured while checking for updates.", "Error");
        } else
        {
            DialogService.ShowSimpleDialog("Unknown API data returned.", "Error");
        }
    }

    public static async Task<bool> checkforupdatesAsync()
    {
        if (Definitions.APIversion != CurrentVersion)
        {
            DialogService.ShowSimpleDialog("A new update was detected latest version: " + Definitions.APIversion + " current version: " + Definitions.CurrentVersion, "outdated");
            Definitions.outdated = true;
            return Definitions.APIversion != CurrentVersion;
        } else
        {
            return true;
        }
    }
}