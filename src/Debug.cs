using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;
using System.Threading.Tasks;

namespace ELS
{
    class Debug
    {
        internal static async Task Spawn()
        {
#if DEBUG
            if (!Game.PlayerPed.IsInVehicle())
            {
                var polModel = new Model((VehicleHash)Game.GenerateHash("sadot"));
                await polModel.Request(-1);
                Vehicle veh = await World.CreateVehicle(polModel, Game.PlayerPed.Position);
                polModel.MarkAsNoLongerNeeded();
                //veh.RegisterAsNetworked();
                Screen.ShowNotification($"network status {Function.Call<bool>(Hash.NETWORK_GET_ENTITY_IS_NETWORKED, veh)}");
                veh.SetExistOnAllMachines(true);
                await CitizenFX.Core.BaseScript.Delay(10000);
                CitizenFX.Core.Debug.WriteLine($"vehtonet: {API.VehToNet(veh.Handle)} getnetworkidfromentity: {API.NetworkGetNetworkIdFromEntity(veh.Handle)}");
                CitizenFX.Core.Debug.WriteLine($"ModelName: {veh.Model} DisplayName: {veh.DisplayName}");
                CitizenFX.Core.Debug.WriteLine($"Is this ELS Vehicle {veh.IsEls()}");
                if (veh == null)
                {
                    CitizenFX.Core.Debug.WriteLine("failure to spawn");
                    return;
                }
            }
            else //if (Game.PlayerPed.CurrentVehicle.IsEls())
            {
                Vehicle veh = Game.PlayerPed.CurrentVehicle;
                veh.RadioStation = RadioStation.RadioOff;
            }
            // Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Any);
#endif
        }
        internal static void DebugText()
        {
            if (Game.Player.Character.CurrentVehicle == null )
            {
                return;
            }
            if (!Game.Player.Character.CurrentVehicle.IsEls() || !API.DecorExistOn(Game.Player.Character.CurrentVehicle.Handle, "elsplus_id")) { return; }
            int id = API.DecorGetInt(Game.Player.Character.CurrentVehicle.Handle, "elsplus_id");
            //var bonePos = Game.Player.Character.LastVehicle.Bones["door_dside_f"].Position;
            //var pos = Game.Player.Character.GetPositionOffset(bonePos);
            /*X:{pos.X} Y:{pos.Y} Z:{pos.Z} Lenght:{pos.Length()}*/
            var text = new Text($" ELS ID of Currently {id}", new PointF(Screen.Width / 2.0f, 10f), 0.5f)
            {
                Alignment = Alignment.Center
            };
            text.Color = Color.FromArgb(255, 0, 0);
            text.Draw();
        }
    }
}
