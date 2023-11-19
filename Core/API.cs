using Microsoft.UI.Xaml;
using FortniteLauncher.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FortniteLauncher.Interop;
using FortniteLauncher.MongoDB;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FortniteLauncher.Pages;
using FortniteLauncher;

public class API
{
    public static string apiUrl = Definitions.APIurl;
    private static string accessToken = Definitions.accessToken;

    private static HttpClient CreateHttpClient()
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return httpClient;
    }
    public static async Task<string> Connect()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "test");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        return "Success";
                    }
                    else
                    {
                        DialogService.ShowSimpleDialog("Gotten invalid data from the API please try again later.", "API Error");
                        return "Error";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("API connection failed please check your internet connection and try again.", "API Connect Error");
                    return "Error";
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Connect Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
    }

    public static async Task<string> Version()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "version"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            return null;
                        }
                        return content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get latest version please try again later.", "API Error");
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Version Error");
            //Debug.WriteLine($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
        return "Error";
    }

    public static async Task<string> BackendStatus()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "backendstatus"); // the rest of the API URL goes in ""
                string content = await response.Content.ReadAsStringAsync();
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    if (content == "Success")
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to check Backend status please try again later.", "API Error");
                    return "Error";
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API BS Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
    }

    public static async Task<string> GameServerStatus()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "gamestatus");// not working
                string content = await response.Content.ReadAsStringAsync();
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    if (content == "Success")
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("GameServer Status is unavailable please try again later.", "API Error");
                    return "Error";
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API GS Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
    }

    public static async Task<string> GetKey()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "GetKey");
                string content = await response.Content.ReadAsStringAsync();
                if (content == "Invalid authorization token")
                {
                    return null;
                }
                return content;
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Key Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
    }

    public static async Task<string> MongoDBConnectionString()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + $"GetConnectionString?Key={await GetKey()}");
                string content = await response.Content.ReadAsStringAsync();
                // Check if the request was successful
                if (content == "key not found." || content == "Invalid authorization token")
                {
                    DialogService.ShowSimpleDialog("Exit Launcher is outdated please update to continue using Exit Launcher!", "Old Authentication");
                }
                else
                {
                    return content;
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API String Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
    }

    public static async Task<string> GetAccountId()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                while (Definitions.Email == null)
                {
                    await Task.Delay(1000); // avoid freezing
                }

                if (Definitions.Password != null)
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl + $"GetAccountId?email={Definitions.Email}&password={Definitions.Password}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response to get the accountId
                        JObject jsonResponse = JObject.Parse(content);
                        string accountId = jsonResponse["accountId"].ToString();

                        return accountId;
                    }
                    else
                    {
                        ///DialogService.ShowSimpleDialog("Failed to get account profile possibly because your account was deleted.", "API Error");
                        //MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                    }

                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API ID Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
        return "Error";
    }

    public static async Task<string> GetSkin()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                while (Definitions.Email == null)
                {
                    await Task.Delay(1000); // avoid freezing
                }

                if (Definitions.Password != null)
                {
                    string accountId = await GetAccountId(); // Retrieve the accountId

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl + $"GetSkin?Key={accountId}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();
                        if (content == "No username provided.")
                        {
                            DialogService.ShowSimpleDialog("Failed to get current skin username is null.", "API Error");
                        }
                        else if (content == "User not found." || content == "Invalid authorization token")
                        {
                            DialogService.ShowSimpleDialog("Failed to authenticate user.", "API Error");
                            MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(content) && content.StartsWith("AthenaCharacter:"))
                            {
                                string trimmedValue = content.Substring("AthenaCharacter:".Length);
                                string url = "http://fortnite-api.com/images/cosmetics/br/" + trimmedValue + "/smallicon.png";
                                return url;
                            }
                            else if (!string.IsNullOrEmpty(content) && content.StartsWith("ApolloCharacter:"))
                            {
                                string trimmedValue = content.Substring("ApolloCharacter:".Length);
                                string url = "http://fortnite-api.com/images/cosmetics/br/" + trimmedValue + "/smallicon.png";
                                return url;
                            }

                            return "Error";
                        }
                    }
                    else
                    {
                        //DialogService.ShowSimpleDialog("Failed to get account profile possibly because your account was deleted.", "API Error");
                        //MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                    }

                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Skin Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
        return "Error";
    }

    public static async Task<Dictionary<string, int>> GetStats()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                while (Definitions.Email == null)
                {
                    await Task.Delay(1000); // avoid freezing
                }

                if (Definitions.Password != null)
                {
                    string accountId = await GetAccountId(); // Retrieve the accountId

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl + $"GetStats?Key={accountId}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();

                        // Use regular expressions to parse the response string
                        var statsDict = new Dictionary<string, int>();
                        var matches = Regex.Matches(content, @"(\w+):\s*(\d+)");
                        foreach (Match match in matches)
                        {
                            string statName = match.Groups[1].Value;
                            int statValue = Convert.ToInt32(match.Groups[2].Value);
                            statsDict[statName] = statValue;
                        }

                        // Check if the required keys exist
                        if (statsDict.ContainsKey("Top25") && statsDict.ContainsKey("Top10") && statsDict.ContainsKey("Top5"))
                        {
                            return statsDict;
                        }
                        else
                        {
                            // Handle the case where the required keys are missing
                            DialogService.ShowSimpleDialog("Response is missing required keys.", "API Error");
                            return null;
                        }
                    }
                    else
                    {
                        DialogService.ShowSimpleDialog("Failed to get current stats.", "API Error");
                        return null;
                    }
                }
            }

            // If no data was returned, return null
            return null;
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Stats Error");
            string accountId = await GetAccountId();
            MessageBox.Show(accountId, ex.Message);
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return null; // Handle the exception by returning null
        }
    }

    public static async Task GetBEClientURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "BEClient"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        Definitions.BELauncherurl = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get BE url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task GetLauncherclientURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "LauncherClient"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        Definitions.Launcherurl = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get CLIENT url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task GetRedirectURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "Redirect"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        Definitions.RedirectUrl = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get Redirect url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task GetAnticheatZipURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "AnticheatZip"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        Definitions.AntiCheatUrl = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get EAC url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task GetSplashscreenURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "Splashscreen"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        Definitions.ExitSplashScreen = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get SPLASH url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task GetAnticheatConfigURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "AnticheatConfig"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        //Definitions.ExitSettingsJson = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get CONF url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task MemoryLeakFixerURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "MemoryleakFixer"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        //Definitions.ExitMemoryLeakFixer = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get MMLEAK url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

    public static async Task UpdaterURL()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "Updaterurl"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            // idek
                        }
                        //Definitions.Updaterurl = content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get Download links Error: Failed to get Updater url.", "API Error");
                    //Definitions.DisableLaunchBTN = true;
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Exit API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
        }
    }

}
