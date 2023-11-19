using System;
using System.IO;

namespace FortniteLauncher.Helpers
{
    public class PathHelper
    {
        public static bool IsPathValid(string path)
        {
            string enginePath = Path.Combine(path, "Engine");
            string fortniteGamePath = Path.Combine(path, "FortniteGame");

            bool engineDirectoryExists = Directory.Exists(enginePath);
            bool fortniteGameDirectoryExists = Directory.Exists(fortniteGamePath);

            if (!engineDirectoryExists || !fortniteGameDirectoryExists)
            {
                return false;
            }

            string parentDirectory = Path.GetDirectoryName(path);
            bool parentDirectoryExists = !string.IsNullOrEmpty(parentDirectory) && Directory.Exists(parentDirectory);

            return parentDirectoryExists;
        }
    }
}
