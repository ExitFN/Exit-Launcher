using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteLauncher.Helpers
{
    internal class StorageSizesConverter
    {
        public static double BytesToGigabytes(long bytes)
        {
            return Math.Round((double)bytes / (1024 * 1024 * 1024), 2);
        }
        public static double BytesToMegabytes(long bytes)
        {
            return Math.Round((double)bytes / (1024 * 1024), 2);
        }
    }
}
