using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.configuration
{
    class SoundConfig
    {
        public static Sounds SoundBindings = new Sounds();
        public SoundConfig()
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
        }

        private void FileLoader_OnSettingsLoaded(SettingsType.Type type, string Data)
        {
            if (type == SettingsType.Type.VCF)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(Data);
                SoundBindings.MainHorn = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["AudioString"].Value;
                SoundBindings.SrnTone1 = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AudioString"].Value;
                SoundBindings.SrnTone2 = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AudioString"].Value;
                SoundBindings.SrnTone3 = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AudioString"].Value;
                SoundBindings.SrnTone4 = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AudioString"].Value;
            }
        }

        public class Sounds
        {
            public string SrnTone1 { get; internal set; }
            public string SrnTone2 { get; internal set; }
            public string SrnTone3 { get; internal set; }
            public string SrnTone4 { get; internal set; }
            public string MainHorn { get; internal set; }
        }
    }
}
