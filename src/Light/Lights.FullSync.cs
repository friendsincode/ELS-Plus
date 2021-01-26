using ELS.Board;
using ELS.Extra;
using ELS.FullSync;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //JObject obj = new JObject();
            LightFSData data = new LightFSData();
            Dictionary<int, ExtraFSData> prm = new Dictionary<int, ExtraFSData>();
            foreach (Extra.Extra e in _extras.PRML.Values)
            {
                prm.Add(e.Id, e.GetData());

                Utils.DebugWriteLine($"Added {e.Id} to prml sync data");

            }
            Dictionary<int, ExtraFSData> sec = new Dictionary<int, ExtraFSData>();
            foreach (Extra.Extra e in _extras.SECL.Values)
            {
                sec.Add(e.Id, e.GetData());

                Utils.DebugWriteLine($"Added {e.Id} to secl sync data");

            }
            Dictionary<int, ExtraFSData> wrn = new Dictionary<int, ExtraFSData>();
            foreach (Extra.Extra e in _extras.WRNL.Values)
            {
                wrn.Add(e.Id, e.GetData());

                Utils.DebugWriteLine($"Added {e.Id} to wrnl sync data");

            }

            if (prm != null && prm.Count > 0)
            {
                data.PRM = prm;

                Utils.DebugWriteLine($"added PRML data");

            }
            if (sec != null && sec.Count > 0)
            {
                data.SEC = sec;

                Utils.DebugWriteLine($"added secl data");

            }
            if (wrn != null && wrn.Count > 0)
            {
                data.WRN = wrn;

                Utils.DebugWriteLine($"added wrnl data");

            }
            if (_extras.SBRN != null)
            {
                data.STDYBRN = _extras.SBRN.GetData();

                Utils.DebugWriteLine($"added SBRN data");

            }
            if (_extras.SCL != null)
            {
               data.SCL =  _extras.SCL.GetData();

                Utils.DebugWriteLine($"added SCL data");

            }
            if (_extras.TDL != null)
            {
                data.TKDN = _extras.TDL.GetData();

                Utils.DebugWriteLine($"added TDL data");

            }
            data.Stage = _stage.CurrentStage;
            data.BRD = _extras.BRD.GetData();
            data.PrmPattern = CurrentPrmPattern;
            data.SecPattern = CurrentSecPattern;
            data.WrnPattern = CurrentWrnPattern;
            data.Scene = scene.GetData();
            //dic.Add("BRD", _extras.BRD.GetData());
            //dic.Add("PrmPatt", CurrentPrmPattern);
            //dic.Add("SecPatt", CurrentSecPattern);
            //dic.Add("WrnPatt", CurrentWrnPattern);
            //dic.Add("stage", _stage.CurrentStage);
            if (spotLight != null)
            {
                data.Spotlight = spotLight.GetData();
            }
            if (scene != null)
            {
               data.Scene = scene.GetData();
            }
            //string dic = JSON.Serialize(new
            //{
            //    PRML = prm,
            //    SECL = sec,
            //    WRNL = wrn,
            //    SBRN = _extras.SBRN?.GetData(),
            //    SCL = _extras.SCL?.GetData(),
            //    TDL = _extras.TDL?.GetData(),
            //    BRD = _extras.BRD?.GetData(),
            //    PrmPatt = CurrentPrmPattern,
            //    SecPatt = CurrentSecPattern,
            //    WrnPatt = CurrentWrnPattern,
            //    stage = _stage.CurrentStage,
            //    spotLight = spotLight?.GetData(),
            //    scene = scene?.GetData()
            //}) ;
            return data;
        }

        public void SetData(LightFSData data)
        {
            if (data.Equals(null))
            {
                return;
            }
            if (data.PRM != null && data.PRM.Count > 0)
            {

                Utils.DebugWriteLine($"Got PRML data");

                //Dictionary<string, object> prm = (Dictionary<string, object>)data["PRML"];
                foreach (Extra.Extra e in _extras.PRML.Values)
                {
                    e.SetData(data.PRM[e.Id]);

                    Utils.DebugWriteLine($"Added {e.Id} from prml sync data");

                }
            }
            if (data.WRN != null && data.SEC.Count > 0)
            {

                Utils.DebugWriteLine($"Got SECL DAta");

                //Dictionary<string, object> sec = (Dictionary<string, object>)data["SECL"];
                foreach (Extra.Extra e in _extras.SECL.Values)
                {
                    e.SetData(data.SEC[e.Id]);

                    Utils.DebugWriteLine($"Added {e.Id} from secl sync data");

                }
            }
            if (data.WRN != null && data.WRN.Count > 0)
            {

                Utils.DebugWriteLine($"Got WRNL data");

                //Dictionary<string, object> wrn = (Dictionary<string, object>)data["WRNL"];
                foreach (Extra.Extra e in _extras.WRNL.Values)
                {
                    e.SetData(data.WRN[e.Id]);

                    Utils.DebugWriteLine($"Added {e.Id} from wrnl sync data");

                }
            }
            try
            {
                if (!data.STDYBRN.Equals(null))
                {
                    _extras.SBRN.SetData(data.STDYBRN);

                    Utils.DebugWriteLine($"Added SBRN from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"SBRN error: {e.Message}");
            }
            try
            {
                if (!data.SCL.Equals(null))
                {
                    _extras.SCL.SetData(data.SCL);

                    Utils.DebugWriteLine($"Added SCL from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"SCL error: {e.Message}");
            }
            try
            {
                if (!data.TKDN.Equals(null))
                {
                    _extras.TDL.SetData(data.TKDN);

                    Utils.DebugWriteLine($"Added TDL from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"TDL error: {e.Message}");
            }
            try
            {
                if (!data.BRD.Equals(null))
                {

                    Utils.DebugWriteLine($"Got BRD from sync data");

                    _extras.BRD.SetData(data.BRD);

                    Utils.DebugWriteLine($"Added BRD from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"BRD error: {e.Message}");
            }

            CurrentPrmPattern = data.PrmPattern;

                Utils.DebugWriteLine($"Added PrmPatt from sync data");


            CurrentSecPattern = data.SecPattern;

                Utils.DebugWriteLine($"Added SecPatt from sync data");


            CurrentWrnPattern = data.WrnPattern;

                Utils.DebugWriteLine($"Added WrnPatt from sync data");

                _stage.SetStage(data.Stage);

             Utils.DebugWriteLine($"Added stage from sync data");

            if (spotLight != null)
            {
                spotLight.SetData(data.Spotlight);
            }
            if (scene != null)
            {
                scene.SetData(data.Scene);
            }
        }

    }
}
