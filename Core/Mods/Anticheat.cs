using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using FortniteLauncher.Pages;
using FortniteLauncher.Services;
using FortniteLauncher.Core.Mods;
using FortniteLauncher.Pages;
using FortniteLauncher.Services;
using SharpCompress.Archives;
using Windows.ApplicationModel;

public class Anticheat
{
    public static async Task<string> CheckForMods(string FNPath)
    {
        if (Directory.Exists(FNPath))
        {
            if (Directory.Exists(Path.Combine(FNPath, "FortniteGame", "Content", "Paks")))
            {
                string directoryPath = Path.Combine(FNPath, "FortniteGame", "Content", "Paks");

                // S14.60
                string[] expectedFiles = new string[]
{
                    Path.Combine(directoryPath, "global.ucas"),
                    Path.Combine(directoryPath, "global.utoc"),
                    Path.Combine(directoryPath, "pakchunk0optional-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk0optional-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk0optional-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk0optional-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk0-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk0-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk0-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk0-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk2-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk2-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk2-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk2-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk5-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk5-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk5-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk5-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk7-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk7-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk7-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk7-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk8-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk8-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk8-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk8-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk9-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk9-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk9-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk9-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s1-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s1-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s1-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s1-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s2-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s2-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s2-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s2-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s3-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s3-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s3-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s3-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s4-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s4-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s4-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s4-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s5-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s5-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s5-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s5-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s6-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s6-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s6-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s6-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s7-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s7-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s7-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s7-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s8-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s8-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s8-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s8-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s9-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s9-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s9-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s9-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s10-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s10-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s10-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s10-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s11-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s11-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s11-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s11-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s12-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s12-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s12-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s12-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s13-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s13-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s13-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s13-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s14-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s14-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s14-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s14-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s15-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s15-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s15-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s15-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s16-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s16-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s16-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s16-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s17-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s17-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s17-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s17-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s18-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s18-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s18-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s18-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s19-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s19-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s19-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s19-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s20-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s20-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s20-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s20-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10_s21-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s21-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s21-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10_s21-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk10-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk10-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk11_s1-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk11_s1-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk11_s1-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk11_s1-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk11-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk11-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk11-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk11-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1000-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1000-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1000-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1000-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1001-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1001-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1001-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1001-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1002-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1002-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1002-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1002-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1003-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1003-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1003-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1003-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1004-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1004-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1004-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1004-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1005-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1005-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1005-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1005-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1006-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1006-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1006-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1006-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakchunk1007-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1007-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1007-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakchunk1007-WindowsClient.utoc"),
                    Path.Combine(directoryPath, "pakChunkEarly-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakChunkEarly-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakChunkEarly-WindowsClient.ucas"),
                    Path.Combine(directoryPath, "pakChunkEarly-WindowsClient.utoc"),
};

                string[] filesInDirectory = Directory.GetFiles(directoryPath);

                bool hasUnexpectedFiles = false;
                long totalSize = 0;
                List<string> unexpectedFiles = new List<string>();
                foreach (string filePath in filesInDirectory)
                {
                    if (Array.IndexOf(expectedFiles, filePath) == -1)
                    {
                        unexpectedFiles.Add(filePath);
                        if (unexpectedFiles.Count > 5)
                        {
                            DialogService.ShowSimpleDialog("Many files are missing please make sure you have selected a 14.60 build.", "Error");
                            return "Error";
                        }
                        Fortnite.KillFnProc();
                        bool result = await DialogService.YesOrNoDialog($"Exit does not support playing with modded paks. Would you like to remove the listed file(s)? {Environment.NewLine} {string.Join(", ", unexpectedFiles)}", "Modified Client");
                        if (result)
                        {
                            try
                            {
                                foreach (string unexpectedFile in unexpectedFiles)
                                {
                                    File.Delete(unexpectedFile);
                                }
                            }
                            catch { }
                        }

                        hasUnexpectedFiles = true;
                        return "Error";
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        totalSize += fileInfo.Length;
                    }
                }

                long expectedSize = 97881153484;

                if (hasUnexpectedFiles)
                {
                    Fortnite.KillFnProc();
                    Console.WriteLine("Error: There are unexpected files in the directory.");
                    return "Error";
                }
                else
                {
                    if (totalSize == expectedSize)
                    {
                        return "Success";
                    }
                    else
                    {
                        Fortnite.KillFnProc();
                        DialogService.ShowSimpleDialog($"Pak check failed invalid size.", "Error");
                        return "Error";
                    }
                }
            }
            else
            {
                Fortnite.KillFnProc();
                DialogService.ShowSimpleDialog("Failed to launch Fortnite. Paks directory not found.", "Error");
                return "Error";
            }
        }
        else
        {
            Fortnite.KillFnProc();
            DialogService.ShowSimpleDialog("Fortnite path is empty. Please check your settings and try again.", "Error");
            return "Error";
        }
    }

    public static async Task<bool> VerifyClient(string FNPath)
    {
        try
        {
            string[] requiredPaths = new string[]
            {
                Path.Combine(FNPath, "ExitClient-Win64-Shipping.exe"),
                Path.Combine(FNPath, "EasyAntiCheat", "Certificates"),
                Path.Combine(FNPath, "EasyAntiCheat", "Licenses"),
                Path.Combine(FNPath, "EasyAntiCheat", "Localization"),
                Path.Combine(FNPath, "EasyAntiCheat", "EasyAntiCheat_EOS_Setup.exe"),
                Path.Combine(FNPath, "EasyAntiCheat", "Settings.json"),
                Path.Combine(FNPath, "EasyAntiCheat", "SplashScreen.png")
            };
            try
            {
                if (File.Exists(FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe") || !File.Exists(FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe"))
                {
                    File.Delete(FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe");
                    await DownloadFile.DownloadFiles(Definitions.BELauncherurl, FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_BE.exe");
                }

                if (File.Exists(FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe") || !File.Exists(FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe"))
                {
                    File.Delete(FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe");
                    await DownloadFile.DownloadFiles(Definitions.Launcherurl, FNPath + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe");
                }


                File.Delete(FNPath + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                await DownloadFile.DownloadFiles(Definitions.RedirectUrl, FNPath + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                if (!File.Exists(Definitions.RootDirectory + "\\Assets\\MemoryLeakFixer.dll"))
                {
                    if (!Directory.Exists(Definitions.RootDirectory + "\\Assets"))
                    {
                        Directory.CreateDirectory(Definitions.RootDirectory + "\\Assets");
                    }
                    //await DownloadFile.DownloadFiles(Definitions.ExitMemoryLeakFixer, Definitions.RootDirectory + "\\Assets\\MemoryLeakFixer.dll");
                }
            }
            catch { }
            //Scan(FNPath);
            foreach (string path in requiredPaths)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    await PatchClient(FNPath);
                    return true;
                }
            }

            File.Delete(FNPath + "\\EasyAntiCheat\\SplashScreen.png");
            File.Delete(FNPath + "\\EasyAntiCheat\\Settings.json");

            using (WebClient client = new WebClient())
            {
                if (Definitions.ExitSplashScreen == null || Definitions.ExitSettingsJson == null)
                {
                    //await LoginPage.GetDownloads();
                    client.DownloadFile(Definitions.ExitSplashScreen, FNPath + "\\EasyAntiCheat\\SplashScreen.png");
                    client.DownloadFile(Definitions.ExitSettingsJson, FNPath + "\\EasyAntiCheat\\Settings.json");
                }
                else
                {
                    client.DownloadFile(Definitions.ExitSplashScreen, FNPath + "\\EasyAntiCheat\\SplashScreen.png");
                    client.DownloadFile(Definitions.ExitSettingsJson, FNPath + "\\EasyAntiCheat\\Settings.json");
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Error while verifying files: " + ex.Message, "Error");
            return false;
        }

        return true;
    }

    static async Task DownloadEACPatch(string FNPath)
    {
        try
        {
            string url = Definitions.AntiCheatUrl;
            string downloadPath = Definitions.RootDirectory + "\\EAC";

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            string extractionPath = FNPath;

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, Path.Combine(downloadPath, "EasyAntiCheat.zip"));
            }

            ZipFile.ExtractToDirectory(Path.Combine(downloadPath, "EasyAntiCheat.zip"), extractionPath);
        }
        catch (WebException ex)
        {
            DialogService.ShowSimpleDialog("Failed to download required files: " + ex.Message, "Webclient Error");
            return;
        }
    }

    static void StartPatching()
    {
        PlayPage.STATIC_MainLaunchCard.Header = "Patching Fortnite";
        PlayPage.STATIC_MainLaunchCard.Description = "Downloading required files...";
    }

    public static async Task<string> PatchClient(string FNPath)
    {
        try
        {
            ProgressRing loading = new ProgressRing();
            loading.IsIndeterminate = true;
            loading.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
            loading.HorizontalAlignment = HorizontalAlignment.Center;

            StartPatching();

            PlayPage.STATIC_MainLaunchCard.Content = loading;
            await Task.Delay(1000);
            Fortnite.KillFnProc();

            if (Directory.Exists(FNPath + "\\EasyAntiCheat"))
            {
                Directory.Delete(FNPath + "\\EasyAntiCheat", true);
            }

            if (File.Exists(FNPath + "\\ExitClient-Win64-Shipping.exe"))
            {
                File.Delete(FNPath + "\\ExitClient-Win64-Shipping.exe");
            }

            await DownloadEACPatch(FNPath);
            string downloadPath = Definitions.RootDirectory + "\\EAC";

            if (Directory.Exists(downloadPath))
            {
                Directory.Delete(downloadPath, true);
            }

            if (File.Exists(downloadPath + "\\EasyAntiCheat.zip"))
            {
                File.Delete(downloadPath + "\\EasyAntiCheat.zip");
            }
            return "Success";
        }
        catch (WebException ex)
        {
            DialogService.ShowSimpleDialog("An error occurred while installing zip: " + ex.Message, "Error");
            return "Error";
        }
    }
}