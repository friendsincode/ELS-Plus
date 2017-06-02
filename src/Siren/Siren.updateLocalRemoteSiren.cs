using System;
using CitizenFX.Core;
using System.Collections.Generic;

namespace ELS.Siren
{
    partial class Siren
    {
        struct syncSettings
        {
            private bool dualsiren;
            private bool mainSiren;
        }
        internal void FullSync()
        {
            this._mainSiren.FullSync();
            this._tones.FullSync();
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
        public void updateLocalRemoteSiren(string sirenString, bool state)
        {
#if DEBUG
            Debug.WriteLine(sirenString.ToString());
#endif
            RemoteEventManager.Commands command;
            Enum.TryParse(sirenString, out command);
            switch (command)
            {
                case RemoteEventManager.Commands.MainSiren:
                    MainSirenToggleControls(state);
                    break;
                case RemoteEventManager.Commands.AirHorn:
                    AirHornControls(state);
                    break;
                case RemoteEventManager.Commands.ManualTone1:
                    ManualTone1Controls(state);
                    break;
                case RemoteEventManager.Commands.ManualTone2:
                    ManualTone2Controls(state);
                    break;
                case RemoteEventManager.Commands.ManualTone3:
                    ManualTone3Controls(state);
                    break;
                case RemoteEventManager.Commands.ManualTone4:
                    ManualTone4Controls(state);
                    break;
            }
        }
    }
}