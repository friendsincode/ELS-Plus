using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        public void ticker()
        {
            Game.DisableControlThisFrame(0, Control.VehicleHorn);
            if (Game.IsControlJustReleased(0, Control.VehicleHorn))
            {
                _vehicle.IsSirenActive = !_vehicle.IsSirenActive;
            }

            AirHornControlsKB();
            ManualTone1ControlsKB();
            ManualTone2ControlsKB();
            ManualTone3ControlsKB();
            ManualTone4ControlsKB();
            ManualSoundControlsKB();
            MainSirenToggleControlsKB();
            DualSirenControlsKB();

        }
    }
}
