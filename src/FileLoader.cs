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
            Debug.WriteLine(num.ToString() + " " + name);
            for (int i = 0; i < num; i++)
            {
                var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "ELSFM", i);
                var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);
                if (filename.Equals("extra-files/ELS.ini"))
                {
                    OnSettingsLoaded?.Invoke(configuration.SettingsType.Type.GLOBAL, data);
                }
                if (filename.Equals("extra-files/ELS.xml", StringComparison.CurrentCultureIgnoreCase))
                {
                    OnSettingsLoaded?.Invoke(configuration.SettingsType.Type.LIGHTING,data);
                }
            }
        }
    }
}
