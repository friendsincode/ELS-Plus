using System.Collections.Generic;
using CitizenFX.Core;
using ELS.FullSync;

namespace ELS.Siren
{
    partial class Siren
    {
        internal partial class MainSiren
        {


            public Dictionary<string, object> GetData()
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("interupted", this.interupted.ToString());
                dic.Add("currentTone", currentTone.ToString());
                dic.Add("state", this._enable.ToString());
                return dic;
            }

            public void SetData(IDictionary<string, object> data)
            {
                currentTone = int.Parse(data["currentTone"].ToString());
                interupted = bool.Parse(data["interupted"].ToString());
                _enable = (bool.Parse(data["state"].ToString()));
            }
        }
    }
}