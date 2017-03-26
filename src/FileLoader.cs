using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELS.configuration;

namespace ELS
{
    class FileLoader
    {
        ELS _baseScript;
        public delegate void SettingsLoadedHandler(configuration.SettingsType.Type type, String Data);
        public static event SettingsLoadedHandler OnSettingsLoaded;
        public FileLoader(ELS h)
        {
            _baseScript = h;
        }
        public void RunLoadeer(string name)
        {
            if (ELS.isStopped) return;
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "ELSFM");
#if DEBUG
            Debug.WriteLine("number of files to load: " + num.ToString() + " " + name);
#endif
            for (int i = 0; i < num; i++)
            {
                var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "ELSFM", i);
#if DEBUG
                Debug.WriteLine($"Name: {name} Loading: {filename}");
#endif

                if (filename.Equals("extra-files/ELS.ini"))
                {
                    var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                    OnSettingsLoaded?.Invoke(configuration.SettingsType.Type.GLOBAL, data);
                }
                // var ldata = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                //Debug.WriteLine(ldata);
            }

            num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "ELSFMVCF");
            for (int i = 0; i < num; i++)
            {
                var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "ELSFMVCF", i);
#if DEBUG
                Debug.WriteLine($"Name: {name} Loading: {filename}");
#endif


                var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                VCF.load(SettingsType.Type.VCF, filename, data);
                //Debug.WriteLine(ldata);
            }
        }
    }
}
