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

                t = u["CONTROL"]["Snd_SrnTon1"].IntValue;
                KeyBindings.Snd_SrnTon1 = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon2"].IntValue;
                KeyBindings.Snd_SrnTon2 = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon3"].IntValue;
                KeyBindings.Snd_SrnTon3 = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon4"].IntValue;
                KeyBindings.Snd_SrnTon4 = (Control)t;

                t = u["CONTROL"]["Sound_Manul"].IntValue;
                KeyBindings.Sound_Manul = (Control)t;

                t = u["CONTROL"]["Toggle_SIRN"].IntValue;
                KeyBindings.Toggle_SIRN = (Control)t;

                t = u["CONTROL"]["Toggle_DSRN"].IntValue;
                KeyBindings.Toggle_DSRN = (Control)t;

                t = u["CONTROL"]["TogInfoPanl"].IntValue;
                KeyBindings.TogInfoPanl = (Control)t;

                ControlsUpdated?.Invoke(KeyBindings);

            }
        }
        public class ELSControls
        {
            public Control Toggle_SIRN { get; internal set; }
            public Control Sound_Ahorn { get; internal set; }
            public Control Snd_SrnTon1 { get; internal set; }
            public Control Snd_SrnTon2 { get; internal set; }
            public Control Snd_SrnTon3 { get; internal set; }
            public Control Snd_SrnTon4 { get; internal set; }
            public Control Sound_Manul { get; internal set; }
            public Control Toggle_DSRN { get; internal set; }
            public Control TogInfoPanl { get; internal set; }
        }
    }
}