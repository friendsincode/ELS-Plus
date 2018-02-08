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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CitizenFX.Core;

namespace ELS.configuration
{
    internal struct VCFEntry
    {
        public string filename;
        public string resource;
        public Vcfroot root;
        public Model modelHash;

        public VCFEntry(string fn, string res, Model hash, Vcfroot vcfroot)
        {
            filename = fn;
            resource = res;
            root = vcfroot;
            modelHash = hash;
        }
    }

    public class VCF
    {
        internal static List<VCFEntry> ELSVehicle = new List<VCFEntry>();
        public VCF()
        {
        }
        internal static void load(SettingsType.Type type, string name, string Data, string ResourceName)
        {
            var bytes = Encoding.UTF8.GetBytes(Data);
            if (bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                var ex = new Exception($"Error Loading:{name}\n" +
                                    $"Please save {name} with UTF-8 no BOM/Signature Encoding");
                throw (ex);
            }
            Encoding.UTF8.GetPreamble();
            Model hash = Game.GenerateHash(Path.GetFileNameWithoutExtension(name));
            var data = new VCFEntry(Path.GetFileNameWithoutExtension(name), ResourceName, hash, new Vcfroot());
            if (type == SettingsType.Type.VCF)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(Data);
                bool res;
                //data.filename = Path.GetFileNameWithoutExtension(name);
                if (data.root == null)
                {
                    CitizenFX.Core.Debug.WriteLine("Null issue");
                    return;
                }
                #region VCF Info
                CitizenFX.Core.Debug.WriteLine($"Parsing VCF Info for vehicle {name}");
                //VCF Description
                if (doc["vcfroot"].Attributes["Description"] != null)
                {
                    data.root.Description = doc["vcfroot"].Attributes["Description"].Value;
                }

                //VCF Author
                if (doc["vcfroot"].Attributes["Author"] != null)
                {
                    data.root.Author = doc["vcfroot"].Attributes["Author"].Value;
                }
                #endregion
                #region Interface
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Interface");
#endif
                try
                {
                    data.root.INTERFACE.LstgActivationType = doc["vcfroot"]["INTERFACE"]["LstgActivationType"].InnerText;
                    data.root.INTERFACE.DefaultSirenMode = doc["vcfroot"]["INTERFACE"]["DefaultSirenMode"].InnerText;
                    data.root.INTERFACE.InfoPanelHeaderColor = doc["vcfroot"]["INTERFACE"]["InfoPanelHeaderColor"].InnerText;
                    data.root.INTERFACE.InfoPanelButtonLightColor = doc["vcfroot"]["INTERFACE"]["InfoPanelButtonLightColor"].InnerText;
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Extras Override
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Extras");
#endif
                try
                {
                    //Extra 01
                    data.root.EOVERRIDE.Extra01.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra01.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra01.Color = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra01.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra01.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra01.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["OffsetZ"].Value);
                    //Extra 02
                    data.root.EOVERRIDE.Extra02.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra02.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra02.Color = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra02.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra02.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra02.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["OffsetZ"].Value);
                    //Extra 03
                    data.root.EOVERRIDE.Extra03.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra03.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra03.Color = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra03.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra03.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra03.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["OffsetZ"].Value);
                    //Extra 04
                    data.root.EOVERRIDE.Extra04.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra04.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra04.Color = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra04.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra04.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra04.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["OffsetZ"].Value);
                    //Extra 05
                    data.root.EOVERRIDE.Extra05.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra05.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra05.Color = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra05.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra05.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra05.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["OffsetZ"].Value);
                    //Extra 06
                    data.root.EOVERRIDE.Extra06.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra06.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra06.Color = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra06.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra06.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra06.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["OffsetZ"].Value);
                    //Extra 07
                    data.root.EOVERRIDE.Extra07.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra07.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra07.Color = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra07.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra07.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra07.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["OffsetZ"].Value);
                    //Extra 08
                    data.root.EOVERRIDE.Extra08.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra08.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra08.Color = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra08.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra08.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra08.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["OffsetZ"].Value);
                    //Extra 09
                    data.root.EOVERRIDE.Extra09.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["IsElsControlled"].Value);
                    data.root.EOVERRIDE.Extra09.AllowEnvLight = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["AllowEnvLight"].Value);
                    data.root.EOVERRIDE.Extra09.Color = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["Color"].Value;
                    data.root.EOVERRIDE.Extra09.OffsetX = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["OffsetX"].Value);
                    data.root.EOVERRIDE.Extra09.OffsetY = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["OffsetY"].Value);
                    data.root.EOVERRIDE.Extra09.OffsetZ = float.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["OffsetZ"].Value);
                    //Extra 10
                    data.root.EOVERRIDE.Extra10.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra10"].Attributes["IsElsControlled"].Value);
                    //Extra 11
                    data.root.EOVERRIDE.Extra11.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra11"].Attributes["IsElsControlled"].Value);
                    //Extra 12
                    data.root.EOVERRIDE.Extra12.IsElsControlled = bool.Parse(doc["vcfroot"]["EOVERRIDE"]["Extra12"].Attributes["IsElsControlled"].Value);
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"EOveride for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region MISC
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Misc");
#endif
                try
                {
                    data.root.MISC.VehicleIsSlicktop = bool.Parse(doc["vcfroot"]["MISC"]["VehicleIsSlicktop"].InnerText);
                    data.root.MISC.ArrowboardType = doc["vcfroot"]["MISC"]["ArrowboardType"].InnerText;
                    data.root.MISC.UseSteadyBurnLights = bool.Parse(doc["vcfroot"]["MISC"]["UseSteadyBurnLights"].InnerText);
                    data.root.MISC.DfltSirenLtsActivateAtLstg = int.Parse(doc["vcfroot"]["MISC"]["DfltSirenLtsActivateAtLstg"].InnerText);
                    data.root.MISC.Takedowns.AllowUse = bool.Parse(doc["vcfroot"]["MISC"]["Takedowns"].Attributes["AllowUse"].Value);
                    data.root.MISC.Takedowns.Mirrored = bool.Parse(doc["vcfroot"]["MISC"]["Takedowns"].Attributes["Mirrored"].Value);
                    data.root.MISC.SceneLights.AllowUse = bool.Parse(doc["vcfroot"]["MISC"]["SceneLights"].Attributes["AllowUse"].Value);
                    data.root.MISC.SceneLights.IlluminateSidesOnly = bool.Parse(doc["vcfroot"]["MISC"]["SceneLights"].Attributes["IlluminateSidesOnly"].Value);
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Misc for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Cruise
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Cruise");
#endif
                try
                {
                    data.root.CRUISE.DisableAtLstg3 = bool.Parse(doc["vcfroot"]["CRUISE"]["DisableAtLstg3"].InnerText);
                    data.root.CRUISE.UseExtras.Extra1 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra1"].Value);
                    data.root.CRUISE.UseExtras.Extra2 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra2"].Value);
                    data.root.CRUISE.UseExtras.Extra3 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra3"].Value);
                    data.root.CRUISE.UseExtras.Extra4 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra4"].Value);
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Cruise for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Aux Coronas
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Aux Coronas");
#endif
                try
                {
                    //Headlights
                    data.root.ACORONAS.Headlights.DfltPattern = int.Parse(doc["vcfroot"]["ACORONAS"]["Headlights"].Attributes["DfltPattern"].Value);
                    data.root.ACORONAS.Headlights.ColorL = doc["vcfroot"]["ACORONAS"]["Headlights"].Attributes["ColorL"].Value;
                    data.root.ACORONAS.Headlights.ColorR = doc["vcfroot"]["ACORONAS"]["Headlights"].Attributes["ColorR"].Value;
                    //TailLights
                    data.root.ACORONAS.TailLights.DfltPattern = int.Parse(doc["vcfroot"]["ACORONAS"]["TailLights"].Attributes["DfltPattern"].Value);
                    data.root.ACORONAS.TailLights.ColorL = doc["vcfroot"]["ACORONAS"]["TailLights"].Attributes["ColorL"].Value;
                    data.root.ACORONAS.TailLights.ColorR = doc["vcfroot"]["ACORONAS"]["TailLights"].Attributes["ColorR"].Value;
                    //IndicatorsF
                    data.root.ACORONAS.IndicatorsF.DfltPattern = int.Parse(doc["vcfroot"]["ACORONAS"]["IndicatorsF"].Attributes["DfltPattern"].Value);
                    data.root.ACORONAS.IndicatorsF.ColorL = doc["vcfroot"]["ACORONAS"]["IndicatorsF"].Attributes["ColorL"].Value;
                    data.root.ACORONAS.IndicatorsF.ColorR = doc["vcfroot"]["ACORONAS"]["IndicatorsF"].Attributes["ColorR"].Value;
                    //IndicatorsB
                    data.root.ACORONAS.IndicatorsB.DfltPattern = int.Parse(doc["vcfroot"]["ACORONAS"]["IndicatorsB"].Attributes["DfltPattern"].Value);
                    data.root.ACORONAS.IndicatorsB.ColorL = doc["vcfroot"]["ACORONAS"]["IndicatorsB"].Attributes["ColorL"].Value;
                    data.root.ACORONAS.IndicatorsB.ColorR = doc["vcfroot"]["ACORONAS"]["IndicatorsB"].Attributes["ColorR"].Value;
                    //ReverseLights
                    data.root.ACORONAS.ReverseLights.DfltPattern = int.Parse(doc["vcfroot"]["ACORONAS"]["ReverseLights"].Attributes["DfltPattern"].Value);
                    data.root.ACORONAS.ReverseLights.ColorL = doc["vcfroot"]["ACORONAS"]["ReverseLights"].Attributes["ColorL"].Value;
                    data.root.ACORONAS.ReverseLights.ColorR = doc["vcfroot"]["ACORONAS"]["ReverseLights"].Attributes["ColorR"].Value;
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"ACoronas for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Sounds
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Sounds");
#endif
                try
                {
                    //Manual Tone 1
                    data.root.SOUNDS.ManTone1.AudioString = doc?["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.ManTone1.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AllowUse"].Value);
                    //Manual Tone 2
                    data.root.SOUNDS.ManTone2.AudioString = doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.ManTone2.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AllowUse"].Value);
                    //Main Horn
                    data.root.SOUNDS.MainHorn.AudioString = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.MainHorn.InterruptsSiren = bool.Parse(doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["InterruptsSiren"].Value);
                    //Siren Tone 1
                    data.root.SOUNDS.SrnTone1.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.SrnTone1.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AllowUse"].Value);
                    //Siren Tone 2
                    data.root.SOUNDS.SrnTone2.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.SrnTone2.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AllowUse"].Value);
                    //Siren Tone 3
                    data.root.SOUNDS.SrnTone3.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.SrnTone3.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AllowUse"].Value);
                    //Siren Tone 4
                    data.root.SOUNDS.SrnTone4.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AudioString"].Value;
                    data.root.SOUNDS.SrnTone4.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AllowUse"].Value);
                    //Aux Siren Tone
                    data.root.SOUNDS.AuxSiren.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AllowUse"].Value);
                    data.root.SOUNDS.AuxSiren.AudioString = doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AudioString"].Value;
                    //Panic Mode Tone
                    data.root.SOUNDS.PanicMde.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AllowUse"].Value);
                    data.root.SOUNDS.PanicMde.AudioString = doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AudioString"].Value;
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Sounds for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Warning Lights
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Warning Lights");
#endif
                try
                {
                    data.root.WRNL.LightingFormat = doc["vcfroot"]["WRNL"].Attributes["LightingFormat"].Value;
                    if (doc["vcfroot"]["WRNL"].Attributes["DisableAtLstg3"] != null)
                    {
                        data.root.WRNL.DisableAtLstg3 = bool.Parse(doc["vcfroot"]["WRNL"].Attributes["DisableAtLstg3"]?.Value);
                    }
                    data.root.WRNL.ExtrasActiveAtLstg1 = doc["vcfroot"]["WRNL"].Attributes["ExtrasActiveAtLstg1"]?.Value;
                    data.root.WRNL.ExtrasActiveAtLstg2 = doc["vcfroot"]["WRNL"].Attributes["ExtrasActiveAtLstg2"]?.Value;
                    data.root.WRNL.ExtrasActiveAtLstg3 = doc["vcfroot"]["WRNL"].Attributes["ExtrasActiveAtLstg3"]?.Value;
                    //Preset Patterns
                    bool preset = false;
                    if (doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.WRNL.PresetPatterns.Lstg1.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value);
                    }
                    data.root.WRNL.PresetPatterns.Lstg1.Pattern = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg1"]?.Attributes["Pattern"].Value;
                    if (doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.WRNL.PresetPatterns.Lstg2.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value);
                    }
                    data.root.WRNL.PresetPatterns.Lstg2.Pattern = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg2"]?.Attributes["Pattern"].Value;
                    if (doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.WRNL.PresetPatterns.Lstg3.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value);
                    }
                    data.root.WRNL.PresetPatterns.Lstg3.Pattern = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg3"]?.Attributes["Pattern"].Value;
                    //Forced Patterns

                    data.root.WRNL.ForcedPatterns.MainHorn.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["MainHorn"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.MainHorn.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["MainHorn"].Attributes["Pattern"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone1.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone1"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone1.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone1"].Attributes["Pattern"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone2.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone2"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone2.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone2"].Attributes["Pattern"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone3.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone3"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone3.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone3"].Attributes["Pattern"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone4.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone4"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.SrnTone4.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone4"].Attributes["Pattern"].Value);
                    data.root.WRNL.ForcedPatterns.PanicMde.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["PanicMde"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.PanicMde.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["PanicMde"].Attributes["Pattern"].Value);
                    data.root.WRNL.ForcedPatterns.OutOfVeh.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Enabled"].Value);
                    data.root.WRNL.ForcedPatterns.OutOfVeh.Pattern = int.Parse(doc["vcfroot"]["WRNL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Pattern"].Value);
                    //Scan Patterns Custom Pool
                    CitizenFX.Core.Debug.WriteLine("Parsing Warning Lights scan");
                    data.root.WRNL.ScanPatternCustomPool.Enabled = bool.Parse(doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].Attributes["Enabled"].Value);
                    data.root.WRNL.ScanPatternCustomPool.Sequential = bool.Parse(doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].Attributes["Sequential"].Value);

                    for (int i = 0; i < doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].ChildNodes.Count; i++)
                    {
                        data.root.WRNL.ScanPatternCustomPool.Pattern.Add(int.Parse(doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].ChildNodes[i].InnerText));
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Warning lights for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }

                #endregion

                #region Primary Lights
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Primary Lights");
#endif
                try
                {
                    data.root.PRML.LightingFormat = doc["vcfroot"]["PRML"].Attributes["LightingFormat"].Value;
                    if (doc["vcfroot"]["PRML"].Attributes["DisableAtLstg3"] != null)
                    {
                        data.root.PRML.DisableAtLstg3 = bool.Parse(doc["vcfroot"]["PRML"].Attributes["DisableAtLstg3"]?.Value);
                    }
                    data.root.PRML.ExtrasActiveAtLstg1 = doc["vcfroot"]["PRML"].Attributes["ExtrasActiveAtLstg1"]?.Value;
                    data.root.PRML.ExtrasActiveAtLstg2 = doc["vcfroot"]["PRML"].Attributes["ExtrasActiveAtLstg2"]?.Value;
                    data.root.PRML.ExtrasActiveAtLstg3 = doc["vcfroot"]["PRML"].Attributes["ExtrasActiveAtLstg3"]?.Value;
                    //Preset Patterns
                    if (doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.PRML.PresetPatterns.Lstg1.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value);
                    }

                    data.root.PRML.PresetPatterns.Lstg1.Pattern = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg1"]?.Attributes["Pattern"].Value;

                    if (doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.PRML.PresetPatterns.Lstg2.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value);
                    }
                    data.root.PRML.PresetPatterns.Lstg2.Pattern = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg2"]?.Attributes["Pattern"].Value;
                    if (doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.PRML.PresetPatterns.Lstg3.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value);
                    }
                    data.root.PRML.PresetPatterns.Lstg3.Pattern = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg3"]?.Attributes["Pattern"].Value;
                    //Forced Patterns
                    data.root.PRML.ForcedPatterns.MainHorn.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["MainHorn"].Attributes["Enabled"].Value);
                    data.root.PRML.ForcedPatterns.MainHorn.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["MainHorn"].Attributes["Pattern"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone1.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone1"].Attributes["Enabled"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone1.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone1"].Attributes["Pattern"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone2.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone2"].Attributes["Enabled"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone2.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone2"].Attributes["Pattern"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone3.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone3"].Attributes["Enabled"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone3.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone3"].Attributes["Pattern"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone4.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone4"].Attributes["Enabled"].Value);
                    data.root.PRML.ForcedPatterns.SrnTone4.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone4"].Attributes["Pattern"].Value);
                    data.root.PRML.ForcedPatterns.PanicMde.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["PanicMde"].Attributes["Enabled"].Value);
                    data.root.PRML.ForcedPatterns.PanicMde.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["PanicMde"].Attributes["Pattern"].Value);
                    //data.root.PRML.ForcedPatterns.OutOfVeh.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["OutOfVeh"].Attributes["Enabled"].Value);
                    //data.root.PRML.ForcedPatterns.OutOfVeh.Pattern = int.Parse(doc["vcfroot"]["PRML"]["ForcedPatterns"]["OutOfVeh"].Attributes["Pattern"].Value);
                    //Scan Patterns Custom Pool
                    data.root.PRML.ScanPatternCustomPool.Enabled = bool.Parse(doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].Attributes["Enabled"].Value);
                    data.root.PRML.ScanPatternCustomPool.Sequential = bool.Parse(doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].Attributes["Sequential"].Value);

                    for (int i = 0; i < doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].ChildNodes.Count; i++)
                    {
                        data.root.PRML.ScanPatternCustomPool.Pattern.Add(int.Parse(doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].ChildNodes[i].InnerText));
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Primary Lights for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }

                #endregion

                #region Secondary Lights
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Secondary Lights");
#endif
                try
                {
                    data.root.SECL.LightingFormat = doc["vcfroot"]["SECL"].Attributes["LightingFormat"].Value;
                    if (doc["vcfroot"]["SECL"].Attributes["DisableAtLstg3"] != null)
                    {
                        data.root.SECL.DisableAtLstg3 = bool.Parse(doc["vcfroot"]["SECL"].Attributes["DisableAtLstg3"]?.Value);
                    }
                    data.root.SECL.ExtrasActiveAtLstg1 = doc["vcfroot"]["SECL"].Attributes["ExtrasActiveAtLstg1"]?.Value;
                    data.root.SECL.ExtrasActiveAtLstg2 = doc["vcfroot"]["SECL"].Attributes["ExtrasActiveAtLstg2"]?.Value;
                    data.root.SECL.ExtrasActiveAtLstg3 = doc["vcfroot"]["SECL"].Attributes["ExtrasActiveAtLstg3"]?.Value;
                    //Preset Patterns
                    if (doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.SECL.PresetPatterns.Lstg1.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value);
                    }
                    data.root.SECL.PresetPatterns.Lstg1.Pattern = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg1"]?.Attributes["Pattern"].Value;
                    if (doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.SECL.PresetPatterns.Lstg2.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value);
                    }
                    data.root.SECL.PresetPatterns.Lstg2.Pattern = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg2"]?.Attributes["Pattern"].Value;
                    if (doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value != null)
                    {
                        data.root.SECL.PresetPatterns.Lstg3.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value);
                    }
                    data.root.SECL.PresetPatterns.Lstg3.Pattern = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg3"]?.Attributes["Pattern"].Value;
                    //Forced Patterns
                    data.root.SECL.ForcedPatterns.MainHorn.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["MainHorn"].Attributes["Enabled"].Value);
                    data.root.SECL.ForcedPatterns.MainHorn.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["MainHorn"].Attributes["Pattern"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone1.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone1"].Attributes["Enabled"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone1.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone1"].Attributes["Pattern"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone2.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone2"].Attributes["Enabled"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone2.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone2"].Attributes["Pattern"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone3.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone3"].Attributes["Enabled"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone3.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone3"].Attributes["Pattern"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone4.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone4"].Attributes["Enabled"].Value);
                    data.root.SECL.ForcedPatterns.SrnTone4.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone4"].Attributes["Pattern"].Value);
                    data.root.SECL.ForcedPatterns.PanicMde.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["PanicMde"].Attributes["Enabled"].Value);
                    data.root.SECL.ForcedPatterns.PanicMde.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["PanicMde"].Attributes["Pattern"].Value);
                    //data.root.SECL.ForcedPatterns.OutOfVeh.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Enabled"].Value);
                    //data.root.SECL.ForcedPatterns.OutOfVeh.Pattern = int.Parse(doc["vcfroot"]["SECL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Pattern"].Value);
                    //Scan Patterns Custom Pool
                    data.root.SECL.ScanPatternCustomPool.Enabled = bool.Parse(doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].Attributes["Enabled"].Value);
                    data.root.SECL.ScanPatternCustomPool.Sequential = bool.Parse(doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].Attributes["Sequential"].Value);

                    for (int i = 0; i < doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].ChildNodes.Count; i++)
                    {
                        data.root.SECL.ScanPatternCustomPool.Pattern.Add(int.Parse(doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].ChildNodes[i].InnerText));
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Secondary Lights for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion


                //TODO: add method to remove old file or a file from ELSVehicle
                if (ELSVehicle.Exists(veh => veh.modelHash == hash))
                {
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Removeing preexisting VCF for resource ${ResourceName}");
#endif
                    ELSVehicle.RemoveAll(veh => veh.modelHash == hash);
                }
                ELSVehicle.Add(data);
                Utils.ReleaseWriteLine($"Added vehicle {data.filename}");
            }
        }
        internal static void unload(string hash)
        {
            var count = ELSVehicle.RemoveAll(veh => veh.modelHash.Equals(hash));
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"Unloaded {count} VCF for {hash}");
#endif
        }

        internal static bool isValidData(string data)
        {
            if (String.IsNullOrEmpty(data)) return false;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(data);
            //TODO change how below is detected to account for xml meta tag being before it.
            return doc.DocumentElement.Name == "vcfroot";
        }
    }
}
namespace ELS.configuration
{
    [XmlRoot(ElementName = "INTERFACE")]
    public class INTERFACE
    {
        [XmlElement(ElementName = "LstgActivationType")]
        public string LstgActivationType { get; set; }
        [XmlElement(ElementName = "DefaultSirenMode")]
        public string DefaultSirenMode { get; set; }
        [XmlElement(ElementName = "InfoPanelHeaderColor")]
        public string InfoPanelHeaderColor { get; set; }
        [XmlElement(ElementName = "InfoPanelButtonLightColor")]
        public string InfoPanelButtonLightColor { get; set; }
    }

    [XmlRoot(ElementName = "Extra01")]
    public class Extra
    {
        [XmlAttribute(AttributeName = "IsElsControlled")]
        public bool IsElsControlled { get; set; }
        [XmlAttribute(AttributeName = "AllowEnvLight")]
        public bool AllowEnvLight { get; set; }
        [XmlAttribute(AttributeName = "Color")]
        public string Color { get; set; }
        [XmlAttribute(AttributeName = "OffsetX")]
        public float OffsetX { get; set; }
        [XmlAttribute(AttributeName = "OffsetY")]
        public float OffsetY { get; set; }
        [XmlAttribute(AttributeName = "OffsetZ")]
        public float OffsetZ { get; set; }

        public Extra()
        {
            IsElsControlled = false;
            AllowEnvLight = false;
        }
    }

    [XmlRoot(ElementName = "EOVERRIDE")]
    public class EOVERRIDE
    {
        [XmlElement(ElementName = "Extra01")]
        public Extra Extra01 { get; set; }
        [XmlElement(ElementName = "Extra02")]
        public Extra Extra02 { get; set; }
        [XmlElement(ElementName = "Extra03")]
        public Extra Extra03 { get; set; }
        [XmlElement(ElementName = "Extra04")]
        public Extra Extra04 { get; set; }
        [XmlElement(ElementName = "Extra05")]
        public Extra Extra05 { get; set; }
        [XmlElement(ElementName = "Extra06")]
        public Extra Extra06 { get; set; }
        [XmlElement(ElementName = "Extra07")]
        public Extra Extra07 { get; set; }
        [XmlElement(ElementName = "Extra08")]
        public Extra Extra08 { get; set; }
        [XmlElement(ElementName = "Extra09")]
        public Extra Extra09 { get; set; }
        [XmlElement(ElementName = "Extra10")]
        public Extra Extra10 { get; set; }
        [XmlElement(ElementName = "Extra11")]
        public Extra Extra11 { get; set; }
        [XmlElement(ElementName = "Extra12")]
        public Extra Extra12 { get; set; }

        public EOVERRIDE()
        {
            Extra01 = new Extra();
            Extra02 = new Extra();
            Extra03 = new Extra();
            Extra04 = new Extra();
            Extra05 = new Extra();
            Extra06 = new Extra();
            Extra07 = new Extra();
            Extra08 = new Extra();
            Extra09 = new Extra();
            Extra10 = new Extra();
            Extra11 = new Extra();
            Extra12 = new Extra();
        }
    }

    [XmlRoot(ElementName = "Takedowns")]
    public class Takedowns
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public bool AllowUse { get; set; }
        [XmlAttribute(AttributeName = "Mirrored")]
        public bool Mirrored { get; set; }
    }

    [XmlRoot(ElementName = "SceneLights")]
    public class SceneLights
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public bool AllowUse { get; set; }
        [XmlAttribute(AttributeName = "IlluminateSidesOnly")]
        public bool IlluminateSidesOnly { get; set; }
    }

    [XmlRoot(ElementName = "MISC")]
    public class MISC
    {
        [XmlElement(ElementName = "VehicleIsSlicktop")]
        public bool VehicleIsSlicktop { get; set; }
        [XmlElement(ElementName = "ArrowboardType")]
        public string ArrowboardType { get; set; }
        [XmlElement(ElementName = "UseSteadyBurnLights")]
        public bool UseSteadyBurnLights { get; set; }
        [XmlElement(ElementName = "DfltSirenLtsActivateAtLstg")]
        public int DfltSirenLtsActivateAtLstg { get; set; }
        [XmlElement(ElementName = "Takedowns")]
        public Takedowns Takedowns { get; set; }
        [XmlElement(ElementName = "SceneLights")]
        public SceneLights SceneLights { get; set; }

        public MISC()
        {
            Takedowns = new Takedowns();
            SceneLights = new SceneLights();
        }
    }

    [XmlRoot(ElementName = "UseExtras")]
    public class UseExtras
    {
        [XmlAttribute(AttributeName = "Extra1")]
        public bool Extra1 { get; set; }
        [XmlAttribute(AttributeName = "Extra2")]
        public bool Extra2 { get; set; }
        [XmlAttribute(AttributeName = "Extra3")]
        public bool Extra3 { get; set; }
        [XmlAttribute(AttributeName = "Extra4")]
        public bool Extra4 { get; set; }

        public UseExtras()
        {
            Extra1 = true;
            Extra2 = true;
            Extra3 = true;
            Extra4 = true;
        }
    }

    [XmlRoot(ElementName = "CRUISE")]
    public class CRUISE
    {
        [XmlElement(ElementName = "DisableAtLstg3")]
        public bool DisableAtLstg3 { get; set; }
        [XmlElement(ElementName = "UseExtras")]
        public UseExtras UseExtras { get; set; }

        public CRUISE()
        {
            UseExtras = new UseExtras();
            DisableAtLstg3 = false;
        }
    }

    [XmlRoot(ElementName = "Headlights")]
    public class AcoronaLights
    {
        [XmlAttribute(AttributeName = "DfltPattern")]
        public int DfltPattern { get; set; }
        [XmlAttribute(AttributeName = "ColorL")]
        public string ColorL { get; set; }
        [XmlAttribute(AttributeName = "ColorR")]
        public string ColorR { get; set; }
    }


    [XmlRoot(ElementName = "ACORONAS")]
    public class ACORONAS
    {
        [XmlElement(ElementName = "Headlights")]
        public AcoronaLights Headlights { get; set; }
        [XmlElement(ElementName = "TailLights")]
        public AcoronaLights TailLights { get; set; }
        [XmlElement(ElementName = "IndicatorsF")]
        public AcoronaLights IndicatorsF { get; set; }
        [XmlElement(ElementName = "IndicatorsB")]
        public AcoronaLights IndicatorsB { get; set; }
        [XmlElement(ElementName = "ReverseLights")]
        public AcoronaLights ReverseLights { get; set; }

        public ACORONAS()
        {
            Headlights = new AcoronaLights();
            TailLights = new AcoronaLights();
            IndicatorsF = new AcoronaLights();
            IndicatorsB = new AcoronaLights();
            ReverseLights = new AcoronaLights();
        }
    }

    [XmlRoot(ElementName = "MainHorn")]
    public class MainHorn
    {
        [XmlAttribute(AttributeName = "InterruptsSiren")]
        public bool InterruptsSiren { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
    }

    public class SrnTone
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public bool AllowUse { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
    }

    public class PatternTone
    {
        public bool Enabled { get; set; }
        public int Pattern { get; set; }

        public PatternTone()
        {
            Enabled = false;
        }
    }

    [XmlRoot(ElementName = "SOUNDS")]
    public class SOUNDS
    {
        [XmlElement(ElementName = "MainHorn")]
        public MainHorn MainHorn { get; set; }
        [XmlElement(ElementName = "ManTone1")]
        public SrnTone ManTone1 { get; set; }
        [XmlElement(ElementName = "ManTone2")]
        public SrnTone ManTone2 { get; set; }
        [XmlElement(ElementName = "SrnTone1")]
        public SrnTone SrnTone1 { get; set; }
        [XmlElement(ElementName = "SrnTone2")]
        public SrnTone SrnTone2 { get; set; }
        [XmlElement(ElementName = "SrnTone3")]
        public SrnTone SrnTone3 { get; set; }
        [XmlElement(ElementName = "SrnTone4")]
        public SrnTone SrnTone4 { get; set; }
        [XmlElement(ElementName = "AuxSiren")]
        public SrnTone AuxSiren { get; set; }
        [XmlElement(ElementName = "PanicMde")]
        public SrnTone PanicMde { get; set; }

        public SOUNDS()
        {
            MainHorn = new MainHorn();
            ManTone1 = new SrnTone();
            ManTone2 = new SrnTone();
            SrnTone1 = new SrnTone();
            SrnTone2 = new SrnTone();
            SrnTone3 = new SrnTone();
            SrnTone4 = new SrnTone();
            AuxSiren = new SrnTone();
            PanicMde = new SrnTone();
        }
    }

    [XmlRoot(ElementName = "PresetPatterns")]
    public class PresetPatterns
    {
        [XmlElement(ElementName = "Lstg3")]
        public Lstg Lstg3 { get; set; }
        [XmlElement(ElementName = "Lstg2")]
        public Lstg Lstg2 { get; set; }
        [XmlElement(ElementName = "Lstg1")]
        public Lstg Lstg1 { get; set; }

        public PresetPatterns()
        {
            Lstg1 = new Lstg();
            Lstg2 = new Lstg();
            Lstg3 = new Lstg();
        }
    }

    [XmlRoot(ElementName = "ForcedPatterns")]
    public class ForcedPatterns
    {
        [XmlElement(ElementName = "MainHorn")]
        public PatternTone MainHorn { get; set; }
        [XmlElement(ElementName = "SrnTone1")]
        public PatternTone SrnTone1 { get; set; }
        [XmlElement(ElementName = "SrnTone2")]
        public PatternTone SrnTone2 { get; set; }
        [XmlElement(ElementName = "SrnTone3")]
        public PatternTone SrnTone3 { get; set; }
        [XmlElement(ElementName = "SrnTone4")]
        public PatternTone SrnTone4 { get; set; }
        [XmlElement(ElementName = "PanicMde")]
        public PatternTone PanicMde { get; set; }
        [XmlElement(ElementName = "OutOfVeh")]
        public PatternTone OutOfVeh { get; set; }

        public ForcedPatterns()
        {
            MainHorn = new PatternTone();
            SrnTone1 = new PatternTone();
            SrnTone2 = new PatternTone();
            SrnTone3 = new PatternTone();
            SrnTone4 = new PatternTone();
            PanicMde = new PatternTone();
            OutOfVeh = new PatternTone();
        }
    }

    [XmlRoot(ElementName = "ScanPatternCustomPool")]
    public class ScanPatternCustomPool
    {
        [XmlElement(ElementName = "Pattern")]
        public List<int> Pattern { get; set; }
        [XmlAttribute(AttributeName = "Enabled")]
        public bool Enabled { get; set; }
        [XmlAttribute(AttributeName = "Sequential")]
        public bool Sequential { get; set; }

        public ScanPatternCustomPool()
        {
            Pattern = new List<int>();
        }
    }

    public class Lstg
    {
        [XmlAttribute(AttributeName = "Enabled")]
        public bool Enabled { get; set; }
        [XmlAttribute(AttributeName = "Pattern")]
        public int IntPattern { get { return int.Parse(Pattern); } }
        public string Pattern { get; set; }
    }

    public class Lights
    {
        [XmlElement(ElementName = "PresetPatterns")]
        public PresetPatterns PresetPatterns { get; set; }
        [XmlElement(ElementName = "ForcedPatterns")]
        public ForcedPatterns ForcedPatterns { get; set; }
        [XmlElement(ElementName = "ScanPatternCustomPool")]
        public ScanPatternCustomPool ScanPatternCustomPool { get; set; }
        [XmlAttribute(AttributeName = "LightingFormat")]
        public string LightingFormat { get; set; }
        [XmlAttribute(AttributeName = "ExtrasActiveAtLstg2")]
        public string ExtrasActiveAtLstg1 { get; set; }
        public string ExtrasActiveAtLstg2 { get; set; }
        public string ExtrasActiveAtLstg3 { get; set; }
        public bool DisableAtLstg3 { get; set; }

        public Lights()
        {
            PresetPatterns = new PresetPatterns();
            ForcedPatterns = new ForcedPatterns();
            ScanPatternCustomPool = new ScanPatternCustomPool();
            DisableAtLstg3 = false;
        }
    }

    [XmlRoot(ElementName = "vcfroot")]
    public class Vcfroot
    {
        [XmlElement(ElementName = "INTERFACE")]
        public INTERFACE INTERFACE { get; set; }
        [XmlElement(ElementName = "EOVERRIDE")]
        public EOVERRIDE EOVERRIDE { get; set; }
        [XmlElement(ElementName = "MISC")]
        public MISC MISC { get; set; }
        [XmlElement(ElementName = "CRUISE")]
        public CRUISE CRUISE { get; set; }
        [XmlElement(ElementName = "ACORONAS")]
        public ACORONAS ACORONAS { get; set; }
        [XmlElement(ElementName = "SOUNDS")]
        public SOUNDS SOUNDS { get; set; }
        [XmlElement(ElementName = "WRNL")]
        public Lights WRNL { get; set; }
        [XmlElement(ElementName = "PRML")]
        public Lights PRML { get; set; }
        [XmlElement(ElementName = "SECL")]
        public Lights SECL { get; set; }
        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "Author")]
        public string Author { get; set; }

        public Vcfroot()
        {
            INTERFACE = new INTERFACE();
            EOVERRIDE = new EOVERRIDE();
            MISC = new MISC();
            CRUISE = new CRUISE();
            ACORONAS = new ACORONAS();
            SOUNDS = new SOUNDS();
            WRNL = new Lights();
            PRML = new Lights();
            SECL = new Lights();
        }
    }

}
