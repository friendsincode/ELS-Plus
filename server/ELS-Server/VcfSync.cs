using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS_Server
{
    public class VcfSync
    {

        public static List<string> ElsResources = new List<string>();
        public static List<Tuple<string, string, string>> VcfData = new List<Tuple<string, string, string>>();

        public VcfSync()
        {
            Utils.DebugWriteLine("Loading VCF Sync");
        }

        public async Task CheckVCF(Player player)
        {
            foreach (string name in ElsResources)
            {
                Utils.DebugWriteLine($"Processing {name} for {player.Name}");
                LoadFilesPromScript(name);
            }
        }

        public static async Task CheckResources()
        {
            var numResources = Function.Call<int>(Hash.GET_NUM_RESOURCES);
            for (int x = 0; x < numResources; x++)
            {
                var name = Function.Call<string>(Hash.GET_RESOURCE_BY_FIND_INDEX, x);
                string isElsResource = API.GetResourceMetadata(name, "is_els", 0);
                if (!String.IsNullOrEmpty(isElsResource) && isElsResource.Equals("true"))
                {
                    ElsResources.Add(name);
                    LoadFilesPromScript(name);
                    Utils.DebugWriteLine($"Added {name} to resources");
                }

            }
            Utils.ReleaseWriteLine($"Total ELS Resources: {ElsResources.Count}");
        }

        internal static async void LoadFilesPromScript(string name)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
            Utils.DebugWriteLine($"{num} files for {name}");

            for (int i = 0; i < num; i++)
            {
                //var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "file", i);
                DirectoryInfo info = new DirectoryInfo(API.GetResourcePath(name));
                foreach(DirectoryInfo d in info.GetDirectories())
                {
                    checkFiles(name, d.GetFiles());
                }
                checkFiles(name, info.GetFiles());
            }
            
        }

        internal static void checkFiles(string name, FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                Utils.DebugWriteLine($"Checking {f.FullName}");
                if (Path.GetExtension(f.Name).ToLower() == ".xml")
                {
                    string data = File.ReadAllText(f.FullName);
                    try
                    {
                        if (VCF.isValidData(data))
                        {
                            VcfData.Add(new Tuple<string, string, string>(name, f.Name, data));
                            Utils.DebugWriteLine($"Added {f.Name} to parsed list");
                        }
                        else
                        {
                            Utils.DebugWriteLine($"XML data for {f.Name} is not valid");
                        }
                    }
                    catch (XMLParsingException e)
                    {
                        Utils.ReleaseWriteLine($"There was a parsing error in {f.Name} please validate this XML and try again.");
                        Utils.ReleaseWriteLine($"{f.Name} has an error of {e.Message}");
                    }
                }
            }
        }
    }
}
