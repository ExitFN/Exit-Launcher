using MongoDB.Bson.IO;
using FortniteLauncher;
using System;

internal class Definitions
{
    // put all of your shit here so it automaticly downloads and redirects and shit
    public static string CurrentVersion = "1.0.0";
    public static string APIurl = "http://yourip:yourport/v1/";
    public static string RootDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "\\FortniteLauncher";
    public static string BELauncherurl = "";
    public static string Launcherurl = null;
    public static string RedirectUrl = "";
    public static string AntiCheatUrl = "";
    public static string ExitSplashScreen = "";
    public static string ExitSettingsJson = "";
    public static string APIversion = null;
    public static string UserName = null;
    public static string FortnitePath = null;
    public static string Email = null;
    public static string Password = null;
    public static bool EOR = false;
    public static bool DiscordPRC = true;
    public static bool LoggedOut = false;
    public static bool RefreshToken = false;
    public static bool ForceClose = false;
    public static bool outdated = false;
    public static bool FinishedAPIRequest = false;
    public static bool BindPlayButton = true;
    public static bool ConnectionFailedToAPI = false;

    // options
    public static bool limitguestfeatures = true; // set to false to enable all features for guests.
    public static bool AllowLaunchingAnyVersion = true; // false is limited to 14.60 if set to true it will allow launching any version 12.41 is the highest version tested.

    //secrets
    public static string accessToken = "31811a3e1afdb125efa4151438f29928a5dcf6b403c13f0a2dde440bca545654";
}
