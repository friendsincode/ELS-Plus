using System.Collections.Generic;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        internal void FullSync()
        {
            this._mainSiren.RunSync();
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
            _mainSiren.RequestData();
            _tones.RequestData();
        }
    }
}