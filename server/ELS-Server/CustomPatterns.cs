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
            Patterns.Clear();
            foreach (string name in VcfSync.ElsResources)
            {
                ParsePatterns(name);
            }

        }

        internal static void ParsePatterns(string name)
        {
            Patterns.RemoveAll(entry => entry.Item1 == name);
            string path = API.GetResourcePath(name);
            if (String.IsNullOrEmpty(path) || !Directory.Exists(path)) return;
            try
            {
                checkFiles(name, new DirectoryInfo(path).GetFiles("*.xml", SearchOption.AllDirectories));
            }
            catch (IOException ex) { Utils.ReleaseWriteLine($"Cannot read patterns in {name}: {ex.Message}"); }
            catch (UnauthorizedAccessException ex) { Utils.ReleaseWriteLine($"Cannot read patterns in {name}: {ex.Message}"); }
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
