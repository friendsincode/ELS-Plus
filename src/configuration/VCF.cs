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
        internal static void load(SettingsType.Type type, string name, string Data,string ResourceName)
        {
            var bytes = Encoding.UTF8.GetBytes(Data);
            if (bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                var ex = new Exception($"Error Loading:{name}\n" +
                                    $"Please save {name} with UTF-8 no BOM/Signature Encoding");
                throw (ex);
            }
            Encoding.UTF8.GetPreamble();
            var data = new VCFEntry(Path.GetFileNameWithoutExtension(name), ResourceName, Game.GenerateHash(Path.GetFileNameWithoutExtension(name)),new Vcfroot());
            if (type == SettingsType.Type.VCF)
            {
                CitizenFX.Core.Debug.WriteLine("Loading XML");
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(Data);
                bool res;
                //data.filename = Path.GetFileNameWithoutExtension(name);
                if (data.root == null)
                {
                    CitizenFX.Core.Debug.WriteLine("Null issue");
                    return;
                }
                data.root.SOUNDS.ManTone1.AudioString = doc?["vcfroot"]?["SOUNDS"]?["ManTone1"]?.Attributes["AudioString"]?.Value;
                data.root.SOUNDS.ManTone1.AllowUse = doc["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AllowUse"].Value;
                
                data.root.SOUNDS.ManTone2.AudioString = doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AudioString"].Value;
                data.root.SOUNDS.ManTone2.AllowUse = doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AllowUse"].Value;
                

                data.root.SOUNDS.MainHorn.AudioString = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["AudioString"].Value;
                data.root.SOUNDS.MainHorn.InterruptsSiren = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["InterruptsSiren"].Value;
                
                data.root.SOUNDS.SrnTone1.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone1.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AllowUse"].Value;
                
                data.root.SOUNDS.SrnTone2.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone2.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AllowUse"].Value;
                
                data.root.SOUNDS.SrnTone3.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone3.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AllowUse"].Value;
                
                data.root.SOUNDS.SrnTone4.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AudioString"].Value;
                data.root.SOUNDS.SrnTone4.AllowUse = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AllowUse"].Value;
                
                data.root.SOUNDS.AuxSiren.AllowUse = doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AllowUse"].Value;
                data.root.SOUNDS.AuxSiren.AudioString = doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AudioString"].Value;
                
                data.root.SOUNDS.PanicMde.AllowUse = doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AllowUse"].Value;
                data.root.SOUNDS.PanicMde.AudioString = doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AudioString"].Value;
                
                data.root.Author = doc["vcfroot"].Attributes["Author"].Value;
                //TODO: add method to remove old file or a file from ELSVehicle
                if (ELSVehicle.Exists(veh => veh.resource == ResourceName))
                {
                    CitizenFX.Core.Debug.WriteLine($"Removeing preexisting VCF for resource ${ResourceName}");
                    ELSVehicle.RemoveAll(veh => veh.resource == ResourceName);
                }
                ELSVehicle.Add(data);
                CitizenFX.Core.Debug.WriteLine($"Added {data.filename}");
            }
        }
        internal static void unload(string ResourceName)
        {
            var count = ELSVehicle.RemoveAll(veh => veh.resource.Equals(ResourceName));
            CitizenFX.Core.Debug.WriteLine($"Unloaded {count} VCF for {ResourceName}");
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
            Extra12 = new Extra();        }
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
        public string Extra1 { get; set; }
        [XmlAttribute(AttributeName = "Extra2")]
        public string Extra2 { get; set; }
        [XmlAttribute(AttributeName = "Extra3")]
        public string Extra3 { get; set; }
        [XmlAttribute(AttributeName = "Extra4")]
        public string Extra4 { get; set; }
    }

    [XmlRoot(ElementName = "CRUISE")]
    public class CRUISE
    {
        [XmlElement(ElementName = "DisableAtLstg3")]
        public string DisableAtLstg3 { get; set; }
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
        public string DfltPattern { get; set; }
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
