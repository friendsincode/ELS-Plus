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
            var polModel = new Model(VehicleHash.Police);
            await polModel.Request(-1);
            Vehicle veh = await World.CreateVehicle(polModel, Game.PlayerPed.Position);
            polModel.MarkAsNoLongerNeeded();
            //veh.RegisterAsNetworked();
            Screen.ShowNotification($"network status {Function.Call<bool>(Hash.NETWORK_GET_ENTITY_IS_NETWORKED, veh)}");
            //veh.SetExistOnAllMachines(true);
            //await CitizenFX.Core.BaseScript.Delay(10000);

            if (veh == null)
            {
                CitizenFX.Core.Debug.WriteLine("failure to spawn");
                return;
            }
            Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Any);
        }

    }
}
