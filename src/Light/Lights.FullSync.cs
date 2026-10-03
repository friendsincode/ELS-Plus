using ELS.Board;
using ELS.Extra;
using ELS.FullSync;

using System;
using System.Collections.Generic;

namespace ELS.Light
{
    public struct LightFSData
    {
        public Dictionary<int, ExtraFSData> PRM { get; set; }
        public Dictionary<int, ExtraFSData> SEC { get; set; }
        public Dictionary<int, ExtraFSData> WRN { get; set; }
        public ExtraFSData SCL { get; set; }
        public ExtraFSData TKDN { get; set; }
        public ExtraFSData STDYBRN { get; set; }
        public SpotLightFSData Spotlight { get; set; }
        public int Stage { get; set; }
        public int PrmPattern { get; set; }
        public int SecPattern { get; set; }
        public int WrnPattern { get; set; }
        public ArrowBoardFSData BRD { get; set; }
        public SceneFSData Scene { get; set; }
    }
    partial class Lights : IFullSyncComponent<LightFSData>
    {
        public LightFSData GetData()
        {
            return new LightFSData
            {
                PRM = GetExtras(_extras.PRML),
                SEC = GetExtras(_extras.SECL),
                WRN = GetExtras(_extras.WRNL),
                STDYBRN = _extras.SBRN?.GetData() ?? default(ExtraFSData),
                SCL = _extras.SCL?.GetData() ?? default(ExtraFSData),
                TKDN = _extras.TDL?.GetData() ?? default(ExtraFSData),
                BRD = _extras.BRD?.GetData() ?? default(ArrowBoardFSData),
                Scene = scene?.GetData() ?? default(SceneFSData),
                Spotlight = spotLight?.GetData() ?? default(SpotLightFSData),
                Stage = _stage.CurrentStage,
                PrmPattern = CurrentPrmPattern,
                SecPattern = CurrentSecPattern,
                WrnPattern = CurrentWrnPattern
            };
        }

        private static Dictionary<int, ExtraFSData> GetExtras(Dictionary<int, Extra.Extra> extras)
        {
            var result = new Dictionary<int, ExtraFSData>();
            foreach (var entry in extras) result[entry.Key] = entry.Value.GetData();
            return result;
        }

        private static void ApplyExtras(Dictionary<int, Extra.Extra> extras, Dictionary<int, ExtraFSData> data)
        {
            if (data == null) return;
            foreach (var entry in extras)
            {
                ExtraFSData state;
                if (data.TryGetValue(entry.Key, out state)) entry.Value.SetData(state);
            }
        }

        public void SetData(LightFSData data)
        {
            _stage.SetStage(data.Stage);
            CurrentPrmPattern = data.PrmPattern;
            CurrentSecPattern = data.SecPattern;
            CurrentWrnPattern = data.WrnPattern;
            ApplyExtras(_extras.PRML, data.PRM);
            ApplyExtras(_extras.SECL, data.SEC);
            ApplyExtras(_extras.WRNL, data.WRN);
            _extras.SBRN?.SetData(data.STDYBRN);
            _extras.SCL?.SetData(data.SCL);
            _extras.TDL?.SetData(data.TKDN);
            _extras.BRD?.SetData(data.BRD);
            spotLight?.SetData(data.Spotlight);
            scene?.SetData(data.Scene);
        }
    }
}
