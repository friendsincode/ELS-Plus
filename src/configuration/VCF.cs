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
                data.root.INTERFACE.LstgActivationType = doc["vcfroot"]["INTERFACE"]["LstgActivationType"].Value;
                data.root.INTERFACE.DefaultSirenMode = doc["vcfroot"]["INTERFACE"]["DefaultSirenMode"].Value;
                data.root.INTERFACE.InfoPanelHeaderColor = doc["vcfroot"]["INTERFACE"]["InfoPanelHeaderColor"].Value;
                data.root.INTERFACE.InfoPanelButtonLightColor = doc["vcfroot"]["INTERFACE"]["InfoPanelButtonLightColor"].Value;
                #endregion
#region Extras Override
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Extras");
#endif
                //Extra 01
                data.root.EOVERRIDE.Extra01.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra01.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra01.Color = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra01.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra01.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra01.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra01"].Attributes["OffsetZ"].Value;
                //Extra 02
                data.root.EOVERRIDE.Extra02.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra02.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra02.Color = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra02.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra02.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra02.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra02"].Attributes["OffsetZ"].Value;
                //Extra 03
                data.root.EOVERRIDE.Extra03.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra03.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra03.Color = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra03.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra03.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra03.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra03"].Attributes["OffsetZ"].Value;
                //Extra 04
                data.root.EOVERRIDE.Extra04.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra04.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra04.Color = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra04.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra04.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra04.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra04"].Attributes["OffsetZ"].Value;
                //Extra 05
                data.root.EOVERRIDE.Extra05.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra05.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra05.Color = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra05.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra05.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra05.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra05"].Attributes["OffsetZ"].Value;
                //Extra 06
                data.root.EOVERRIDE.Extra06.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra06.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra06.Color = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra06.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra06.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra06.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra06"].Attributes["OffsetZ"].Value;
                //Extra 07
                data.root.EOVERRIDE.Extra07.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra07.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra07.Color = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra07.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra07.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra07.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra07"].Attributes["OffsetZ"].Value;
                //Extra 08
                data.root.EOVERRIDE.Extra08.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra08.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra08.Color = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra08.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra08.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra08.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra08"].Attributes["OffsetZ"].Value;
                //Extra 09
                data.root.EOVERRIDE.Extra09.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["IsElsControlled"].Value;
                data.root.EOVERRIDE.Extra09.AllowEnvLight = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["AllowEnvLight"].Value;
                data.root.EOVERRIDE.Extra09.Color = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["Color"].Value;
                data.root.EOVERRIDE.Extra09.OffsetX = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["OffsetX"].Value;
                data.root.EOVERRIDE.Extra09.OffsetY = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["OffsetY"].Value;
                data.root.EOVERRIDE.Extra09.OffsetZ = doc["vcfroot"]["EOVERRIDE"]["Extra09"].Attributes["OffsetZ"].Value;
                //Extra 10
                data.root.EOVERRIDE.Extra10.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra10"].Attributes["IsElsControlled"].Value;
                //Extra 11
                data.root.EOVERRIDE.Extra11.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra11"].Attributes["IsElsControlled"].Value;
                //Extra 12
                data.root.EOVERRIDE.Extra12.IsElsControlled = doc["vcfroot"]["EOVERRIDE"]["Extra12"].Attributes["IsElsControlled"].Value;
                #endregion
#region MISC
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Misc");
#endif
                data.root.MISC.VehicleIsSlicktop = doc["vcfroot"]["MISC"]["VehicleIsSlicktop"].InnerText;
                data.root.MISC.ArrowboardType = doc["vcfroot"]["MISC"]["ArrowboardType"].InnerText;
                data.root.MISC.UseSteadyBurnLights = doc["vcfroot"]["MISC"]["UseSteadyBurnLights"].InnerText;
                data.root.MISC.DfltSirenLtsActivateAtLstg = doc["vcfroot"]["MISC"]["DfltSirenLtsActivateAtLstg"].InnerText;
                data.root.MISC.Takedowns.AllowUse = doc["vcfroot"]["MISC"]["Takedowns"].Attributes["AllowUse"].Value;
                data.root.MISC.Takedowns.Mirrored = doc["vcfroot"]["MISC"]["Takedowns"].Attributes["Mirrored"].Value;
                data.root.MISC.SceneLights.AllowUse = doc["vcfroot"]["MISC"]["SceneLights"].Attributes["AllowUse"].Value;
                data.root.MISC.SceneLights.AllowUse = doc["vcfroot"]["MISC"]["SceneLights"].Attributes["IlluminateSidesOnly"].Value;
                #endregion
#region Cruise
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Cruise");
#endif
                data.root.CRUISE.DisableAtLstg3 = bool.Parse(doc["vcfroot"]["CRUISE"]["DisableAtLstg3"].InnerText);
                data.root.CRUISE.UseExtras.Extra1 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra1"].Value);
                data.root.CRUISE.UseExtras.Extra2 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra2"].Value);
                data.root.CRUISE.UseExtras.Extra3 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra3"].Value);
                data.root.CRUISE.UseExtras.Extra4 = bool.Parse(doc["vcfroot"]["CRUISE"]["UseExtras"].Attributes["Extra4"].Value);
                #endregion
#region Aux Coronas
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Aux Coronas");
#endif
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
                #endregion
#region Sounds
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Sounds");
#endif
                //Manual Tone 1
                data.root.SOUNDS.ManTone1.AudioString = doc?["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AudioString"].Value;
                data.root.SOUNDS.ManTone1.AllowUse = doc["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AllowUse"].Value;
                //Manual Tone 2
                data.root.SOUNDS.ManTone2.AudioString = doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AudioString"].Value;
                data.root.SOUNDS.ManTone2.AllowUse = doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AllowUse"].Value;
                //Main Horn
                data.root.SOUNDS.MainHorn.AudioString = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["AudioString"].Value;
                data.root.SOUNDS.MainHorn.InterruptsSiren = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["InterruptsSiren"].Value;
                //Siren Tone 1
                data.root.SOUNDS.SrnTone1.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone1.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AllowUse"].Value;
                //Siren Tone 2
                data.root.SOUNDS.SrnTone2.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone2.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AllowUse"].Value;
                //Siren Tone 3
                data.root.SOUNDS.SrnTone3.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone3.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AllowUse"].Value;
                //Siren Tone 4
                data.root.SOUNDS.SrnTone4.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone4.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AllowUse"].Value;
                //Aux Siren Tone
                data.root.SOUNDS.AuxSiren.AllowUse = doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AllowUse"].Value;
                data.root.SOUNDS.AuxSiren.AudioString = doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AudioString"].Value;
                //Panic Mode Tone
                data.root.SOUNDS.PanicMde.AllowUse = doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AllowUse"].Value;
                data.root.SOUNDS.PanicMde.AudioString = doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AudioString"].Value;
                #endregion

#region Warning Lights
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Warning Lights");
#endif
                data.root.WRNL.LightingFormat = doc["vcfroot"]["WRNL"].Attributes["LightingFormat"].Value;
                data.root.WRNL.DisableAtLstg3 = doc["vcfroot"]["WRNL"].Attributes["DisableAtLstg3"]?.Value;
                data.root.WRNL.ExtrasActiveAtLstg1 = doc["vcfroot"]["WRNL"].Attributes["ExtrasActiveAtLstg1"]?.Value;
                data.root.WRNL.ExtrasActiveAtLstg2 = doc["vcfroot"]["WRNL"].Attributes["ExtrasActiveAtLstg2"]?.Value;
                data.root.WRNL.ExtrasActiveAtLstg3 = doc["vcfroot"]["WRNL"].Attributes["ExtrasActiveAtLstg3"]?.Value;
                //Preset Patterns
                data.root.WRNL.PresetPatterns.Lstg1.Enabled = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value;
                data.root.WRNL.PresetPatterns.Lstg1.Pattern = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg1"]?.Attributes["Pattern"].Value;
                data.root.WRNL.PresetPatterns.Lstg2.Enabled = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value;
                data.root.WRNL.PresetPatterns.Lstg2.Pattern = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg2"]?.Attributes["Pattern"].Value;
                data.root.WRNL.PresetPatterns.Lstg3.Enabled = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value;
                data.root.WRNL.PresetPatterns.Lstg3.Pattern = doc["vcfroot"]["WRNL"]["PresetPatterns"]["Lstg3"]?.Attributes["Pattern"].Value;
                //Forced Patterns

                data.root.WRNL.ForcedPatterns.MainHorn.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["MainHorn"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.MainHorn.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["MainHorn"].Attributes["Pattern"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone1.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone1"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone1.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone1"].Attributes["Pattern"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone2.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone2"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone2.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone2"].Attributes["Pattern"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone3.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone3"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone3.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone3"].Attributes["Pattern"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone4.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone4"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.SrnTone4.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["SrnTone4"].Attributes["Pattern"].Value;
                data.root.WRNL.ForcedPatterns.PanicMde.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["PanicMde"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.PanicMde.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["PanicMde"].Attributes["Pattern"].Value;
                data.root.WRNL.ForcedPatterns.OutOfVeh.Enabled = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Enabled"].Value;
                data.root.WRNL.ForcedPatterns.OutOfVeh.Pattern = doc["vcfroot"]["WRNL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Pattern"].Value;
                //Scan Patterns Custom Pool
                CitizenFX.Core.Debug.WriteLine("Parsing Warning Lights scan");
                data.root.WRNL.ScanPatternCustomPool.Enabled = doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].Attributes["Enabled"].Value;
                data.root.WRNL.ScanPatternCustomPool.Sequential = doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].Attributes["Sequential"].Value;

                for (int i = 0; i < doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].ChildNodes.Count; i++)
                {
                    data.root.WRNL.ScanPatternCustomPool.Pattern.Add(doc["vcfroot"]["WRNL"]["ScanPatternCustomPool"].ChildNodes[i].InnerText);
                }

                #endregion

#region Primary Lights
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Primary Lights");
#endif
                data.root.PRML.LightingFormat = doc["vcfroot"]["PRML"].Attributes["LightingFormat"].Value;
                data.root.PRML.DisableAtLstg3 = doc["vcfroot"]["PRML"].Attributes["DisableAtLstg3"]?.Value;
                data.root.PRML.ExtrasActiveAtLstg1 = doc["vcfroot"]["PRML"].Attributes["ExtrasActiveAtLstg1"]?.Value;
                data.root.PRML.ExtrasActiveAtLstg2 = doc["vcfroot"]["PRML"].Attributes["ExtrasActiveAtLstg2"]?.Value;
                data.root.PRML.ExtrasActiveAtLstg3 = doc["vcfroot"]["PRML"].Attributes["ExtrasActiveAtLstg3"]?.Value;
                //Preset Patterns
                data.root.PRML.PresetPatterns.Lstg1.Enabled = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value;
                data.root.PRML.PresetPatterns.Lstg1.Pattern = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg1"]?.Attributes["Pattern"].Value;
                data.root.PRML.PresetPatterns.Lstg2.Enabled = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value;
                data.root.PRML.PresetPatterns.Lstg2.Pattern = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg2"]?.Attributes["Pattern"].Value;
                data.root.PRML.PresetPatterns.Lstg3.Enabled = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value;
                data.root.PRML.PresetPatterns.Lstg3.Pattern = doc["vcfroot"]["PRML"]["PresetPatterns"]["Lstg3"]?.Attributes["Pattern"].Value;
                //Forced Patterns
                data.root.PRML.ForcedPatterns.MainHorn.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["MainHorn"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.MainHorn.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["MainHorn"].Attributes["Pattern"].Value;
                data.root.PRML.ForcedPatterns.SrnTone1.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone1"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.SrnTone1.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone1"].Attributes["Pattern"].Value;
                data.root.PRML.ForcedPatterns.SrnTone2.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone2"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.SrnTone2.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone2"].Attributes["Pattern"].Value;
                data.root.PRML.ForcedPatterns.SrnTone3.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone3"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.SrnTone3.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone3"].Attributes["Pattern"].Value;
                data.root.PRML.ForcedPatterns.SrnTone4.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone4"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.SrnTone4.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["SrnTone4"].Attributes["Pattern"].Value;
                data.root.PRML.ForcedPatterns.PanicMde.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["PanicMde"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.PanicMde.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["PanicMde"].Attributes["Pattern"].Value;
                data.root.PRML.ForcedPatterns.OutOfVeh.Enabled = doc["vcfroot"]["PRML"]["ForcedPatterns"]["OutOfVeh"].Attributes["Enabled"].Value;
                data.root.PRML.ForcedPatterns.OutOfVeh.Pattern = doc["vcfroot"]["PRML"]["ForcedPatterns"]["OutOfVeh"].Attributes["Pattern"].Value;
                //Scan Patterns Custom Pool
                data.root.PRML.ScanPatternCustomPool.Enabled = doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].Attributes["Enabled"].Value;
                data.root.PRML.ScanPatternCustomPool.Sequential = doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].Attributes["Sequential"].Value;

                for (int i = 0; i < doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].ChildNodes.Count; i++)
                {
                    data.root.PRML.ScanPatternCustomPool.Pattern.Add(doc["vcfroot"]["PRML"]["ScanPatternCustomPool"].ChildNodes[i].InnerText);
                }

                #endregion

#region Secondary Lights
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Parsing Secondary Lights");
#endif
                data.root.SECL.LightingFormat = doc["vcfroot"]["SECL"].Attributes["LightingFormat"].Value;
                data.root.SECL.DisableAtLstg3 = doc["vcfroot"]["SECL"].Attributes["DisableAtLstg3"]?.Value;
                data.root.SECL.ExtrasActiveAtLstg1 = doc["vcfroot"]["SECL"].Attributes["ExtrasActiveAtLstg1"]?.Value;
                data.root.SECL.ExtrasActiveAtLstg2 = doc["vcfroot"]["SECL"].Attributes["ExtrasActiveAtLstg2"]?.Value;
                data.root.SECL.ExtrasActiveAtLstg3 = doc["vcfroot"]["SECL"].Attributes["ExtrasActiveAtLstg3"]?.Value;
                //Preset Patterns
                data.root.SECL.PresetPatterns.Lstg1.Enabled = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg1"]?.Attributes["Enabled"].Value;
                data.root.SECL.PresetPatterns.Lstg1.Pattern = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg1"]?.Attributes["Pattern"].Value;
                data.root.SECL.PresetPatterns.Lstg2.Enabled = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg2"]?.Attributes["Enabled"].Value;
                data.root.SECL.PresetPatterns.Lstg2.Pattern = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg2"]?.Attributes["Pattern"].Value;
                data.root.SECL.PresetPatterns.Lstg3.Enabled = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg3"]?.Attributes["Enabled"].Value;
                data.root.SECL.PresetPatterns.Lstg3.Pattern = doc["vcfroot"]["SECL"]["PresetPatterns"]["Lstg3"]?.Attributes["Pattern"].Value;
                //Forced Patterns
                data.root.SECL.ForcedPatterns.MainHorn.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["MainHorn"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.MainHorn.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["MainHorn"].Attributes["Pattern"].Value;
                data.root.SECL.ForcedPatterns.SrnTone1.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone1"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.SrnTone1.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone1"].Attributes["Pattern"].Value;
                data.root.SECL.ForcedPatterns.SrnTone2.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone2"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.SrnTone2.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone2"].Attributes["Pattern"].Value;
                data.root.SECL.ForcedPatterns.SrnTone3.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone3"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.SrnTone3.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone3"].Attributes["Pattern"].Value;
                data.root.SECL.ForcedPatterns.SrnTone4.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone4"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.SrnTone4.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["SrnTone4"].Attributes["Pattern"].Value;
                data.root.SECL.ForcedPatterns.PanicMde.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["PanicMde"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.PanicMde.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["PanicMde"].Attributes["Pattern"].Value;
                data.root.SECL.ForcedPatterns.OutOfVeh.Enabled = doc["vcfroot"]["SECL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Enabled"].Value;
                data.root.SECL.ForcedPatterns.OutOfVeh.Pattern = doc["vcfroot"]["SECL"]["ForcedPatterns"]["OutOfVeh"].Attributes["Pattern"].Value;
                //Scan Patterns Custom Pool
                data.root.SECL.ScanPatternCustomPool.Enabled = doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].Attributes["Enabled"].Value;
                data.root.SECL.ScanPatternCustomPool.Sequential = doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].Attributes["Sequential"].Value;

                for (int i = 0; i < doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].ChildNodes.Count; i++)
                {
                    data.root.SECL.ScanPatternCustomPool.Pattern.Add(doc["vcfroot"]["SECL"]["ScanPatternCustomPool"].ChildNodes[i].InnerText);
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
                CitizenFX.Core.Debug.WriteLine($"Added vehicle {data.filename}");
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
        public string IsElsControlled { get; set; }
        [XmlAttribute(AttributeName = "AllowEnvLight")]
        public string AllowEnvLight { get; set; }
        [XmlAttribute(AttributeName = "Color")]
        public string Color { get; set; }
        [XmlAttribute(AttributeName = "OffsetX")]
        public string OffsetX { get; set; }
        [XmlAttribute(AttributeName = "OffsetY")]
        public string OffsetY { get; set; }
        [XmlAttribute(AttributeName = "OffsetZ")]
        public string OffsetZ { get; set; }
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
        public string AllowUse { get; set; }
        [XmlAttribute(AttributeName = "Mirrored")]
        public string Mirrored { get; set; }
    }

    [XmlRoot(ElementName = "SceneLights")]
    public class SceneLights
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public string AllowUse { get; set; }
        [XmlAttribute(AttributeName = "IlluminateSidesOnly")]
        public string IlluminateSidesOnly { get; set; }
    }

    [XmlRoot(ElementName = "MISC")]
    public class MISC
    {
        [XmlElement(ElementName = "VehicleIsSlicktop")]
        public string VehicleIsSlicktop { get; set; }
        [XmlElement(ElementName = "ArrowboardType")]
        public string ArrowboardType { get; set; }
        [XmlElement(ElementName = "UseSteadyBurnLights")]
        public string UseSteadyBurnLights { get; set; }
        [XmlElement(ElementName = "DfltSirenLtsActivateAtLstg")]
        public string DfltSirenLtsActivateAtLstg { get; set; }
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
        public string InterruptsSiren { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "Pattern")]
        public string Pattern { get; set; }
    }

    [XmlRoot(ElementName = "ManTone1")]
    public class ManTone
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public string AllowUse { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
    }


    [XmlRoot(ElementName = "SrnTone1")]
    public class SrnTone
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public string AllowUse { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "Pattern")]
        public string Pattern { get; set; }
    }


    [XmlRoot(ElementName = "AuxSiren")]
    public class AuxSiren
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public string AllowUse { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
    }

    [XmlRoot(ElementName = "PanicMde")]
    public class PanicMde
    {
        [XmlAttribute(AttributeName = "AllowUse")]
        public string AllowUse { get; set; }
        [XmlAttribute(AttributeName = "AudioString")]
        public string AudioString { get; set; }
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "Pattern")]
        public string Pattern { get; set; }
    }

    [XmlRoot(ElementName = "SOUNDS")]
    public class SOUNDS
    {
        [XmlElement(ElementName = "MainHorn")]
        public MainHorn MainHorn { get; set; }
        [XmlElement(ElementName = "ManTone1")]
        public ManTone ManTone1 { get; set; }
        [XmlElement(ElementName = "ManTone2")]
        public ManTone ManTone2 { get; set; }
        [XmlElement(ElementName = "SrnTone1")]
        public SrnTone SrnTone1 { get; set; }
        [XmlElement(ElementName = "SrnTone2")]
        public SrnTone SrnTone2 { get; set; }
        [XmlElement(ElementName = "SrnTone3")]
        public SrnTone SrnTone3 { get; set; }
        [XmlElement(ElementName = "SrnTone4")]
        public SrnTone SrnTone4 { get; set; }
        [XmlElement(ElementName = "AuxSiren")]
        public AuxSiren AuxSiren { get; set; }
        [XmlElement(ElementName = "PanicMde")]
        public PanicMde PanicMde { get; set; }

        public SOUNDS()
        {
            MainHorn = new MainHorn();
            ManTone1 = new ManTone();
            ManTone2 = new ManTone();
            SrnTone1 = new SrnTone();
            SrnTone2 = new SrnTone();
            SrnTone3 = new SrnTone();
            SrnTone4 = new SrnTone();
            AuxSiren = new AuxSiren();
            PanicMde = new PanicMde();
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

    [XmlRoot(ElementName = "OutOfVeh")]
    public class OutOfVeh
    {
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "Pattern")]
        public string Pattern { get; set; }
    }

    [XmlRoot(ElementName = "ForcedPatterns")]
    public class ForcedPatterns
    {
        [XmlElement(ElementName = "MainHorn")]
        public MainHorn MainHorn { get; set; }
        [XmlElement(ElementName = "SrnTone1")]
        public SrnTone SrnTone1 { get; set; }
        [XmlElement(ElementName = "SrnTone2")]
        public SrnTone SrnTone2 { get; set; }
        [XmlElement(ElementName = "SrnTone3")]
        public SrnTone SrnTone3 { get; set; }
        [XmlElement(ElementName = "SrnTone4")]
        public SrnTone SrnTone4 { get; set; }
        [XmlElement(ElementName = "PanicMde")]
        public PanicMde PanicMde { get; set; }
        [XmlElement(ElementName = "OutOfVeh")]
        public OutOfVeh OutOfVeh { get; set; }

        public ForcedPatterns()
        {
            MainHorn = new MainHorn();
            SrnTone1 = new SrnTone();
            SrnTone2 = new SrnTone();
            SrnTone3 = new SrnTone();
            SrnTone4 = new SrnTone();
            PanicMde = new PanicMde();
            OutOfVeh = new OutOfVeh();
        }
    }

    [XmlRoot(ElementName = "ScanPatternCustomPool")]
    public class ScanPatternCustomPool
    {
        [XmlElement(ElementName = "Pattern")]
        public List<string> Pattern { get; set; }
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "Sequential")]
        public string Sequential { get; set; }
        
        public ScanPatternCustomPool()
        {
            Pattern = new List<string>();
        }
    }

    public class Lstg
    {
        [XmlAttribute(AttributeName = "Enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "Pattern")]
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
        public string DisableAtLstg3 { get; set; }

        public Lights()
        {
            PresetPatterns = new PresetPatterns();
            ForcedPatterns = new ForcedPatterns();
            ScanPatternCustomPool = new ScanPatternCustomPool();
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
