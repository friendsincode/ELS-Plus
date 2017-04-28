using System;
using CitizenFX.Core;

namespace ELS.Siren
{
    partial class Siren
    {
        struct syncSettings
        {
            private bool dualsiren;
            private bool mainSiren;
        }
        public void updateLocalRemoteELSSettings()
        {

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
                    break;
                case RemoteEventManager.Commands.AirHorn:
                    AirHornControls(state);
                    break;
            }
        }
    }
}