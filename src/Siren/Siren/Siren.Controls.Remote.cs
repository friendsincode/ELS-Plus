using System;
using CitizenFX.Core;
using System.Collections.Generic;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        public void SirenControlsRemote(string sirenString, bool state)
        {
#if DEBUG
            CitizenFX.Core.Debug.WriteLine(sirenString.ToString());
#endif
            Enum.TryParse(sirenString, out RemoteEventManager.Commands command);
            switch (command)
            {
                case RemoteEventManager.Commands.MainSiren:
                    MainSirenToggleLogic(state);
                    break;
                case RemoteEventManager.Commands.AirHorn:
                    AirHornLogic(state);
                    break;
                case RemoteEventManager.Commands.ManualTone1:
                    ManualTone1Logic(state);
                    break;
                case RemoteEventManager.Commands.ManualTone2:
                    ManualTone2Logic(state);
                    break;
                case RemoteEventManager.Commands.ManualTone3:
                    ManualTone3Logic(state);
                    break;
                case RemoteEventManager.Commands.ManualTone4:
                    ManualTone4Logic(state);
                    break;
            }
        }
    }
}