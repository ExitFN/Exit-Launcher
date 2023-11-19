using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteLauncher
{
    public class Config
    {
        public bool IsSoundEnabled;

        public string FortnitePath;

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        ///defaults
        ///Dark Theme
        ///Sound Disabled
        ///EOR Enabled
        //////Close On launch Disabled
        ///
        ///Username, email and password are empty and will be set at runtime
    }
}
