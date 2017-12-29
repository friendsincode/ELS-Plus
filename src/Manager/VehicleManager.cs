using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;
using System.Collections;

namespace ELS.Manager
{
    class VehicleManager
    {
        static VehicleList vehicleList;
        public VehicleManager()
        {
            vehicleList = new VehicleList();
        }

        internal async void RunTickAsync()
        {
            //ELSVehicle _currentVehicle;
            if (Game.PlayerPed.IsSittingInELSVehicle() &&
                    (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
                    || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed))
            {
                //Screen.ShowNotification("adding vehicle");
                if (!API.IsEntityAMissionEntity(Game.PlayerPed.CurrentVehicle.Handle))
                {
                    CitizenFX.Core.Debug.WriteLine("Not a mission entity");
                }
                Game.PlayerPed.CurrentVehicle.SetExistOnAllMachines(true);
                if (vehicleList.MakeSureItExists(API.VehToNet(Game.PlayerPed.CurrentVehicle.Handle),vehicle: out ELSVehicle _currentVehicle )) {
                    _currentVehicle?.RunTick();
                }
                else
                {
                    var pos = Game.PlayerPed.CurrentVehicle.Position;
                    var rot = Game.PlayerPed.CurrentVehicle.Rotation;
                    var model = Game.PlayerPed.CurrentVehicle.Model;
                    Game.PlayerPed.CurrentVehicle.Delete();
                    var veh = await World.CreateVehicle(model, pos, rot.Z);
                    Game.PlayerPed.SetIntoVehicle(veh,VehicleSeat.Driver);
                }
#if DEBUG
                if (Game.IsControlJustPressed(0, Control.Cover))
                {
                    FullSync.FullSyncManager.SendDataBroadcast(
                        _currentVehicle.GetData()
                    );
                    CitizenFX.Core.UI.Screen.ShowNotification("FullSync™ ran");
                    CitizenFX.Core.Debug.WriteLine("FullSync™ ran");
                }
#endif
            }
            vehicleList.RunExternalTick();
            Debug.DebugText();

            //TODO Chnage how I check for the panic alarm
        }
 
        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        async internal void SetVehicleSyncData(IDictionary<string, object> dataDic)
        {
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"creating vehicle with NETID of {(int)dataDic["NetworkID"]} LOCALID of {CitizenFX.Core.Native.API.NetToVeh((int)dataDic["NetworkID"])}");
#endif
            var bo = vehicleList.MakeSureItExists((int)dataDic["NetworkID"]
                        , dataDic,out ELSVehicle veh1);
            veh1.SetData(dataDic);
        }

        internal static void SyncRequestReply(int NetworkId)
        {
            if (NetworkId == 0)
            {
                CitizenFX.Core.Debug.WriteLine("ERROR NetwordID equals 0\n");
                return;
            }
            FullSync.FullSyncManager.SendDataBroadcast(     
                (vehicleList.Find(o => o.GetNetworkId() == NetworkId)).GetData()
            );
        }
        internal void SyncAllVehiclesOnFirstSpawn(System.Dynamic.ExpandoObject data)
        {
            dynamic k = data;
            var y = data.ToArray();
            foreach ( var struct1 in y)
            {
                int netID = int.Parse(struct1.Key);
                var vehData = (IDictionary<string,object>)struct1.Value;
                vehicleList.MakeSureItExists((int)vehData["NetworkID"],
                        vehData,
                        out ELSVehicle veh
                );
            }
        }

        void GetAllVehicles()
        {

        }
        internal void CleanUP()
        {
            vehicleList.CleanUP();
        }
        ~VehicleManager()
        {
            CleanUP();
        }
    }
}