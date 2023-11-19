using FortniteLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking;

namespace FortniteLauncher.Core.Mods
{
    internal class WhitelistedProcesses
    {
        public static List<string> BlockedApps = new List<string>
                 {
                    "ida64", "IDAPortable", "OllyDbg", "x64dbg", "Cheat Engine",
                    "Ghidra", "Binary Ninja", "Radare2", "Hopper", "IDA Free", "PEiD", "Exeinfo PE",
                    "Procmon", "Wireshark", "Fiddler", "Charles Proxy", "Burp Suite", "ZAP Proxy",
                    "Wireshark", "Telerik Fiddler", "WiX Toolset", "NSIS", "Inno Setup", "Nullsoft Scriptable Install System",
                    "Dependency Walker", "PE Explorer", "Resource Hacker", "dnSpy", "ILSpy", "Reflector",
                    "Eclipse", "PyCharm", "Vim", "WinHex", "Hex Fiend", "Hex Workshop", "IDA Free", "BinaryDiff", "OllyICE",
                    "GameGuardian", "Memory Editors", "Debuggers", "Disassemblers", "Packet Sniffers", "Proxy Tools", "UuuClient", "ProcessHacker", "Wemod"
                    
                };
        public static async Task ProcessCheckLoops()
        {
            bool done = false;
            while (!done)
            {
                foreach (var process in Process.GetProcesses())
                {
                    string processName = process.ProcessName.ToLower();
                    if (BlockedApps.Contains(processName))
                    {
                        Fortnite.Error();
                        DialogService.ShowSimpleDialog("A whitelisted process is running please close: " + processName + " before playing Exit.", "Error");
                        break;
                    }
                }
                await Task.Delay(1000);
            }
        }
    }
}
