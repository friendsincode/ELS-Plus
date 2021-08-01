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
//using System.Xml.Serialization;
using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        internal static Dictionary<int, VCFEntry> ELSVehicle = new Dictionary<int, VCFEntry>();
        public VCF()
        {
        }

        internal static async void ParseVcfs(List<dynamic> VcfData)
        {
            foreach (dynamic vcf in VcfData)
            {
                Utils.DebugWriteLine($"Currently adding {vcf.Item2} from {vcf.Item1}");
                load(SettingsType.Type.VCF, vcf.Item2, vcf.Item3, vcf.Item1);
            }
        }

        static void load(SettingsType.Type type, string name, string Data, string ResourceName)
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
                //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                NanoXMLDocument doc = new NanoXMLDocument(Data);
                // doc.LoadXml(Data);
                bool res;
                //data.filename = Path.GetFileNameWithoutExtension(name);
                if (data.root == null)
                {
                    CitizenFX.Core.Debug.WriteLine("Null issue");
                    return;
                }
                Dictionary<string, NanoXMLNode> subNodes = new Dictionary<string, NanoXMLNode>();
                foreach (NanoXMLNode node in doc.RootNode.SubNodes)
                {
                    subNodes.Add(node.Name, node);
                }
                #region VCF Info
                Utils.ReleaseWriteLine($"Parsing VCF Info for vehicle {name}");
                //VCF Description
                if (doc.RootNode.GetAttribute("Description") != null)
                {
                    data.root.Description = doc.RootNode.GetAttribute("Description").Value;
                }

                //VCF Author
                if (doc.RootNode.GetAttribute("Author") != null)
                {
                    data.root.Author = doc.RootNode.GetAttribute("Author").Value;
                }
                #endregion
                #region Interface

                Utils.DebugWriteLine("Parsing Interface");
                try
                {
                    foreach (NanoXMLNode n in subNodes["INTERFACE"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "LstgActivationType":
                                data.root.INTERFACE.LstgActivationType = n.Value;
                                break;
                            case "DefaultSirenMode":
                                data.root.INTERFACE.DefaultSirenMode = n.Value;
                                break;
                            case "InfoPanelHeaderColor":
                                data.root.INTERFACE.InfoPanelHeaderColor = n.Value;
                                break;
                            case "InfoPanelButtonLightColor":
                                data.root.INTERFACE.InfoPanelButtonLightColor = n.Value;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Extras Override
                Utils.DebugWriteLine("Parsing Extras");

                try
                {
                    foreach (NanoXMLNode n in subNodes["EOVERRIDE"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "Extra01":
                                data.root.EOVERRIDE.Extra01.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra01.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra01.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra01.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra01.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra01.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra02":
                                data.root.EOVERRIDE.Extra02.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra02.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra02.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra02.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra02.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra02.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra03":
                                data.root.EOVERRIDE.Extra03.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra03.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra03.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra03.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra03.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra03.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra04":
                                data.root.EOVERRIDE.Extra04.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra04.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra04.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra04.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra04.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra04.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra05":
                                data.root.EOVERRIDE.Extra05.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra05.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra05.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra05.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra05.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra05.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra06":
                                data.root.EOVERRIDE.Extra06.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra06.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra06.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra06.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra06.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra06.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra07":
                                data.root.EOVERRIDE.Extra07.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra07.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra07.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra07.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra07.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra07.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra08":
                                data.root.EOVERRIDE.Extra08.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra08.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra08.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra08.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra08.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra08.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra09":
                                data.root.EOVERRIDE.Extra09.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                data.root.EOVERRIDE.Extra09.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                data.root.EOVERRIDE.Extra09.Color = n.GetAttribute("Color").Value;
                                data.root.EOVERRIDE.Extra09.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                data.root.EOVERRIDE.Extra09.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                data.root.EOVERRIDE.Extra09.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra10":
                                data.root.EOVERRIDE.Extra10.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                //data.root.EOVERRIDE.Extra10.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                //data.root.EOVERRIDE.Extra10.Color = n.GetAttribute("Color").Value;
                                //data.root.EOVERRIDE.Extra10.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                //data.root.EOVERRIDE.Extra10.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                //data.root.EOVERRIDE.Extra10.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra11":
                                data.root.EOVERRIDE.Extra11.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                //data.root.EOVERRIDE.Extra11.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                //data.root.EOVERRIDE.Extra11.Color = n.GetAttribute("Color").Value; 
                                //data.root.EOVERRIDE.Extra11.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                //data.root.EOVERRIDE.Extra11.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                //data.root.EOVERRIDE.Extra11.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                            case "Extra12":
                                data.root.EOVERRIDE.Extra12.IsElsControlled = bool.Parse(n.GetAttribute("IsElsControlled").Value);
                                //data.root.EOVERRIDE.Extra12.AllowEnvLight = bool.Parse(n.GetAttribute("AllowEnvLight").Value);
                                //data.root.EOVERRIDE.Extra12.Color = n.GetAttribute("Color").Value;
                                //data.root.EOVERRIDE.Extra12.OffsetX = float.Parse(n.GetAttribute("OffsetX").Value);
                                //data.root.EOVERRIDE.Extra12.OffsetY = float.Parse(n.GetAttribute("OffsetY").Value);
                                //data.root.EOVERRIDE.Extra12.OffsetZ = float.Parse(n.GetAttribute("OffsetZ").Value);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"EOverride for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region MISC
                Utils.DebugWriteLine("Parsing Misc");

                try
                {
                    foreach (NanoXMLNode n in subNodes["MISC"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "VehicleIsSlicktop":
                                data.root.MISC.VehicleIsSlicktop = bool.Parse(n.Value);
                                break;
                            case "ArrowboardType":
                                data.root.MISC.ArrowboardType = n.Value;
                                break;
                            case "UseSteadyBurnLights":
                                data.root.MISC.UseSteadyBurnLights = bool.Parse(n.Value);
                                break;
                            case "DfltSirenLtsActivateAtLstg":
                                data.root.MISC.DfltSirenLtsActivateAtLstg = int.Parse(n.Value);
                                break;
                            case "Takedowns":
                                data.root.MISC.Takedowns.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.MISC.Takedowns.Mirrored = bool.Parse(n.GetAttribute("Mirrored").Value);
                                break;
                            case "SceneLights":
                                data.root.MISC.SceneLights.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.MISC.SceneLights.IlluminateSidesOnly = bool.Parse(n.GetAttribute("IlluminateSidesOnly").Value);
                                break;
                            case "LadderControl":
                                data.root.MISC.HasLadderControl = bool.Parse(n.GetAttribute("enabled").Value);
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "HoriztonalControl":
                                            data.root.MISC.LadderControl.HorizontalControl = sn.Value;
                                            break;
                                        case "VerticalControl":
                                            data.root.MISC.LadderControl.VerticalControl = sn.Value;
                                            break;
                                        case "MovementSpeed":
                                            data.root.MISC.LadderControl.MovementSpeed = int.Parse(sn.Value);
                                            break;
                                    }
                                }
                                break;

                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Misc for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Cruise
                Utils.DebugWriteLine("Parsing Cruise");
                try
                {
                    foreach (NanoXMLNode n in subNodes["CRUISE"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "DisableAtLstg3":
                                data.root.CRUISE.DisableAtLstg3 = bool.Parse(n.Value);
                                break;
                            case "UseExtras":
                                data.root.CRUISE.UseExtras.Extra1 = bool.Parse(n.GetAttribute("Extra1").Value);
                                data.root.CRUISE.UseExtras.Extra2 = bool.Parse(n.GetAttribute("Extra2").Value);
                                data.root.CRUISE.UseExtras.Extra3 = bool.Parse(n.GetAttribute("Extra3").Value);
                                data.root.CRUISE.UseExtras.Extra4 = bool.Parse(n.GetAttribute("Extra4").Value);
                                break;

                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Cruise for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Aux Coronas
                Utils.DebugWriteLine("Parsing Aux Coronas");
                try
                {
                    foreach (NanoXMLNode n in subNodes["ACORONAS"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "Headlights":
                                data.root.ACORONAS.Headlights.DfltPattern = int.Parse(n.GetAttribute("DfltPattern").Value);
                                data.root.ACORONAS.Headlights.ColorL = n.GetAttribute("ColorL").Value;
                                data.root.ACORONAS.Headlights.ColorR = n.GetAttribute("ColorR").Value;
                                break;
                            case "TailLights":
                                data.root.ACORONAS.TailLights.DfltPattern = int.Parse(n.GetAttribute("DfltPattern").Value);
                                data.root.ACORONAS.TailLights.ColorL = n.GetAttribute("ColorL").Value;
                                data.root.ACORONAS.TailLights.ColorR = n.GetAttribute("ColorR").Value;
                                break;
                            case "IndicatorsF":
                                data.root.ACORONAS.IndicatorsF.DfltPattern = int.Parse(n.GetAttribute("DfltPattern").Value);
                                data.root.ACORONAS.IndicatorsF.ColorL = n.GetAttribute("ColorL").Value;
                                data.root.ACORONAS.IndicatorsF.ColorR = n.GetAttribute("ColorR").Value;
                                break;
                            case "IndicatorsB":
                                data.root.ACORONAS.IndicatorsB.DfltPattern = int.Parse(n.GetAttribute("DfltPattern").Value);
                                data.root.ACORONAS.IndicatorsB.ColorL = n.GetAttribute("ColorL").Value;
                                data.root.ACORONAS.IndicatorsB.ColorR = n.GetAttribute("ColorR").Value;
                                break;
                            case "ReverseLights":
                                data.root.ACORONAS.ReverseLights.DfltPattern = int.Parse(n.GetAttribute("DfltPattern").Value);
                                data.root.ACORONAS.ReverseLights.ColorL = n.GetAttribute("ColorL").Value;
                                data.root.ACORONAS.ReverseLights.ColorR = n.GetAttribute("ColorR").Value;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"ACoronas for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Sounds
                Utils.DebugWriteLine("Parsing Sounds");
                try
                {
                    foreach (NanoXMLNode n in subNodes["SOUNDS"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "ManTone1":
                                data.root.SOUNDS.ManTone1.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.ManTone1.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.ManTone1.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                data.root.SOUNDS.ManTone1.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                break;
                            case "ManTone2":
                                data.root.SOUNDS.ManTone2.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.ManTone2.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.ManTone2.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.ManTone2.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "MainHorn":
                                data.root.SOUNDS.MainHorn.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.MainHorn.InterruptsSiren = bool.Parse(n.GetAttribute("InterruptsSiren").Value);
                                data.root.SOUNDS.MainHorn.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.MainHorn.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "SrnTone1":
                                data.root.SOUNDS.SrnTone1.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.SrnTone1.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.SrnTone1.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.SrnTone1.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "SrnTone2":
                                data.root.SOUNDS.SrnTone2.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.SrnTone2.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.SrnTone2.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.SrnTone2.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "SrnTone3":
                                data.root.SOUNDS.SrnTone3.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.SrnTone3.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.SrnTone3.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.SrnTone3.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "SrnTone4":
                                data.root.SOUNDS.SrnTone4.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.SrnTone4.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.SrnTone4.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.SrnTone4.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "AuxSiren":
                                data.root.SOUNDS.AuxSiren.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.AuxSiren.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.AuxSiren.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.AuxSiren.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;
                            case "PanicMde":
                                data.root.SOUNDS.PanicMde.AllowUse = bool.Parse(n.GetAttribute("AllowUse").Value);
                                data.root.SOUNDS.PanicMde.AudioString = n.GetAttribute("AudioString").Value;
                                data.root.SOUNDS.PanicMde.SoundBank = n.GetAttribute("SoundBank")?.Value;
                                data.root.SOUNDS.PanicMde.SoundSet = n.GetAttribute("SoundSet")?.Value;
                                break;

                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Sounds for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                #region Warning Lights

                Utils.DebugWriteLine("Parsing Warning Lights");
                try
                {
                    data.root.WRNL.LightingFormat = subNodes["WRNL"].GetAttribute("LightingFormat").Value;
                    if (subNodes["WRNL"].GetAttribute("DisableAtLstg3") != null)
                    {
                        data.root.WRNL.DisableAtLstg3 = bool.Parse(subNodes["WRNL"].GetAttribute("DisableAtLstg3").Value);
                    }

                    data.root.WRNL.ExtrasActiveAtLstg1 = subNodes["WRNL"].GetAttribute("ExtrasActiveAtLstg1")?.Value;
                    data.root.WRNL.ExtrasActiveAtLstg2 = subNodes["WRNL"].GetAttribute("ExtrasActiveAtLstg2")?.Value;
                    data.root.WRNL.ExtrasActiveAtLstg3 = subNodes["WRNL"].GetAttribute("ExtrasActiveAtLstg3")?.Value;

                    foreach (NanoXMLNode n in subNodes["WRNL"].SubNodes)
                    {
                        switch (n.Name)
                        {

                            case "PresetPatterns":
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "Lstg1":
                                            data.root.WRNL.PresetPatterns.Lstg1.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.PresetPatterns.Lstg1.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.PresetPatterns.Lstg1.IntPattern = int.Parse(data.root.WRNL.PresetPatterns.Lstg1.Pattern);
                                            }
                                            break;
                                        case "Lstg2":
                                            data.root.WRNL.PresetPatterns.Lstg2.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.PresetPatterns.Lstg2.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.PresetPatterns.Lstg2.IntPattern = int.Parse(data.root.WRNL.PresetPatterns.Lstg2.Pattern);
                                            }
                                            break;
                                        case "Lstg3":
                                            data.root.WRNL.PresetPatterns.Lstg3.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.PresetPatterns.Lstg3.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.PresetPatterns.Lstg3.IntPattern = int.Parse(data.root.WRNL.PresetPatterns.Lstg3.Pattern);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "ForcedPatterns":
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "MainHorn":
                                            data.root.WRNL.ForcedPatterns.MainHorn.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.MainHorn.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.MainHorn.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.MainHorn.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.MainHorn.Pattern);
                                            }
                                            break;
                                        case "SrnTone1":
                                            data.root.WRNL.ForcedPatterns.SrnTone1.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.SrnTone1.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.SrnTone1.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.SrnTone1.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.SrnTone1.Pattern);
                                            }
                                            break;
                                        case "SrnTone2":
                                            data.root.WRNL.ForcedPatterns.SrnTone2.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.SrnTone2.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.SrnTone2.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.SrnTone2.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.SrnTone2.Pattern);
                                            }
                                            break;
                                        case "SrnTone3":
                                            data.root.WRNL.ForcedPatterns.SrnTone3.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.SrnTone3.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.SrnTone3.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.SrnTone3.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.SrnTone3.Pattern);
                                            }
                                            break;
                                        case "SrnTone4":
                                            data.root.WRNL.ForcedPatterns.SrnTone4.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.SrnTone4.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.SrnTone4.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.SrnTone4.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.SrnTone4.Pattern);
                                            }
                                            break;
                                        case "AuxSiren":
                                            data.root.WRNL.ForcedPatterns.AuxSiren.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.AuxSiren.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.AuxSiren.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.AuxSiren.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.AuxSiren.Pattern);
                                            }
                                            break;
                                        case "PanicMde":
                                            data.root.WRNL.ForcedPatterns.PanicMde.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.PanicMde.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.PanicMde.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.PanicMde.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.PanicMde.Pattern);
                                            }
                                            break;
                                        case "OutOfVeh":
                                            data.root.WRNL.ForcedPatterns.OutOfVeh.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.WRNL.ForcedPatterns.OutOfVeh.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.WRNL.ForcedPatterns.OutOfVeh.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.WRNL.ForcedPatterns.OutOfVeh.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.OutOfVeh.Pattern);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "ScanPatternCustomPool":
                                data.root.WRNL.ScanPatternCustomPool.Enabled = bool.Parse(n.GetAttribute("Enabled").Value);
                                data.root.WRNL.ScanPatternCustomPool.Sequential = bool.Parse(n.GetAttribute("Sequential").Value);
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    data.root.WRNL.ScanPatternCustomPool.Pattern.Add(int.Parse(sn.Value));
                                }
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Warning lights for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }

                #endregion

                #region Primary Lights

                Utils.DebugWriteLine("Parsing Primary Lights");
                try
                {
                    data.root.PRML.LightingFormat = subNodes["PRML"].GetAttribute("LightingFormat").Value;
                    if (subNodes["PRML"].GetAttribute("DisableAtLstg3") != null)
                    {
                        data.root.PRML.DisableAtLstg3 = bool.Parse(subNodes["PRML"].GetAttribute("DisableAtLstg3").Value);
                    }
                    data.root.PRML.ExtrasActiveAtLstg1 = subNodes["PRML"].GetAttribute("ExtrasActiveAtLstg1")?.Value;
                    data.root.PRML.ExtrasActiveAtLstg2 = subNodes["PRML"].GetAttribute("ExtrasActiveAtLstg2")?.Value;
                    data.root.PRML.ExtrasActiveAtLstg3 = subNodes["PRML"].GetAttribute("ExtrasActiveAtLstg3")?.Value;
                    foreach (NanoXMLNode n in subNodes["PRML"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "PresetPatterns":
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "Lstg1":
                                            data.root.PRML.PresetPatterns.Lstg1.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.PresetPatterns.Lstg1.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.PresetPatterns.Lstg1.IntPattern = int.Parse(data.root.PRML.PresetPatterns.Lstg1.Pattern);
                                            }
                                            break;
                                        case "Lstg2":
                                            data.root.PRML.PresetPatterns.Lstg2.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.PresetPatterns.Lstg2.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.PresetPatterns.Lstg2.IntPattern = int.Parse(data.root.PRML.PresetPatterns.Lstg2.Pattern);
                                            }
                                            break;
                                        case "Lstg3":
                                            data.root.PRML.PresetPatterns.Lstg3.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.PresetPatterns.Lstg3.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.PresetPatterns.Lstg3.IntPattern = int.Parse(data.root.PRML.PresetPatterns.Lstg3.Pattern);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "ForcedPatterns":
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "MainHorn":
                                            data.root.PRML.ForcedPatterns.MainHorn.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.MainHorn.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.MainHorn.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.MainHorn.IntPattern = int.Parse(data.root.WRNL.ForcedPatterns.MainHorn.Pattern);
                                            }
                                            break;
                                        case "SrnTone1":
                                            data.root.PRML.ForcedPatterns.SrnTone1.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.SrnTone1.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.SrnTone1.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.SrnTone1.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.SrnTone1.Pattern);
                                            }
                                            break;
                                        case "SrnTone2":
                                            data.root.PRML.ForcedPatterns.SrnTone2.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.SrnTone2.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.SrnTone2.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.SrnTone2.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.SrnTone2.Pattern);
                                            }
                                            break;
                                        case "SrnTone3":
                                            data.root.PRML.ForcedPatterns.SrnTone3.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.SrnTone3.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.SrnTone3.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.SrnTone3.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.SrnTone3.Pattern);
                                            }
                                            break;
                                        case "SrnTone4":
                                            data.root.PRML.ForcedPatterns.SrnTone4.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.SrnTone4.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.SrnTone4.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.SrnTone4.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.SrnTone4.Pattern);
                                            }
                                            break;
                                        case "AuxSiren":
                                            data.root.PRML.ForcedPatterns.AuxSiren.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.AuxSiren.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.AuxSiren.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.AuxSiren.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.MainHorn.Pattern);
                                            }
                                            break;
                                        case "PanicMde":
                                            data.root.PRML.ForcedPatterns.PanicMde.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.PanicMde.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.PanicMde.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.PanicMde.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.PanicMde.Pattern);
                                            }
                                            break;
                                        case "OutOfVeh":
                                            data.root.PRML.ForcedPatterns.OutOfVeh.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.PRML.ForcedPatterns.OutOfVeh.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.PRML.ForcedPatterns.OutOfVeh.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.PRML.ForcedPatterns.OutOfVeh.IntPattern = int.Parse(data.root.PRML.ForcedPatterns.OutOfVeh.Pattern);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "ScanPatternCustomPool":
                                data.root.PRML.ScanPatternCustomPool.Enabled = bool.Parse(n.GetAttribute("Enabled").Value);
                                data.root.PRML.ScanPatternCustomPool.Sequential = bool.Parse(n.GetAttribute("Sequential").Value);
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    data.root.PRML.ScanPatternCustomPool.Pattern.Add(int.Parse(sn.Value));
                                }
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Primary Lights for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }

                #endregion

                #region Secondary Lights
                Utils.DebugWriteLine("Parsing Secondary Lights");
                try
                {
                    data.root.SECL.LightingFormat = subNodes["SECL"].GetAttribute("LightingFormat").Value;
                    if (subNodes["SECL"].GetAttribute("DisableAtLstg3") != null)
                    {
                        data.root.SECL.DisableAtLstg3 = bool.Parse(subNodes["SECL"].GetAttribute("DisableAtLstg3").Value);
                    }
                    data.root.SECL.ExtrasActiveAtLstg1 = subNodes["SECL"].GetAttribute("ExtrasActiveAtLstg1")?.Value;
                    data.root.SECL.ExtrasActiveAtLstg2 = subNodes["SECL"].GetAttribute("ExtrasActiveAtLstg2")?.Value;
                    data.root.SECL.ExtrasActiveAtLstg3 = subNodes["SECL"].GetAttribute("ExtrasActiveAtLstg3")?.Value;
                    foreach (NanoXMLNode n in subNodes["SECL"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "PresetPatterns":
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "Lstg1":
                                            data.root.SECL.PresetPatterns.Lstg1.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.PresetPatterns.Lstg1.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.PresetPatterns.Lstg1.IntPattern = int.Parse(data.root.SECL.PresetPatterns.Lstg1.Pattern);
                                            }
                                            break;
                                        case "Lstg2":
                                            data.root.SECL.PresetPatterns.Lstg2.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.PresetPatterns.Lstg2.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.PresetPatterns.Lstg2.IntPattern = int.Parse(data.root.SECL.PresetPatterns.Lstg2.Pattern);
                                            }
                                            break;
                                        case "Lstg3":
                                            data.root.SECL.PresetPatterns.Lstg3.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.PresetPatterns.Lstg3.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.PresetPatterns.Lstg3.IntPattern = int.Parse(data.root.SECL.PresetPatterns.Lstg3.Pattern);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "ForcedPatterns":
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    switch (sn.Name)
                                    {
                                        case "MainHorn":
                                            data.root.SECL.ForcedPatterns.MainHorn.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.MainHorn.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.MainHorn.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.MainHorn.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.MainHorn.Pattern);
                                            }
                                            break;
                                        case "SrnTone1":
                                            data.root.SECL.ForcedPatterns.SrnTone1.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.SrnTone1.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.SrnTone1.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.SrnTone1.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.SrnTone1.Pattern);
                                            }
                                            break;
                                        case "SrnTone2":
                                            data.root.SECL.ForcedPatterns.SrnTone2.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.SrnTone2.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.SrnTone2.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.SrnTone2.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.SrnTone2.Pattern);
                                            }
                                            break;
                                        case "SrnTone3":
                                            data.root.SECL.ForcedPatterns.SrnTone3.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.SrnTone3.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.SrnTone3.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.SrnTone3.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.SrnTone3.Pattern);
                                            }
                                            break;
                                        case "SrnTone4":
                                            data.root.SECL.ForcedPatterns.SrnTone4.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.SrnTone4.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.SrnTone4.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.SrnTone4.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.SrnTone4.Pattern);
                                            }
                                            break;
                                        case "AuxSiren":
                                            data.root.SECL.ForcedPatterns.AuxSiren.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.AuxSiren.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.AuxSiren.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.AuxSiren.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.AuxSiren.Pattern);
                                            }
                                            break;
                                        case "PanicMde":
                                            data.root.SECL.ForcedPatterns.PanicMde.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.PanicMde.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.PanicMde.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.PanicMde.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.PanicMde.Pattern);
                                            }
                                            break;
                                        case "OutOfVeh":
                                            data.root.SECL.ForcedPatterns.OutOfVeh.Enabled = bool.Parse(sn.GetAttribute("Enabled").Value);
                                            data.root.SECL.ForcedPatterns.OutOfVeh.Pattern = sn.GetAttribute("Pattern").Value;
                                            if (!data.root.SECL.ForcedPatterns.OutOfVeh.Pattern.ToLower().Equals("scan"))
                                            {
                                                data.root.SECL.ForcedPatterns.OutOfVeh.IntPattern = int.Parse(data.root.SECL.ForcedPatterns.OutOfVeh.Pattern);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "ScanPatternCustomPool":
                                data.root.SECL.ScanPatternCustomPool.Enabled = bool.Parse(n.GetAttribute("Enabled").Value);
                                data.root.SECL.ScanPatternCustomPool.Sequential = bool.Parse(n.GetAttribute("Sequential").Value);
                                foreach (NanoXMLNode sn in n.SubNodes)
                                {
                                    data.root.SECL.ScanPatternCustomPool.Pattern.Add(int.Parse(sn.Value));
                                }
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.ReleaseWriteLine($"Secondary Lights for {name} failed to parse due to {e.Message} with inner of {e.InnerException}");
                }
                #endregion

                //TODO: add method to remove old file or a file from ELSVehicle
                if (ELSVehicle.ContainsKey(hash))
                {
                    Utils.DebugWriteLine($"Removeing preexisting VCF for resource {ResourceName} for {name}");
                    ELSVehicle.Remove(hash);
                }
                ELSVehicle.Add(hash, data);
                Utils.ReleaseWriteLine($"Added vehicle {data.filename}");
            }
        }

        internal static void unload(string hash)
        {
            var count = ELSVehicle.Count;
            ELSVehicle.Clear();
            Utils.DebugWriteLine($"Unloaded {count} VCF for {hash}");
        }

        internal static void ParsePatterns(List<dynamic> patterns)
        {
            foreach (dynamic patt in patterns)
            {
                LoadCustomPattern(patt);
            }
        }

        static void LoadCustomPattern(dynamic pattData)
        {
            Light.Patterns.CustomPattern pattern = new Light.Patterns.CustomPattern(150, 200, 200, new Dictionary<int, string>());
            NanoXMLDocument doc = new NanoXMLDocument(pattData.Item3);
            List<StringBuilder> builders = new List<StringBuilder>();
            for (int i = 0; i < 10; i++)
            {
                builders.Add(new StringBuilder());
            }
            foreach (NanoXMLNode n in doc.RootNode.SubNodes)
            {
                switch (n.Name)
                {
                    case "PRIMARY":
                        pattern.PrmDelay = int.Parse(n.GetAttribute("speed").Value);
                        foreach (NanoXMLNode state in n.SubNodes)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (state.GetAttribute($"Extra{i + 1}") != null)
                                {
                                    builders[i].Append(bool.Parse(state.GetAttribute($"Extra{i + 1}").Value) ? "1" : "0");
                                }
                            }
                        }
                        break;
                    case "SECONDARY":
                        pattern.SecDelay = int.Parse(n.GetAttribute("speed").Value);
                        foreach (NanoXMLNode state in n.SubNodes)
                        {
                            for (int i = 5; i < 9; i++)
                            {
                                if (state.GetAttribute($"Extra{i + 1}") != null)
                                {
                                    builders[i].Append(bool.Parse(state.GetAttribute($"Extra{i + 1}").Value) ? "1" : "0");
                                }
                            }
                        }
                        break;
                    case "ADVISOR":
                        pattern.TADelay = int.Parse(n.GetAttribute("speed").Value);
                        foreach (NanoXMLNode state in n.SubNodes)
                        {
                            for (int i = 7; i < 9; i++)
                            {
                                if (state.GetAttribute($"Extra{i + 1}") != null)
                                {
                                    builders[i].Append(bool.Parse(state.GetAttribute($"Extra{i + 1}").Value) ? "1" : "0");
                                }
                            }
                        }
                        break;
                }
            }
            if (builders[0].Length > 0)
            {
                pattern.PatternData.Add(1, builders[0].ToString());
            }
            if (builders[1].Length > 0)
            {
                pattern.PatternData.Add(2, builders[1].ToString());
            }
            if (builders[2].Length > 0)
            {
                pattern.PatternData.Add(3, builders[2].ToString());
            }
            if (builders[3].Length > 0)
            {
                pattern.PatternData.Add(4, builders[3].ToString());
            }
            if (builders[4].Length > 0)
            {
                pattern.PatternData.Add(5, builders[4].ToString());
            }
            if (builders[5].Length > 0)
            {
                pattern.PatternData.Add(6, builders[5].ToString());
            }
            if (builders[6].Length > 0)
            {
                pattern.PatternData.Add(7, builders[6].ToString());
            }
            if (builders[7].Length > 0)
            {
                pattern.PatternData.Add(8, builders[7].ToString());
            }
            if (builders[8].Length > 0)
            {
                pattern.PatternData.Add(9, builders[8].ToString());
            }
            foreach (int hash in ELSVehicle.Keys)
            {
                if (ELSVehicle[hash].root.CustomPatterns == null)
                {
                    ELSVehicle[hash].root.CustomPatterns = new Dictionary<string, Light.Patterns.CustomPattern>();
                }
                if (ELSVehicle[hash].root.CustomPatterns.ContainsKey(Path.GetFileNameWithoutExtension(pattData.Item2).ToLower()))
                {
                    ELSVehicle[hash].root.CustomPatterns[Path.GetFileNameWithoutExtension(pattData.Item2).ToLower()] = pattern;
                }
                else
                {
                    ELSVehicle[hash].root.CustomPatterns.Add(Path.GetFileNameWithoutExtension(pattData.Item2).ToLower(), pattern);
                }

            }

        }

        internal static bool isValidData(string data)
        {
            if (String.IsNullOrEmpty(data)) return false;
            NanoXMLDocument doc = new NanoXMLDocument(data);
            return doc.RootNode.Name == "vcfroot";
        }
    }
}
namespace ELS.configuration
{

    public class INTERFACE
    {
        public string LstgActivationType { get; set; }
        public string DefaultSirenMode { get; set; }
        public string InfoPanelHeaderColor { get; set; }
        public string InfoPanelButtonLightColor { get; set; }
    }

    public class Extra
    {

        public bool IsElsControlled { get; set; }
        public bool AllowEnvLight { get; set; }
        public string Color { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float OffsetZ { get; set; }

        public Extra()
        {
            IsElsControlled = false;
            AllowEnvLight = false;
        }
    }
    public class EOVERRIDE
    {

        public Extra Extra01 { get; set; }

        public Extra Extra02 { get; set; }

        public Extra Extra03 { get; set; }
        public Extra Extra04 { get; set; }
        public Extra Extra05 { get; set; }
        public Extra Extra06 { get; set; }
        public Extra Extra07 { get; set; }
        public Extra Extra08 { get; set; }
        public Extra Extra09 { get; set; }
        public Extra Extra10 { get; set; }
        public Extra Extra11 { get; set; }
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

    public class Takedowns
    {
        public bool AllowUse { get; set; }
        public bool Mirrored { get; set; }
    }

    public class SceneLights
    {
        public bool AllowUse { get; set; }
        public bool IlluminateSidesOnly { get; set; }
    }

    public class MISC
    {
        public bool VehicleIsSlicktop { get; set; }
        public string ArrowboardType { get; set; }
        public bool UseSteadyBurnLights { get; set; }
        public int DfltSirenLtsActivateAtLstg { get; set; }
        public bool HasLadderControl { get; set; }

        public Takedowns Takedowns { get; set; }

        public SceneLights SceneLights { get; set; }

        public LadderControl LadderControl { get; set; }

        public MISC()
        {
            Takedowns = new Takedowns();
            SceneLights = new SceneLights();
            LadderControl = new LadderControl();
        }
    }

    public class LadderControl
    {
        public string VerticalControl { get; set; }
        public string HorizontalControl { get; set; }
        public int MovementSpeed { get; set; }
    }

    public class UseExtras
    {

        public bool Extra1 { get; set; }

        public bool Extra2 { get; set; }

        public bool Extra3 { get; set; }

        public bool Extra4 { get; set; }

        public UseExtras()
        {
            Extra1 = true;
            Extra2 = true;
            Extra3 = true;
            Extra4 = true;
        }
    }


    public class CRUISE
    {

        public bool DisableAtLstg3 { get; set; }

        public UseExtras UseExtras { get; set; }

        public CRUISE()
        {
            UseExtras = new UseExtras();
            DisableAtLstg3 = false;
        }
    }


    public class AcoronaLights
    {

        public int DfltPattern { get; set; }

        public string ColorL { get; set; }

        public string ColorR { get; set; }
    }



    public class ACORONAS
    {

        public AcoronaLights Headlights { get; set; }

        public AcoronaLights TailLights { get; set; }

        public AcoronaLights IndicatorsF { get; set; }

        public AcoronaLights IndicatorsB { get; set; }

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


    public class MainHorn
    {

        public bool InterruptsSiren { get; set; }

        public string AudioString { get; set; }

        public string SoundBank { get; set; }

        public string SoundSet { get; set; }
    }

    public class SrnTone
    {

        public bool AllowUse { get; set; }

        public string AudioString { get; set; }

        public string SoundBank { get; set; }

        public string SoundSet { get; set; }
    }

    public class TonePattern
    {
        public bool Enabled { get; set; }
        public string Pattern { get; set; }
        public int IntPattern { get; set; }

        public TonePattern()
        {
            Enabled = false;
            IntPattern = -1;
        }
    }


    public class SOUNDS
    {

        public MainHorn MainHorn { get; set; }

        public SrnTone ManTone1 { get; set; }

        public SrnTone ManTone2 { get; set; }

        public SrnTone SrnTone1 { get; set; }

        public SrnTone SrnTone2 { get; set; }

        public SrnTone SrnTone3 { get; set; }

        public SrnTone SrnTone4 { get; set; }

        public SrnTone AuxSiren { get; set; }

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


    public class PresetPatterns
    {

        public Lstg Lstg3 { get; set; }

        public Lstg Lstg2 { get; set; }

        public Lstg Lstg1 { get; set; }

        public PresetPatterns()
        {
            Lstg1 = new Lstg();
            Lstg2 = new Lstg();
            Lstg3 = new Lstg();
        }
    }


    public class ForcedPatterns
    {

        public TonePattern MainHorn { get; set; }

        public TonePattern SrnTone1 { get; set; }

        public TonePattern SrnTone2 { get; set; }

        public TonePattern SrnTone3 { get; set; }

        public TonePattern SrnTone4 { get; set; }

        public TonePattern PanicMde { get; set; }

        public TonePattern AuxSiren { get; set; }


        public TonePattern OutOfVeh { get; set; }

        public ForcedPatterns()
        {
            MainHorn = new TonePattern();
            SrnTone1 = new TonePattern();
            SrnTone2 = new TonePattern();
            SrnTone3 = new TonePattern();
            SrnTone4 = new TonePattern();
            AuxSiren = new TonePattern();
            PanicMde = new TonePattern();
            OutOfVeh = new TonePattern();
        }
    }


    public class ScanPatternCustomPool
    {

        public List<int> Pattern { get; set; }

        public bool Enabled { get; set; }

        public bool Sequential { get; set; }

        public ScanPatternCustomPool()
        {
            Pattern = new List<int>();
        }
    }

    public class Lstg
    {

        public bool Enabled { get; set; }

        public int IntPattern { get; set; }
        public string Pattern { get; set; }
    }

    public class Lights
    {

        public PresetPatterns PresetPatterns { get; set; }

        public ForcedPatterns ForcedPatterns { get; set; }

        public ScanPatternCustomPool ScanPatternCustomPool { get; set; }

        public string LightingFormat { get; set; }

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


    public class Vcfroot
    {

        public INTERFACE INTERFACE { get; set; }

        public EOVERRIDE EOVERRIDE { get; set; }

        public MISC MISC { get; set; }

        public CRUISE CRUISE { get; set; }

        public ACORONAS ACORONAS { get; set; }

        public SOUNDS SOUNDS { get; set; }

        public Lights WRNL { get; set; }

        public Lights PRML { get; set; }

        public Lights SECL { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public Dictionary<string, Light.Patterns.CustomPattern> CustomPatterns { get; set; }

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
