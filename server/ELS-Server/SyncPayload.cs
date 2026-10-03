using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace ELS_Server
{
    // Validate the wire format before storing or forwarding client-controlled JSON.
    internal static class SyncPayload
    {
        internal static bool TryParse(string json, out JObject data)
        {
            data = null;
            if (String.IsNullOrEmpty(json) || json.Length > 32768) return false;
            try
            {
                using (var reader = new JsonTextReader(new StringReader(json)) { MaxDepth = 16 })
                {
                    data = JObject.Load(reader);
                    if (reader.Read()) return false;
                }
                var light = data["Light"] as JObject;
                var siren = data["Siren"] as JObject;
                return Fields(data, "Id", "NetworkId", "Light", "Siren") &&
                    Integer(data["Id"], 1, int.MaxValue) && Integer(data["NetworkId"], 1, 65535) &&
                    Fields(light, "PRM", "SEC", "WRN", "SCL", "TKDN", "STDYBRN", "Spotlight", "Stage", "PrmPattern", "SecPattern", "WrnPattern", "BRD", "Scene") &&
                    Extras(light["PRM"]) && Extras(light["SEC"]) && Extras(light["WRN"]) &&
                    Extra(light["SCL"]) && Extra(light["TKDN"]) && Extra(light["STDYBRN"]) &&
                    Integer(light["Stage"], 0, 3) && Integer(light["PrmPattern"], 0, 4096) &&
                    Integer(light["SecPattern"], 0, 4096) && Integer(light["WrnPattern"], 0, 4096) &&
                    Flags(light["BRD"], "Raised") && Flags(light["Scene"], "TurnedOn") &&
                    Spotlight(light["Spotlight"]) && Fields(siren, "Dual", "MainSiren", "Tones") &&
                    Boolean(siren["Dual"]) && MainSiren(siren["MainSiren"]) &&
                    Flags(siren["Tones"], "Horn", "Tone1", "Tone2", "Tone3", "Tone4", "Panic");
            }
            catch (JsonException) { return false; }
            catch (OverflowException) { return false; }
        }

        private static bool Fields(JObject obj, params string[] names)
        {
            return obj != null && obj.Count == names.Length && names.All(name => obj[name] != null);
        }

        private static bool Boolean(JToken value) => value != null && value.Type == JTokenType.Boolean;
        private static bool Integer(JToken value, int min, int max)
        {
            return value != null && value.Type == JTokenType.Integer && (long)value >= min && (long)value <= max;
        }

        private static bool Flags(JToken value, params string[] names)
        {
            var obj = value as JObject;
            return Fields(obj, names) && names.All(name => Boolean(obj[name]));
        }

        private static bool Extra(JToken value)
        {
            var obj = value as JObject;
            return Fields(obj, "PatternRunning", "TurnedOn", "Pattern") &&
                Boolean(obj["PatternRunning"]) && Boolean(obj["TurnedOn"]) && Integer(obj["Pattern"], 0, 4096);
        }

        private static bool Extras(JToken value)
        {
            if (value != null && value.Type == JTokenType.Null) return true;
            var obj = value as JObject;
            return obj != null && obj.Count <= 12 && obj.Properties().All(p =>
            {
                int id;
                return int.TryParse(p.Name, out id) && id >= 1 && id <= 12 && Extra(p.Value);
            });
        }

        private static bool MainSiren(JToken value)
        {
            var obj = value as JObject;
            return Fields(obj, "Interrupted", "CurrentTone", "State") && Boolean(obj["Interrupted"]) &&
                Boolean(obj["State"]) && Integer(obj["CurrentTone"], 0, 3);
        }

        private static bool Spotlight(JToken value)
        {
            var obj = value as JObject;
            return Fields(obj, "Horizontal", "Vertical", "TurnedOn") && Boolean(obj["TurnedOn"]) &&
                Angle(obj["Horizontal"]) && Angle(obj["Vertical"]);
        }

        private static bool Angle(JToken value)
        {
            if (value == null || (value.Type != JTokenType.Float && value.Type != JTokenType.Integer)) return false;
            double number = (double)value;
            return !Double.IsNaN(number) && !Double.IsInfinity(number) && Math.Abs(number) <= 360;
        }
    }
}
