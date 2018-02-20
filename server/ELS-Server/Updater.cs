using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS_Server
{
    internal class Updater
    {

        internal static string GetCurrentVersion()
        {
            var resourcedir = System.AppDomain.CurrentDomain.BaseDirectory;
            resourcedir = System.IO.Path.Combine(resourcedir, "resources", CitizenFX.Core.Native.API.GetCurrentResourceName());
            Utils.DebugWriteLine(resourcedir);
            if (!System.IO.Directory.Exists(resourcedir))
            {
                Utils.ReleaseWrite($"Error: {resourcedir} does not exist.");
                return "";
            }

            var t = System.IO.Directory.EnumerateFileSystemEntries(resourcedir);
            Dictionary<string, string> files = new Dictionary<string, string> { { "els-plus", null }, { "els-server", null } };

            foreach (var file in t)
            {
                if (System.IO.Path.GetFileName(file).EndsWith(".net.dll") && files.ContainsKey(System.IO.Path.GetFileName(file.TrimEnd(".net.dll".ToCharArray()))))
                {
                    files[System.IO.Path.GetFileName(file).TrimEnd(".net.dll".ToCharArray())] = System.Diagnostics.FileVersionInfo.GetVersionInfo(file).ProductVersion;
                    Utils.DebugWriteLine($"{files.ToString()}");
                }
            }
            return "";
        }
    }
}
