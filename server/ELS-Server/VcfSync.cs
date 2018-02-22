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
        public static List<Tuple<string,string,string>> VcfData = new List<Tuple<string,string,string>>();

        public VcfSync()
        {
            Debug.WriteLine("Loading VCF Sync");
        }

        public async Task CheckVCF(Player player)
        {
            foreach (string name in ElsResources)
            {
                Utils.DebugWriteLine($"Processing {name} for {player.Name}");
                LoadFilesPromScript(name);
            }
            Class1.TriggerClientEvent(player, "ELS:VcfSync:Client", VcfData);
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
                    Utils.DebugWriteLine($"Added {name} to resources");
                }
                
            }
            Utils.ReleaseWriteLine($"Total ELS Resources: {ElsResources.Count}");
        }

        internal static void LoadFilesPromScript(string name)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
#if DEBUG
                Debug.WriteLine($"{num} files for {name}");
#endif
                
                for (int i = 0; i < num; i++)
                {
                    var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "file", i);

                    var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                    Utils.DebugWriteLine($"Checking {filename}");
                    if (Path.GetExtension(filename).ToLower() == ".xml")
                    {

                        if (VCF.isValidData(data))
                        {
                            VcfData.Add(new Tuple<string, string, string>(name,filename,data));
                        }
                        else
                        {
#if DEBUG
                        Utils.DebugWriteLine($"XML data for {filename} is not valid");
#endif
                        }
                    }
            }
        }
    }
}
