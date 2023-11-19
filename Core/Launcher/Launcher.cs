using ABI.Windows.ApplicationModel.Activation;
using Amazon.Auth.AccessControlPolicy;
using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes; // Make sure this is fully qualified.
using MongoDB.Driver.Core.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Gaming.Input;
using Windows.UI.ViewManagement;
using WinUIEx;
using static CommunityToolkit.WinUI.UI.Animations.Expressions.ExpressionValues;
using FortniteLauncher.Pages;
using FortniteLauncher.Services;
using FortniteLauncher.Core.Mods;

public class Fortnite
{
    public static async Task Launch(string GamePath, string Email, string Password)
    {
        try
        {
            // Delete the BattlEye folder
            //string battlEyeFolderPath = System.IO.Path.Combine(GamePath, "FortniteGame", "Binaries", "Win64", "BattlEye"); // Fully qualify System.IO.Path

            //if (Directory.Exists(battlEyeFolderPath))
            {
                //Directory.Delete(battlEyeFolderPath, true);
            }

            // Delete the FortniteClient-Win64-Shipping_BE.exe file
            //string exeFilePath_BE = System.IO.Path.Combine(GamePath, "FortniteGame", "Binaries", "Win64", "FortniteClient-Win64-Shipping_BE.exe"); // Fully qualify System.IO.Path

            //if (File.Exists(exeFilePath_BE))
            {
                //File.Delete(exeFilePath_BE);
            }

            // Delete the FortniteClient-Win64-Shipping_EAC.exe file
            //string exeFilePath_EAC = System.IO.Path.Combine(GamePath, "FortniteGame", "Binaries", "Win64", "FortniteClient-Win64-Shipping_EAC.exe"); // Fully qualify System.IO.Path

            //if (File.Exists(exeFilePath_EAC))
            {
                //File.Delete(exeFilePath_EAC);
            }

            WhitelistedProcesses.ProcessCheckLoops();
            KillFnProc();
            CrashHandler.Start();
            //pakcheck(GamePath);
            StartLauncherProcesses(GamePath, Email, Password);
            PlayPage.STATIC_MainLaunchCard.Header = "Close Exit";
            PlayPage.STATIC_MainLaunchCard.Description = "Click here to close Exit. If you're experiencing issues, please restart your computer.";
            PlayPage.STATIC_MainLaunchCard.Content = null;
            FontIcon icon2 = new FontIcon();
            icon2.Glyph = "\uE8BB";
            PlayPage.STATIC_MainLaunchCard.HeaderIcon = icon2;
            Definitions.BindPlayButton = false;
            PlayPage.STATIC_MainLaunchCard.Click += STATIC_MainLaunchCard_CloseClicked;
        }
        catch (Exception ex)
        {
            Error();
            DialogService.ShowSimpleDialog("Failed to start Fortnite process: " + ex, "Error");
        }
    }

    public static void Error()
    {
        KillFnProc();
        PlayPage.STATIC_MainLaunchCard.Header = "Launch Season 4";
        PlayPage.STATIC_MainLaunchCard.Description = "Launch Fortnite Chapter 2 Season 4 powered by Exit";

        FontIcon icon = new FontIcon();
        icon.Glyph = "\uE768";
        PlayPage.STATIC_MainLaunchCard.HeaderIcon = icon;
        PlayPage.STATIC_MainLaunchCard.Tag = "Launch";
        PlayPage.STATIC_MainLaunchCard.Content = null;
        Definitions.BindPlayButton = true;
        return;
    }

    public static void STATIC_MainLaunchCard_CloseClicked(object sender, RoutedEventArgs e)
    {
        if (!Definitions.BindPlayButton)
        {
            try
            {
                Error();
            }
            catch { }
        }
    }

    private static async Task StartLauncherProcesses(string gamePath, string email, string password)
    {
        try
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = gamePath + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe",
                CreateNoWindow = true,
                UseShellExecute = false
            });

            Process.Start(new ProcessStartInfo()
            {
                FileName = gamePath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe",
                CreateNoWindow = false, // Set to false to show the window
                UseShellExecute = false
            });

            Process launcherProcess = new Process
            {
                StartInfo = new ProcessStartInfo(gamePath + "\\EasyAntiCheat\\EasyAntiCheat_EOS_Setup.exe")
                {
                    Arguments = "install \"ef7b6dadbcdf42c6872aa4ad596bbeaf\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            launcherProcess.Start();

            Process gameProcess = new Process
            {
                StartInfo = new ProcessStartInfo(gamePath + "\\ExitClient-Win64-Shipping.exe")
                {
                    Arguments = $"-AUTH_LOGIN={email} -AUTH_PASSWORD={password} -AUTH_TYPE=epic -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fltoken=3db3ba5dcbd2e16703f3978d -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiYmU5ZGE1YzJmYmVhNDQw7DBjnzDnXyyEnX7OljJm-j2d88G_WgwQ9wrE6lwMEHZHjBd1ISJdUO1UVUqkfLdU5nofBQ -fromfl=eac",
                    RedirectStandardOutput = true
                }
            };
            gameProcess.EnableRaisingEvents = true;
            gameProcess.Exited += GameProcessExited;
            gameProcess.StartInfo.RedirectStandardOutput = true;
            gameProcess.StartInfo.UseShellExecute = false;
            gameProcess.Start();
        }
        catch (Exception ex)
        {
            Error();
            DialogService.ShowSimpleDialog("An Error occured while launching Fortnite: " + ex.Message, "Error");
            return;
        }
    }

    static void GameProcessExited(object sender, EventArgs e)
    {
        KillFnProc();
        try
        {
            //Definitions.MainWindow.Restore(); // it dont work ):
        }
        catch (Exception ex)
        {
            //DialogService.ShowSimpleDialog("Error while restoring window: " + ex, "Error"); // DO NOT UNCOMMENT!!!
        }
    }

    static void KillProcessByName(string processName)
    {
        Process[] processes = Process.GetProcessesByName(processName);

        foreach (Process process in processes)
        {
            try
            {
                process.Kill();
                process.WaitForExit();
            }
            catch
            {
            }
        }
    }
    public static void KillFnProc()
    {
        KillProcessByName("FortniteClient-Win64-Shipping");
        KillProcessByName("FortniteClient-Win64-Shipping_BE");
        KillProcessByName("FortniteClient-Win64-Shipping_EAC");
        KillProcessByName("ExitClient-Win64-Shipping");
        KillProcessByName("FortniteLauncher");
        KillProcessByName("EpicGamesLauncher");
        KillProcessByName("CrashReportClient");
    }
}