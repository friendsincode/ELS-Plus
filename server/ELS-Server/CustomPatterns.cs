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
        internal static bool IsValidPatternData(string data)
        {
            if (String.IsNullOrEmpty(data)) return false;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(data);
            //TODO change how below is detected to account for xml meta tag being before it.
            return doc.DocumentElement.Name == "pattern";
        }

        public void CheckVCF(Player player)
        {
            var numResources = API.GetNumResources();
            for (int x = 0; x < numResources; x++)
            {
                var name = Function.Call<string>(Hash.GET_RESOURCE_BY_FIND_INDEX, x);
                ParsePatterns(name, player);
            }
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

#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Checking {filename}");
#endif
                    if (Path.GetExtension(filename).ToLower() == ".xml")
                    {

                        if (CustomPatterns.IsValidPatternData(data))
                        {
                            Class1.TriggerClientEvent(player, "ELS:PatternSync:Client", name, filename, data);
                        }
                        else
                        {
#if DEBUG
                            CitizenFX.Core.Debug.WriteLine($"XML Pattern data for {filename} is not valid");
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
