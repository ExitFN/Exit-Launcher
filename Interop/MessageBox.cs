using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FortniteLauncher.Interop
{
    internal class MessageBox
    {
        public enum Options
        {
            OK
        }
        public static void Show(string Content, string Title = null, Options Options = Options.OK)
        {
            [DllImport("user32.dll")]
            static extern int MessageBox(IntPtr hWind, String text, String caption, int options);
            MessageBox(IntPtr.Zero, Content, Title, Convert.ToInt32(Options));
        }
    }
}
