/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ELS.configuration
{
    internal delegate void ControlsUpdatedhandler(ElsConfiguration.ELSKBControls kbControls, ElsConfiguration.ELSGPControls gpControls);
    internal class ElsConfiguration
    {

        public static event ControlsUpdatedhandler ControlsUpdated;
        public static ELSKBControls KBBindings = new ELSKBControls();
        public static ELSGPControls GPBindings = new ELSGPControls();

        internal ElsConfiguration()
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
        }

        private void FileLoader_OnSettingsLoaded(SettingsType.Type type, string Data)
        {
            if (type == SettingsType.Type.GLOBAL)
            {
                var u = SharpConfig.Configuration.LoadFromString(Data);
                var t = u["KEYBOARD"]["Sound_Ahorn"].IntValue;
                KBBindings.Sound_Ahorn = (Control)t;

                t = u["KEYBOARD"]["Snd_SrnTon1"].IntValue;
                KBBindings.Snd_SrnTon1 = (Control)t;

                t = u["KEYBOARD"]["Snd_SrnTon2"].IntValue;
                KBBindings.Snd_SrnTon2 = (Control)t;

                t = u["KEYBOARD"]["Snd_SrnTon3"].IntValue;
                KBBindings.Snd_SrnTon3 = (Control)t;

                t = u["KEYBOARD"]["Snd_SrnTon4"].IntValue;
                KBBindings.Snd_SrnTon4 = (Control)t;

                t = u["KEYBOARD"]["Sound_Manul"].IntValue;
                KBBindings.Sound_Manul = (Control)t;

                t = u["KEYBOARD"]["Toggle_SIRN"].IntValue;
                KBBindings.Toggle_SIRN = (Control)t;

                t = u["KEYBOARD"]["Toggle_DSRN"].IntValue;
                KBBindings.Toggle_DSRN = (Control)t;

                t = u["KEYBOARD"]["TogInfoPanl"].IntValue;
                KBBindings.TogInfoPanl = (Control)t;

                t = u["KEYBOARD"]["Snd_SrnPnic"].IntValue;
                KBBindings.Snd_SrnPnic = (Control)t;

                t = u["KEYBOARD"]["Toggle_SECL"].IntValue;
                KBBindings.ToggleSecL = (Control)t;

                t = u["KEYBOARD"]["Toggle_WRNL"].IntValue;
                KBBindings.ToggleWrnL = (Control)t;

                t = u["KEYBOARD"]["Toggle_CRSL"].IntValue;
                KBBindings.ToggleCrsL = (Control)t;

                t = u["KEYBOARD"]["ChgPat_PRML"].IntValue;
                KBBindings.ChgPattPrmL = (Control)t;

                t = u["KEYBOARD"]["ChgPat_SECL"].IntValue;
                KBBindings.ChgPattSecL = (Control)t;

                t = u["KEYBOARD"]["ChgPat_WRNL"].IntValue;
                KBBindings.ChgPattWrnL = (Control)t;

                t = u["KEYBOARD"]["Toggle_LSTG"].IntValue;
                KBBindings.ToggleLstg = (Control)t;

                t = u["KEYBOARD"]["Toggle_TKDL"].IntValue;
                KBBindings.ToggleTdl = (Control)t;

                t = u["KEYBOARD"]["Toggle_BRD"].IntValue;
                KBBindings.ToggleBoard = (Control)t;

                t = u["KEYBOARD"]["Toggle_LIND"].IntValue;
                KBBindings.ToggleLIND = (Control)t;

                t = u["KEYBOARD"]["Toggle_RIND"].IntValue;
                KBBindings.ToggleRIND = (Control)t;

                t = u["KEYBOARD"]["Toggle_HAZ"].IntValue;
                KBBindings.ToggleHAZ = (Control)t;

                //Gamepad

                t = u["GAMEPAD"]["Toggle_TKDL"].IntValue;
                GPBindings.ToggleTdl = (Control)t;

                t = u["GAMEPAD"]["Sound_Ahorn"].IntValue;
                GPBindings.Sound_Ahorn = (Control)t;

                t = u["GAMEPAD"]["Snd_SrnTon1"].IntValue;
                GPBindings.Snd_SrnTon1 = (Control)t;

                t = u["GAMEPAD"]["Snd_SrnTon3"].IntValue;
                GPBindings.Snd_SrnTon3 = (Control)t;

                t = u["GAMEPAD"]["Toggle_LSTG"].IntValue;
                GPBindings.ToggleLstg = (Control)t;

                t = u["GAMEPAD"]["Toggle_SIRN"].IntValue;
                GPBindings.Toggle_SIRN = (Control)t;


                //ControlsUpdated?.Invoke(KeyBindings);
                Global.EnabeTrafficControl = u["GENERAL"]["ElsTrafCtrlOn"].BoolValue;
                Global.DisableSirenOnExit = u["GENERAL"]["ElsSirenOffonExit"].BoolValue;
                Global.ResetTakedownSpotlight = u["GENERAL"]["ResetTakeDownSpotlight"].BoolValue;
                Global.PrimDelay = u["LIGHTING"]["LightFlashDelayMainLts"].IntValue;
                Global.DeleteInterval = u["Admin"]["VehicleDeleteInterval"].FloatValue * 60 * 1000;
                Global.EnvLightRng = u["LIGHTING"]["EnvLtMultExtraLts_Rng"].FloatValue * 25f;
                Global.EnvLightInt = u["LIGHTING"]["EnvLtMultExtraLts_Int"].FloatValue * .02f;
                Global.TkdnRng = u["LIGHTING"]["EnvLtMultTakedwns_Rng"].FloatValue * 25f;
                Global.TkdnInt = u["LIGHTING"]["EnvLtMultTakedwns_Int"].FloatValue * 1f;
                Global.DayLtBrightness = u["LIGHTING"]["DayLtBrightness"].FloatValue * 100f;
                Global.NightLtBrightness = u["LIGHTING"]["NightLtBrightness"].FloatValue * 100f;
                Global.SoundVolume = u["AUDIO"]["BtnClicksVolume"].FloatValue / 100;
                Global.BtnClicksBtwnSrnTones = u["AUDIO"]["BtnClicksBtwnSrnTones"].BoolValue;
                Global.BtnClicksBtwnHrnTones = u["AUDIO"]["BtnClicksBtwnHrnTones"].BoolValue;
                Global.BtnClicksIndicators = u["AUDIO"]["BtnClicksIndicators"].BoolValue;
                Global.EnableDLCSound = u["AUDIO"]["EnableDLCSound"].BoolValue;
                Global.DLCSoundBank = u["AUDIO"]["DLCSoundBank"].StringValue;
                Global.DLCSoundSet = u["AUDIO"]["DLCSoundSet"].StringValue;
                Global.AllowController = u["GAMEPAD"]["AllowController"].BoolValue;
                Utils.DebugWrite($"Configuration ran \n ---------------------- \n Traffic Control: {Global.EnabeTrafficControl} \n Delay: {Global.PrimDelay} \n Delete Interval: {Global.DeleteInterval} \n Env Lighting Range: {Global.EnvLightRng}\n");
            }
        }
        internal class ELSKBControls
        {
            internal Control ToggleTdl { get; set; }
            internal Control Toggle_SIRN { get; set; }
            internal Control Sound_Ahorn { get; set; }
            internal Control Snd_SrnTon1 { get; set; }
            internal Control Snd_SrnTon2 { get; set; }
            internal Control Snd_SrnTon3 { get; set; }
            internal Control Snd_SrnTon4 { get; set; }
            internal Control Snd_SrnPnic { get; set; }
            internal Control Sound_Manul { get; set; }
            internal Control Toggle_DSRN { get; set; }
            internal Control TogInfoPanl { get; set; }
            internal Control ToggleBoard { get; set; }
            internal Control ToggleSecL { get; set; }
            internal Control ToggleWrnL { get; set; }
            internal Control ToggleCrsL { get; set; }
            internal Control ChgPattPrmL { get; set; }
            internal Control ChgPattSecL { get; set; }
            internal Control ChgPattWrnL { get; set; }
            internal Control ToggleLstg { get; set; }
            internal Control ToggleLIND { get; set; }
            internal Control ToggleRIND { get; set; }
            internal Control ToggleHAZ { get; set; }
        }

        internal class ELSGPControls
        {
            internal Control ToggleTdl { get; set; }
            internal Control Toggle_SIRN { get; set; }
            internal Control Sound_Ahorn { get; set; }
            internal Control Snd_SrnTon1 { get; set; }
            internal Control Snd_SrnTon3 { get; set; }
            internal Control ToggleLstg { get; set; }
        }

        internal static bool isValidData(string data)
        {
            return SharpConfig.Configuration.LoadFromString(data).Contains("KEYBOARD", "Toggle_WRNL");
        }

        
    }
}