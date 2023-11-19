using System;
using System.Runtime.InteropServices;

namespace FortniteLauncher.CheckSuspendedProcess
{ 
    public static class NativeMethodsProc
    {
        [DllImport("ntdll.dll")]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, out int processInformation, int processInformationLength, IntPtr returnLength);
    }
}
