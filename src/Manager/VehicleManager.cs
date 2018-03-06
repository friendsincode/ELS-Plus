using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;
using System.Collections;
using System.Threading.Tasks;
using ELS.configuration;
using ELS.Light;
using static ELS.RemoteEventManager;

namespace ELS.Manager
{
    class VehicleManager
    {
        internal static VehicleList vehicleList;
        private static ArrayList blacklist = new ArrayList(100);
        public VehicleManager()
        {
            vehicleList = new VehicleList();
        }
        internal static void makenetworked(Vehicle veh)
        {
            //////////
            ///
            ///  THANKS to Antivirus-chan in the FiveM community for supplying this code
            ///
            //////////
            //
            var net1 = API.VehToNet(veh.Handle);
            var attempts = 0;
            do
            {
                Utils.DebugWriteLine($"This is attempt {attempts} we have a netid of {net1}");
                int net2 = API.NetworkGetNetworkIdFromEntity(veh.Handle);
                //BaseScript.Delay(500);
                if (net1 != net2)
                {
                    net1 = net2;
                }
                API.NetworkRegisterEntityAsNetworked(veh.Handle);
                API.SetEntityAsMissionEntity(veh.Handle, false, false);
                API.SetNetworkIdCanMigrate(net1, true);
                API.SetNetworkIdExistsOnAllMachines(net1, true);
                API.NetworkRequestControlOfEntity(veh.Handle);
                attempts++;
                if (attempts >= 100 && net1==0)
                {
                    blacklist.Add(veh);
                    CitizenFX.Core.Debug.WriteLine("Failed to register entity on net\nAdded to Vehicle Backlist blacklist");
                    break;
                }
                else if(net1!=0)
                {
                    break;
                }
            }
            while ( attempts < 100 /*|| !API.NetworkDoesEntityExistWithNetworkId(veh.Handle)*/ );
            
        }
        internal void RunTick()
        {
            try
            {
                if (Game.PlayerPed.IsSittingInELSVehicle() &&
                    !blacklist.Contains(Game.PlayerPed.CurrentVehicle) &&
                        (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
                        || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed))
                {
                    var netid = Game.PlayerPed.CurrentVehicle.GetNetworkId();
                    Utils.DebugWriteLine($"Player is in police vehicle with net id of {netid}");
                    if (netid == 0)
                    {
                        makenetworked(Game.PlayerPed.CurrentVehicle);
                    }
                    if (!blacklist.Contains(Game.PlayerPed.CurrentVehicle) && vehicleList.MakeSureItExists(Game.PlayerPed.CurrentVehicle.GetNetworkId(), vehicle: out ELSVehicle _currentVehicle))
                    {
                        Utils.DebugWriteLine("Vehicle is in vehicle list");
                        _currentVehicle?.RunTick();
                        vehicleList.RunExternalTick(_currentVehicle);
                        Game.PlayerPed.CurrentVehicle.SetExistOnAllMachines(true);
                    }
                    else
                    {
                        //Utils.DebugWriteLine("Vehicle does not exist");
                        //makenetworked(Game.PlayerPed.CurrentVehicle);
                        //var pos = Game.PlayerPed.CurrentVehicle.Position;
                        //var rot = Game.PlayerPed.CurrentVehicle.Rotation;
                        //var model = Game.PlayerPed.CurrentVehicle.Model;
                        //Game.PlayerPed.CurrentVehicle.Delete();
                        //var veh = await World.CreateVehicle(model, pos, rot.Z);
                        //Game.PlayerPed.SetIntoVehicle(veh,VehicleSeat.Driver);
                        //vehicleList.Add(new ELSVehicle(Game.PlayerPed.CurrentVehicle.Handle));
                    }


#if false
                    if (Game.IsControlJustPressed(0, Control.Cover))
                    {
                        FullSync.FullSyncManager.SendDataBroadcast(
                            _currentVehicle.GetData(),
                            Game.Player.ServerId
                        );
                        CitizenFX.Core.UI.Screen.ShowNotification("FullSync™ ran");
                        CitizenFX.Core.Debug.WriteLine("FullSync™ ran");
                    }
                    Debug.DebugText();
#endif
                }
                else
                {
                    vehicleList.RunExternalTick();
                }
                if (Game.PlayerPed.IsInVehicle() && (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
                    && (VehicleClass.Boats != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Trains != Game.PlayerPed.CurrentVehicle.ClassType
                    || VehicleClass.Planes != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Helicopters != Game.PlayerPed.CurrentVehicle.ClassType))
                {
                    //makenetworked(Game.PlayerPed.CurrentVehicle);
                    Indicator.RunAsync(Game.PlayerPed.CurrentVehicle);
                }
                if (Game.PlayerPed.IsGettingIntoAVehicle && !Game.PlayerPed.IsSittingInVehicle() ){
                    var veh = API.GetVehiclePedIsUsing(Game.PlayerPed.Handle);
                    ELS.TriggerEvent("ELS:VehicleEntered",veh);
                }
            }
            catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"VehicleManager Error: {e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        async internal void SetVehicleSyncData(IDictionary<string, object> dataDic, int PlayerId)
        {
            Utils.DebugWriteLine($"Got Sync Data for {dataDic["NetworkID"]}");
            if (Game.Player.ServerId == PlayerId || dataDic == null) return;
            var bo = vehicleList.MakeSureItExists((int)dataDic["NetworkID"], out ELSVehicle veh1, dataDic);
            if (bo && dataDic.ContainsKey("siren") || dataDic.ContainsKey("light"))
            {
                veh1.SetData(dataDic);

#if DEBUG
                CitizenFX.Core.Debug.Write($" Applying vehicle data with NETID of {(int)dataDic["NetworkID"]} LOCALID of {CitizenFX.Core.Native.API.NetToVeh((int)dataDic["NetworkID"])}");
#endif
            }
            else if (dataDic.ContainsKey("IndState"))
            {
                Utils.DebugWriteLine($"Ind sync data for {dataDic["NetworkID"].ToString()} is {dataDic["IndState"]}");
                Indicator.ToggleInicatorState((Vehicle)Vehicle.FromHandle(API.NetworkGetEntityFromNetworkId((int)dataDic["NetworkID"])), Indicator.IndStateLib[dataDic["IndState"].ToString()]);
            }
        }

        internal static void SyncUI(int netId)
        {
            if (netId == 0)
            {
                Utils.DebugWriteLine("Vehicle net ID is empty");
                return;
            }
            vehicleList[netId].SyncUi();
        }

        internal static void SyncRequestReply(Commands command, int NetworkId, int PlayerId)
        {
            if (NetworkId == 0)
            {
                Utils.DebugWriteLine("ERROR sending vehicle data NetwordID equals 0\n");
                return;
            }
            switch (command)
            {
                case Commands.ToggleInd:
                    Dictionary<string, object> dict = new Dictionary<string, object>
                    {
                        {"NetworkID",NetworkId },
                        {"IndState", Indicator.CurrentIndicatorState((Vehicle)Vehicle.FromHandle(API.NetworkGetEntityFromNetworkId(NetworkId))).ToString() }
                    };
                    Utils.DebugWriteLine($"Sending sync data for {dict["NetworkID"]} is {dict["IndState"]}");
                    FullSync.FullSyncManager.SendDataBroadcast(dict, PlayerId);
                    break;
                default:
                    FullSync.FullSyncManager.SendDataBroadcast(vehicleList[NetworkId].GetData(), PlayerId);
                    break;

            }

        }
        internal void SyncAllVehiclesOnFirstSpawn(System.Dynamic.ExpandoObject data)
        {
            dynamic k = data;
            var y = data.ToArray();
            foreach (var struct1 in y)
            {
                int netID = int.Parse(struct1.Key);
                var vehData = (IDictionary<string, object>)struct1.Value;
                vehicleList.MakeSureItExists((int)vehData["NetworkID"],
                    out ELSVehicle veh
,
                    vehData);
            }
        }

        void GetAllVehicles()
        {

        }

        void RemoveStallVehicles()
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
