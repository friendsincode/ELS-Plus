using System.Collections.Generic;
using CitizenFX.Core;
using ELS.FullSync;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry, IFullSyncComponent
    {

        private void RequestFullSyncData()
        {
            FullSyncManager.RequestData(_vehicle.GetNetworkId());
        }

        public void SetData(IDictionary<string, object> data)
        {
            this._mainSiren.SetData((IDictionary<string,object>)data["_mainSiren"]);
            this._tones.SetData((IDictionary<string, object>)data["_tones"]);
            this.dual_siren = bool.Parse(data["dual_siren"].ToString());
        }

        public Dictionary<string, object> ToDic()
        {
            var dic = new Dictionary<string, object>
            {
                {"_mainSiren",_mainSiren.ToDic() },
                { "_tones",_tones.ToDic()},
                {"dual_siren",dual_siren.ToString() }
            };
            return dic;
        }

        public void RunSendSync()
        {
            FullSyncManager.SendData(this.GetType().Name, ToDic(), _vehicle.GetNetworkId());
        }
    }
}