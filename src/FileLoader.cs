using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS
{
    class FileLoader
    {
        ELS _baseScript;
        public FileLoader(ELS h)
        {
            _baseScript = h;
        }
        public void RunLoadeer(string name)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "ELSFM");
            Debug.WriteLine(num.ToString() + " " + name);
            for (int i = 0; i < num; i++)
            {
                var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "ELSFM", i);
                Debug.WriteLine(filename);
                if (filename.Equals("Resources\\ELS.ini"))
                {
                    Debug.WriteLine($"Debug: name:{name} filename:{filename}");
                    var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                    Debug.WriteLine(filename);
                    Debug.WriteLine(data);
                    var parser = new IniParser.Parser.IniDataParser();
                    var j = parser.Parse(data);
                }
            }
        }
    }
}
