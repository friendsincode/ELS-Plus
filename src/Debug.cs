using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;

namespace ELS
{
    class Debug
    {
        internal static async Task Spawn()
        {
            if (!Game.PlayerPed.IsInVehicle())
            {
                var polModel = new Model((VehicleHash)Game.GenerateHash("policegt350r"));
                await polModel.Request(-1);
                Vehicle veh = await World.CreateVehicle(polModel, Game.PlayerPed.Position);
                polModel.MarkAsNoLongerNeeded();
                //veh.RegisterAsNetworked();
                Screen.ShowNotification($"network status {Function.Call<bool>(Hash.NETWORK_GET_ENTITY_IS_NETWORKED, veh)}");
                veh.SetExistOnAllMachines(true);
                await CitizenFX.Core.BaseScript.Delay(10000);
                CitizenFX.Core.Debug.WriteLine($"vehtonet{API.VehToNet(veh.Handle)} getnetworkidfromentity{API.NetworkGetNetworkIdFromEntity(veh.Handle)}");
                CitizenFX.Core.Debug.WriteLine($"ModelName {veh.Model}" +
                    $"DisplayName {veh.DisplayName}");
                if (veh == null)
                {
                    CitizenFX.Core.Debug.WriteLine("failure to spawn");
                    return;
                }
            }
            else //if (Game.PlayerPed.CurrentVehicle.IsEls())
            {
                var veh = Game.PlayerPed.CurrentVehicle;
                for(var x =0; x < 24; x++)
                {
                    if (veh.ExtraExists(x))
                    {
                        veh.ToggleExtra(x, !veh.IsExtraOn(x));
                    }
                    }
                veh.ToggleExtra(1, true);
            }
            // Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Any);
        }

    }
}
