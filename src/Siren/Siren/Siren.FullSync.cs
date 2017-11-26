using System.Collections.Generic;
using CitizenFX.Core;
using ELS.FullSync;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry, IFullSyncComponent
    {
        internal void FullSendSync()
        {
            this._mainSiren.RunSendSync();
            this._tones.RunSendSync();
        }

        internal void SetFullSync(string dataType, IDictionary<string, object> dataDic)
        {
            switch (dataType)
            {
                case "MainSiren":
                    _mainSiren.SetData(dataDic);
                    break;
                case "Tones":
                    _tones.SetData(dataDic);
                    break;
                case "Siren":
                    this.SetData(dataDic);
                    break;
            }
        }

        private void RequestFullSyncData()
        {
            _mainSiren.RequestData();
            _tones.RequestData();
            this.RequestData();
        }

        public void SetData(IDictionary<string, object> data)
        {
            this.dual_siren = bool.Parse(data["dual_siren"].ToString());
        }

        public Dictionary<string, string> ToDic()
        {
            var dic = new Dictionary<string, string>
            {
                {"dual_siren",dual_siren.ToString() }
            };
            return dic;
        }

        public void RequestData()
        {
            throw new System.NotImplementedException();
        }

        public void RunSendSync()
        {
            FullSyncManager.SendData(this.GetType().Name, ToDic(), Game.Player.ServerId);
        }
    }
}