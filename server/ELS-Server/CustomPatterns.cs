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
    public class CustomPatterns
    {
        static List<Tuple<string, string, string>> Patterns = new List<Tuple<string, string, string>>();
        internal static bool IsValidPatternData(string data)
        {
            if (String.IsNullOrEmpty(data)) return false;
            NanoXMLDocument doc = new NanoXMLDocument(data);
            //TODO change how below is detected to account for xml meta tag being before it.
            return doc.RootNode.Name == "pattern";
        }

        public async static Task CheckCustomPatterns(Player player)
        {
            var numResources = API.GetNumResources();
            for (int x = 0; x < numResources; x++)
            {
                var name = Function.Call<string>(Hash.GET_RESOURCE_BY_FIND_INDEX, x);
                ParsePatterns(name, player);
            }
            Class1.TriggerClientEvent(player, "ELS:PatternSync:Client", Patterns);
        }

        internal static void ParsePatterns(string name, Player player)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
            string isElsResource = API.GetResourceMetadata(name, "is_els", 0);
            if (!String.IsNullOrEmpty(isElsResource) && isElsResource.Equals("true"))
            {
                for (int i = 0; i < num; i++)
                {
                    var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "file", i);
                    var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                    Utils.DebugWriteLine($"Checking {filename}");
                    if (Path.GetExtension(filename).ToLower() == ".xml")
                    {

                        if (CustomPatterns.IsValidPatternData(data))
                        {
                            Patterns.Add(new Tuple<string, string, string>(name, filename, data));
                        }
                        else
                        {
                            Utils.DebugWriteLine($"XML Pattern data for {filename} is not valid");
                        }
                    }
                }
            }
            else
            {
                Utils.DebugWriteLine($"{name} is not an ELS Vehicle Resource");
            }
        }
    }
}
