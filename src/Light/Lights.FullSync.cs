using ELS.FullSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    partial class Lights : IFullSyncComponent
    {
        public Dictionary<string, object> GetData()
        {

            var dic = new Dictionary<string, object>();
            var prm = new Dictionary<int, object>();
            foreach (Extra.Extra e in _extras.PRML.Values)
            {
                prm.Add(e.Id, e.GetData());

                Utils.DebugWriteLine($"Added {e.Id} to prml sync data");

            }
            var sec = new Dictionary<int, object>();
            foreach (Extra.Extra e in _extras.SECL.Values)
            {
                sec.Add(e.Id, e.GetData());

                Utils.DebugWriteLine($"Added {e.Id} to secl sync data");

            }
            var wrn = new Dictionary<int, object>();
            foreach (Extra.Extra e in _extras.WRNL.Values)
            {
                wrn.Add(e.Id, e.GetData());

                Utils.DebugWriteLine($"Added {e.Id} to wrnl sync data");

            }
            if (prm != null && prm.Count > 0)
            {
                dic.Add("PRML", prm);

                Utils.DebugWriteLine($"added PRML data");

            }
            if (sec != null && sec.Count > 0)
            {
                dic.Add("SECL", sec);

                Utils.DebugWriteLine($"added secl data");

            }
            if (wrn != null && wrn.Count > 0)
            {
                dic.Add("WRNL", wrn);

                Utils.DebugWriteLine($"added wrnl data");

            }
            if (_extras.SBRN != null)
            {
                dic.Add("SBRN", _extras.SBRN.GetData());

                Utils.DebugWriteLine($"added SBRN data");

            }
            if (_extras.SCL != null)
            {
                dic.Add("SCL", _extras.SCL.GetData());

                Utils.DebugWriteLine($"added SCL data");

            }
            if (_extras.TDL != null)
            {
                dic.Add("TDL", _extras.TDL.GetData());

                Utils.DebugWriteLine($"added TDL data");

            }
            dic.Add("BRD", _extras.BRD.GetData());
            dic.Add("PrmPatt", CurrentPrmPattern);
            dic.Add("SecPatt", CurrentSecPattern);
            dic.Add("WrnPatt", CurrentWrnPattern);
            dic.Add("stage", _stage.CurrentStage);
            if (spotLight != null)
            {
                dic.Add("spotlight", spotLight.GetData());
            }
            if (scene != null)
            {
                dic.Add("scene", scene.GetData());
            }
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {



            if (data.ContainsKey("PRML"))
            {

                Utils.DebugWriteLine($"Got PRML data");

                IDictionary<string, object> prm = (IDictionary<string, object>)data["PRML"];
                foreach (Extra.Extra e in _extras.PRML.Values)
                {
                    e.SetData((IDictionary<string, object>)prm[$"{e.Id}"]);

                    Utils.DebugWriteLine($"Added {e.Id} from prml sync data");

                }
            }
            if (data.ContainsKey("SECL"))
            {

                Utils.DebugWriteLine($"Got SECL DAta");

                IDictionary<string, object> sec = (IDictionary<string, object>)data["SECL"];
                foreach (Extra.Extra e in _extras.SECL.Values)
                {
                    e.SetData((IDictionary<string, object>)sec[$"{e.Id}"]);

                    Utils.DebugWriteLine($"Added {e.Id} from secl sync data");

                }
            }
            if (data.ContainsKey("WRNL"))
            {

                Utils.DebugWriteLine($"Got WRNL data");

                IDictionary<string, object> wrn = (IDictionary<string, object>)data["WRNL"];
                foreach (Extra.Extra e in _extras.WRNL.Values)
                {
                    e.SetData((IDictionary<string, object>)wrn[$"{e.Id}"]);

                    Utils.DebugWriteLine($"Added {e.Id} from wrnl sync data");

                }
            }
            try
            {
                if (data.ContainsKey("SBRN"))
                {
                    _extras.SBRN.SetData((IDictionary<string, object>)data["SBRN"]);

                    Utils.DebugWriteLine($"Added SBRN from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"SBRN error: {e.Message}");
            }
            try
            {
                if (data.ContainsKey("SCL"))
                {
                    _extras.SCL.SetData((IDictionary<string, object>)data["SCL"]);

                    Utils.DebugWriteLine($"Added SCL from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"SCL error: {e.Message}");
            }
            try
            {
                if (data.ContainsKey("TDL"))
                {
                    _extras.TDL.SetData((IDictionary<string, object>)data["TDL"]);

                    Utils.DebugWriteLine($"Added TDL from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"TDL error: {e.Message}");
            }
            try
            {
                if (data.ContainsKey("BRD"))
                {

                    Utils.DebugWriteLine($"Got BRD from sync data");

                    _extras.BRD.SetData((IDictionary<string, object>)data["BRD"]);

                    Utils.DebugWriteLine($"Added BRD from sync data");

                }
            }
            catch (Exception e)
            {
                Utils.DebugWriteLine($"BRD error: {e.Message}");
            }
            if (data.ContainsKey("PrmPatt"))
            {
                CurrentPrmPattern = int.Parse(data["PrmPatt"].ToString());

                Utils.DebugWriteLine($"Added PrmPatt from sync data");

            }
            if (data.ContainsKey("SecPatt"))
            {
                CurrentSecPattern = int.Parse(data["SecPatt"].ToString());

                Utils.DebugWriteLine($"Added SecPatt from sync data");

            }
            if (data.ContainsKey("WrnPatt"))
            {
                CurrentWrnPattern = int.Parse(data["WrnPatt"].ToString());

                Utils.DebugWriteLine($"Added WrnPatt from sync data");

            }
            if (data.ContainsKey("stage"))
            {
                _stage.SetStage(int.Parse(data["stage"].ToString()));

                Utils.DebugWriteLine($"Added stage from sync data");

            }
            if (spotLight != null)
            {
                spotLight.SetData((IDictionary<string, object>)data["spotlight"]);
            }
            if (scene != null)
            {
                scene.SetData((IDictionary<string, object>)data["scene"]);
            }
        }
    }
}
