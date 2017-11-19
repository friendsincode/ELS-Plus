using System;
using CitizenFX.Core;
using System.Collections.Generic;

namespace ELS.Siren
{
    partial class Siren
    {
        public void updateLocaFromlRemoteSirenControlData(string sirenString, bool state)
        {
#if DEBUG
            CitizenFX.Core.Debug.WriteLine(sirenString.ToString());
#endif
            Enum.TryParse(sirenString, out RemoteEventManager.Commands command);
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