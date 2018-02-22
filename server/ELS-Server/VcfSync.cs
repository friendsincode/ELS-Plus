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
            var numResources = Function.Call<int>(Hash.GET_NUM_RESOURCES);
            for (int x = 0; x < numResources; x++)
            {
                var name = Function.Call<string>(Hash.GET_RESOURCE_BY_FIND_INDEX, x);
                LoadFilesPromScript(name, player);
            }
            Class1.TriggerClientEvent(player, "ELS:VcfSync:Client", VcfData);
        }

        internal static void LoadFilesPromScript(string name, Player player)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
            string isElsResource = API.GetResourceMetadata(name, "is_els", 0);
            if (!String.IsNullOrEmpty(isElsResource) && isElsResource.Equals("true"))
            {

#if DEBUG
                Debug.WriteLine($"{num} files for {name}");
#endif
                ElsResources.Add(name);
                for (int i = 0; i < num; i++)
                {
                    var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "file", i);

                    var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);

#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Checking {filename}");
#endif
                    if (Path.GetExtension(filename).ToLower() == ".xml")
                    {

                        if (VCF.isValidData(data))
                        {
                            VcfData.Add(new Tuple<string, string, string>(name,filename,data));
                        }
                        else
                        {
#if DEBUG
                            CitizenFX.Core.Debug.WriteLine($"XML data for {filename} is not valid");
#endif
                        }
                    }
                }
            }
            else
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"{name} is not an ELS Vehicle Resource");
#endif
            }


        }


    }
}
