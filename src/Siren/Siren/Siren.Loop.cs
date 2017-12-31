using CitizenFX.Core;
using ELS.Light;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        public void Ticker()
        {
            Game.DisableControlThisFrame(0, Control.VehicleHorn);
            if (Game.IsControlJustReleased(0, Control.VehicleHorn))
            {
                _vehicle.IsSirenActive = !_vehicle.IsSirenActive;
                var veh = Game.PlayerPed.CurrentVehicle;
                //if (ElsUiPanel._runPattern)
                //{
                //    ElsUiPanel._runPattern = false;
                //}
                //else
                //{
                //    ElsUiPanel._runPattern = true;
                //    LightPattern.RunLightPattern(veh, 1, LightPattern.StringPatterns[0] + LightPattern.StringPatterns[2], "red",75);
                //    LightPattern.RunLightPattern(veh, 2, LightPattern.StringPatterns[1] + LightPattern.StringPatterns[2], "red",75);
                //    LightPattern.RunLightPattern(veh, 3, LightPattern.StringPatterns[0] + LightPattern.StringPatterns[3], "blue",75);
                //    LightPattern.RunLightPattern(veh, 4, LightPattern.StringPatterns[1] + LightPattern.StringPatterns[3], "blue",75);
                //    LightPattern.RunLightPattern(veh, 7, LightPattern.StringPatterns[33], "amber",100);
                //    LightPattern.RunLightPattern(veh, 8, LightPattern.StringPatterns[32], "amber",100);
                //    LightPattern.RunLightPattern(veh, 9, LightPattern.StringPatterns[33], "amber",100);
                //}
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
        public void ExternalTicker()
        {
            PanicAlarmControlsKB();
        }
    }
}
