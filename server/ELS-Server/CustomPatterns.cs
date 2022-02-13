using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ELS_Server
{
    public class CustomPatterns
    {
        internal static List<Tuple<string, string, string>> Patterns = new List<Tuple<string, string, string>>();
        internal static bool IsValidPatternData(string data)
        {
            if (String.IsNullOrEmpty(data)) return false;
            NanoXMLDocument doc = new NanoXMLDocument(data);

            //TODO change how below is detected to account for xml meta tag being before it.
            return doc.RootNode.Name == "pattern";
        }

        public async static Task CheckCustomPatterns()
        {
            var numResources = API.GetNumResources();
            foreach (string name in VcfSync.ElsResources)
            {
                ParsePatterns(name);
            }

        }

        internal static void ParsePatterns(string name)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
            string isElsResource = API.GetResourceMetadata(name, "is_els", 0);
            for (int i = 0; i < num; i++)
            {
                DirectoryInfo info = new DirectoryInfo(API.GetResourcePath(name));
                foreach (DirectoryInfo d in info.GetDirectories())
                {
                    checkFiles(name, d.GetFiles());
                }
                checkFiles(name, info.GetFiles());
                //var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "file", i);
                //var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                //Utils.DebugWriteLine($"Checking {filename}");
                //if (Path.GetExtension(filename).ToLower() == ".xml")
                //{
                //    try
                //    {
                //        if (CustomPatterns.IsValidPatternData(data))
                //        {
                //            Patterns.Add(new Tuple<string, string, string>(name, filename, data));
                //        }
                //        else
                //        {
                //            Utils.DebugWriteLine($"XML Pattern data for {filename} is not valid");
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        Utils.DebugWriteLine($"There was a parsing error in {filename} please validate this XML and try again.");
                //    }
                //}
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
                        if (CustomPatterns.IsValidPatternData(data))
                        {
                            Patterns.Add(new Tuple<string, string, string>(name, f.Name, data));
                            Utils.DebugWriteLine($"Added {f.Name} to parsed patterns list");
                        }
                        else
                        {
                            Utils.DebugWriteLine($"XML Pattern data for {f.Name} is not valid");
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
