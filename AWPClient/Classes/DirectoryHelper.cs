using System;
using System.IO;

namespace LEAD_UPD_CREATOR.Helpers
{
    public class DirectoryHelper
    {
        public static string GetResDirectory()
        {
            string resdir = string.Empty;

            if (OperatingSystem.IsLinux())
            {
                int pos = AppContext.BaseDirectory.LastIndexOf("/");
                string dir = AppContext.BaseDirectory.Substring(0, pos);
                string result = Path.GetDirectoryName(dir);
                resdir = result + "//";
            }
            else if (OperatingSystem.IsWindows())
            {
                int pos = AppContext.BaseDirectory.LastIndexOf("\\");
                string dir = AppContext.BaseDirectory.Remove(pos, AppContext.BaseDirectory.Length - pos);
                string result = Path.GetDirectoryName(dir);
                resdir = result + "\\";
            }

            return resdir;
        }
    }
}
