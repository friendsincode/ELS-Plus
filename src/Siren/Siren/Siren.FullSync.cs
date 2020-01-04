using System.Collections.Generic;
using CitizenFX.Core;
using ELS.FullSync;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry, IFullSyncComponent
    {
        public void SetData(IDictionary<string, object> data)
        {
            Utils.DebugWriteLine("Setting data for sirens");
            if (data.ContainsKey("_mainSiren"))
            {
                Utils.DebugWriteLine("Got Main Siren Data");
                _mainSiren.SetData((IDictionary<string, object>)data["_mainSiren"]);
            }
            if (data.ContainsKey("_tones")) {
                Utils.DebugWriteLine("Got Tone Data");
                _tones.SetData((IDictionary<string,object>)data["_tones"]);
            }
            if(data.TryGetValue("dual_siren", out object res))
            {
                Utils.DebugWriteLine("Got Dual Siren Data");
                dual_siren = (bool)res;
            }
        }

        public Dictionary<string, object> GetData()
        {
            var dic = new Dictionary<string, object>
            {
                {"_mainSiren",_mainSiren.GetData() },
                { "_tones",_tones.GetData()},
                {"dual_siren",dual_siren }
            };
            return dic;
        }
    }
}