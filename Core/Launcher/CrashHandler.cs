using FortniteLauncher;
using FortniteLauncher.Interop;
using FortniteLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class CrashHandler
{
    public static async Task Start()
    {
        string processName = "CrashReportClient";
        bool isProcessRunning;
        int crashes = 5;
        while (true)
        {
            isProcessRunning = IsProcessRunning(processName);

            if (isProcessRunning)
            {
                Fortnite.KillFnProc();
                bool repairfiles = await DialogService.YesOrNoDialog("Fortnite has crashed would you like to repair the files? Note: All in game settings will be reset!", "Fatal Error");
                crashes++;
                if (crashes < 5)
                {
                    bool sendlogs = await DialogService.YesOrNoDialog("Fortnite has crashed to many times please create a ticket in the Discord server and provide them with the zip file.", "Fatal Error");
                    if (sendlogs)
                    {
                        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        Fortnite.KillFnProc();

                        // Define the source directory to be zipped
                        string sourceDirectory = localAppData + "\\FortniteGame";

                        if (Directory.Exists(sourceDirectory))
                        {
                            // Define the path to the log file to be included in the zip
                            string logFilePath = sourceDirectory + "\\Saved\\Logs\\FortniteGame.log";

                            if (File.Exists(logFilePath))
                            {
                                string CurrentDirectory = Directory.GetCurrentDirectory();
                                if (!Directory.Exists(CurrentDirectory + "\\Support\\Logs"))
                                {
                                    Directory.CreateDirectory(CurrentDirectory + "\\Support\\Logs");
                                }
                                string zipFilePath = CurrentDirectory + "\\Support\\Logs\\";
                                Process.Start(zipFilePath);
                                ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);
                            }
                        }
                    }
                    return;
                }
                if (repairfiles)
                {
                    try
                    {
                        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        Fortnite.KillFnProc();
                        if (Directory.Exists(localAppData + "\\FortniteGame"))
                        {
                            Directory.Delete(localAppData + "\\FortniteGame", true);
                        }
                        if (Directory.Exists(Globals.m_config.FortnitePath + "\\EasyAntiCheat"))
                        {
                            Directory.Delete(Globals.m_config.FortnitePath + "\\EasyAntiCheat", true);
                        }
                        if (File.Exists(Globals.m_config.FortnitePath + "\\ExitClient-Win64-Shipping.exe"))
                        {
                            File.Delete(Globals.m_config.FortnitePath + "\\ExitClient-Win64-Shipping.exe");
                        }
                        if (File.Exists(Globals.m_config.FortnitePath + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                        {
                            File.Delete(Globals.m_config.FortnitePath + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                        }
                        await Task.Delay(5000);
                        DialogService.ShowSimpleDialog("Fortnite files have been repaired successfully.", "Success");
                    } catch { }

                }
                break;
            }
            await Task.Delay(1000);
        }
    }


    static bool IsProcessRunning(string processName)
    {
        Process[] processes = Process.GetProcessesByName(processName);

        return processes.Length > 0;
    }
}