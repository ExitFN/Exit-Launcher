using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using FortniteLauncher.Interop;
using FortniteLauncher.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.DialProtocol;
using Windows.Storage;

namespace FortniteLauncher.Core
{
    public class Settings
    {
        private static string RootDirectory = Definitions.RootDirectory;
        private static string SaveFile = Path.Combine(RootDirectory, "settings.json");

        public static void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(Globals.m_config);

            if (!Directory.Exists(RootDirectory))
            {
                Directory.CreateDirectory(RootDirectory);
            }

            using (var fileStream = new FileStream(SaveFile, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(json);
                }
            }
        }

        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(SaveFile))
                {
                    using (var fileStream = new FileStream(SaveFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            string json = reader.ReadToEnd();
                            Config config = JsonConvert.DeserializeObject<Config>(json);
                            Globals.m_config = config;
                        }
                    }
                }
                else
                {
                    // File not found, set default values
                    Globals.m_config = new Config
                    {
                        IsSoundEnabled = false,
                    };

                    SaveSettings();
                }
            }
            catch (Exception ex)
            {
                //we cannot show a dialog, it might cause a crash as the main window is not loaded yet
                MessageBox.Show("An Error occured while loading settings: " + ex.Message, "Error");
            }
        }
    }
}
