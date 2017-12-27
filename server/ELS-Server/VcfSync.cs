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


        public VcfSync()
        {
            Debug.WriteLine("Load VCF Sync Running");
        }

        public void CheckVCF()
        {
            var numResources = Function.Call<int>(Hash.GET_NUM_RESOURCES);
            for (int x = 0; x < numResources; x++)
            {
                var name = Function.Call<string>(Hash.GET_RESOURCE_BY_FIND_INDEX, x);
                LoadFilesPromScript(name);
            }
        }

        private static void LoadFilesPromScript(string name)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
            CitizenFX.Core.Debug.WriteLine($"{num} files for {name}");
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
#if DEBUG
                        CitizenFX.Core.Debug.WriteLine("Sending data to XML parser");
                        CitizenFX.Core.Debug.WriteLine($"VCF.load({filename}, data,{name})");
#endif
                        VCF.load(filename, data, name);
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


    }
}
