using System.Collections.Generic;

namespace ELS.Siren
{
    partial class Siren
    {
        internal void FullSync()
        {
            this._mainSiren.FullSync();
            this._tones.RunSync();
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
            }
        }

        private void RequestFullSyncData()
        {
            this._vehicle.GetNetWorkId();
        }
    }
}