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
            var disableHWInput = false;
            if (Game.Player.Character.CurrentVehicle == this._vehicle &&
               this._vehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed &&
               this.dual_siren)
            {
                disableHWInput = true;
            }
            Enum.TryParse(sirenString, out RemoteEventManager.Commands command);
            switch (command)
            {
                case RemoteEventManager.Commands.MainSiren:
                    MainSirenToggleLogic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.AirHorn:
                    AirHornLogic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.ManualTone1:
                    ManualTone1Logic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.ManualTone2:
                    ManualTone2Logic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.ManualTone3:
                    ManualTone3Logic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.ManualTone4:
                    ManualTone4Logic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.ManualSound:
                    ManualSoundLogic(state,disableHWInput);
                    break;
                case RemoteEventManager.Commands.DualSiren:
                    DualSirenLogic(state,disableHWInput);
                    break;
            }
        }
    }
}