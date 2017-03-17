using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.configuration
{
    delegate void ControlsUpdatedhandler(configuration.ControlConfiguration.ELSControls elsControls);
    class ControlConfiguration
    {

        public static event ControlsUpdatedhandler ControlsUpdated;
        public static ELSControls KeyBindings = new ELSControls();
        public ControlConfiguration()
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;   
        }

        private void FileLoader_OnSettingsLoaded(SettingsType.Type type, string Data)
        {
            if (type == SettingsType.Type.GLOBAL)
            {
                var u = SharpConfig.Configuration.LoadFromString(Data);
                var t = u["CONTROL"]["Sound_Ahorn"].IntValue;
                KeyBindings.Sound_Ahorn = (Control)t;
                ControlsUpdated?.Invoke(KeyBindings);
            }
        }
        public class ELSControls
        {
            public Control Toggle_SIRN { get; internal set; }
            public Control Sound_Ahorn { get; internal set; }
        }
    }
}