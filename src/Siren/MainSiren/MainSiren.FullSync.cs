using System.Collections.Generic;
using CitizenFX.Core;
using ELS.FullSync;

namespace ELS.Siren
{
    partial class Siren
    {
        private partial class MainSiren
        {
            void IFullSyncComponentSetData(IDictionary<string, object> data)
            {
                SetData(data);
            }

            public Dictionary<string, string> ToDic()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("interupted", this.interupted.ToString());
                dic.Add("currentTone", this.MainTones.IndexOf(currentTone).ToString());
                dic.Add("state", this._enable.ToString());
                return dic;
            }

            public void RequestData()
            {
                // throw new NotImplementedException();
            }

            public void RunSendSync()
            {
                FullSyncManager.SendData(this.GetType().Name, this.ToDic(), Game.Player.ServerId);
            }

            public void SetData(IDictionary<string, object> data)
            {
                currentTone = MainTones[int.Parse(data["currentTone"].ToString())];
                interupted = bool.Parse(data["interupted"].ToString());
                _enable = (bool.Parse(data["state"].ToString()));
            }
        }
    }
}