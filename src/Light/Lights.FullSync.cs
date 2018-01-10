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
            }
            var sec = new Dictionary<int, object>();
            foreach (Extra.Extra e in _extras.SECL.Values)
            {
                sec.Add(e.Id, e.GetData());
            }
            var wrn = new Dictionary<int, object>();
            foreach (Extra.Extra e in _extras.WRNL.Values)
            {
                wrn.Add(e.Id, e.GetData());
            }
            dic.Add("PRML", prm);
            dic.Add("SECL", sec);
            dic.Add("WRNL", wrn);
            if (_extras.SBRN != null)
            {
                dic.Add("SBRN", _extras.SBRN.GetData());
            }
            if (_extras.SCL != null)
            {
                dic.Add("SCL", _extras.SCL.GetData());
            }
            if (_extras.TDL != null)
            {
                dic.Add("TDL", _extras.TDL.GetData());
            }
            dic.Add("BRD", _extras.BRD.GetData());
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
#if DEBUG
            
#endif
            if (data.ContainsKey("PRML"))
            {
                IDictionary<string, object> prm = (IDictionary<string, object>)data["PRML"];
                foreach (Extra.Extra e in _extras.PRML.Values)
                {
                    e.SetData((IDictionary<string,object>) prm[$"{e.Id}"]);
                }
            }
            if (data.ContainsKey("SECL"))
            {
                IDictionary<string, object> sec = (IDictionary<string, object>)data["SECL"];
                foreach (Extra.Extra e in _extras.SECL.Values)
                {

                    e.SetData((IDictionary<string, object>)sec[$"{e.Id}"]);
                }
            }
            if (data.ContainsKey("WRNL"))
            {
                IDictionary<string, object> wrn = (IDictionary<string, object>)data["WRNL"];
                foreach (Extra.Extra e in _extras.WRNL.Values)
                {

                    e.SetData((IDictionary<string, object>)wrn[$"{e.Id}"]);
                }
            }
            if (data.ContainsKey("SBRN"))
            {
                _extras.SBRN.SetData((IDictionary<string, object>)data["SBRN"]);
            }
            if (data.ContainsKey("SCL"))
            {
                _extras.SBRN.SetData((IDictionary<string, object>)data["SCL"]);
            }
            if (data.ContainsKey("TDL"))
            {
                _extras.SBRN.SetData((IDictionary<string, object>)data["TDL"]);
            }
            if (data.ContainsKey("BRD"))
            {
                _extras.BRD.SetData((IDictionary<string, object>)data["BRD"]);
            }
        }
    }
}
