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
        static bool notified = false;
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
            //if (!veh.Model.IsLoaded) veh.Model.Request(-1);
            var net1 = API.VehToNet(veh.Handle);
            var attempts = 0;
            do
            {
                Utils.DebugWriteLine($"Attempt {attempts} of {veh.Handle} driven by {veh.GetPedOnSeat(VehicleSeat.Driver).Handle}");
                //BaseScript.Delay(500);
                var netid = API.NetworkGetNetworkIdFromEntity(veh.Handle);
                API.NetworkRegisterEntityAsNetworked(veh.Handle);
                API.SetEntityAsMissionEntity(veh.Handle, false, false);
                API.SetNetworkIdCanMigrate(netid, true);
                API.SetNetworkIdExistsOnAllMachines(netid, true);
                API.NetworkRequestControlOfEntity(veh.Handle);
                attempts++;
            }
            while (!API.NetworkDoesEntityExistWithNetworkId(veh.Handle) && attempts < 20);
            if (attempts == 20 && !notified)
            {
                CitizenFX.Core.Debug.WriteLine("Failed to register entity on net");
                notified = true;
            }
            else if (!notified)
            {
                CitizenFX.Core.Debug.WriteLine($"Registered {veh.Handle} on net as {net1}");
                ELS.TriggerEvent("ELS:VehicleEntered");
                notified = true;
            }
        }
        internal async void RunTickAsync()
        {
            try
            {
                if (Game.PlayerPed.IsSittingInELSVehicle() &&
                        (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
                        || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed))
                {
                    if (vehicleList.ContainsKey(Game.PlayerPed.CurrentVehicle.GetNetworkId()))
                    {
                        Utils.DebugWriteLine("Vehicle is in the list running ticks");
                        ELSVehicle _currentVehicle = vehicleList[Game.PlayerPed.CurrentVehicle.GetNetworkId()];
                        _currentVehicle?.RunTick();
                        vehicleList.RunExternalTick(_currentVehicle);
                        Game.PlayerPed.CurrentVehicle.SetExistOnAllMachines(true);
                    }
                    else
                    {
                        if (!vehicleList.VehRegAttempts.ContainsKey(Game.PlayerPed.CurrentVehicle.GetNetworkId()) || Game.GameTime - vehicleList.VehRegAttempts[Game.PlayerPed.CurrentVehicle.GetNetworkId()].Item2 >= 15000 && vehicleList.VehRegAttempts[Game.PlayerPed.CurrentVehicle.GetNetworkId()].Item1 < 5)
                        {
                            if (vehicleList.MakeSureItExists(API.VehToNet(Game.PlayerPed.CurrentVehicle.Handle), vehicle: out ELSVehicle _currentVehicle))
                            {
                                _currentVehicle?.RunTick();
                                vehicleList.RunExternalTick(_currentVehicle);
                                Game.PlayerPed.CurrentVehicle.SetExistOnAllMachines(true);
                            }
                            else
                            {
                                vehicleList.RunExternalTick();
                            }
                        }
                        else
                        {
                            vehicleList.RunExternalTick();
                        }
                    }
                    


#if DEBUG
                    //if (Game.IsControlJustPressed(0, Control.Cover))
                    //{
                    //    FullSync.FullSyncManager.SendDataBroadcast(
                    //        _currentVehicle.GetData(),
                    //        Game.Player.ServerId
                    //    );
                    //    CitizenFX.Core.UI.Screen.ShowNotification("FullSync™ ran");
                    //    CitizenFX.Core.Debug.WriteLine("FullSync™ ran");
                    //}
                    //Debug.DebugText();
#endif
                } else if (Game.PlayerPed.IsInVehicle() && (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
                    && (VehicleClass.Boats != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Trains != Game.PlayerPed.CurrentVehicle.ClassType
                    || VehicleClass.Planes != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Helicopters != Game.PlayerPed.CurrentVehicle.ClassType))
                {
                    if (Game.PlayerPed.CurrentVehicle.GetNetworkId() == 0)
                    {
                        makenetworked(Game.PlayerPed.CurrentVehicle);
                    }
                    Indicator.RunAsync(Game.PlayerPed.CurrentVehicle);
                }
                else
                {
                    vehicleList.RunExternalTick();
                }
            }
            catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"VehicleManager Error: {e.Message} \n Stacktrace: {e.StackTrace}");
            }

            //TODO Chnage how I check for the panic alarm
        }

        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        async internal void SetVehicleSyncData(IDictionary<string, object> dataDic,int PlayerId)
        {
            if (Game.Player.ServerId == PlayerId)
            {
                Utils.DebugWriteLine("We are player exiting set sync data");
                return;
            }

            Utils.DebugWriteLine($"{PlayerId} has sent us data parsing");
            int netid = int.Parse(dataDic["NetworkID"].ToString());
            if (vehicleList.ContainsKey(netid) && dataDic.ContainsKey("siren") || dataDic.ContainsKey("lights"))
            {
                vehicleList[netid].SetData(dataDic);
                Utils.DebugWriteLine($" Applying vehicle data with NETID of {netid} LOCALID of {API.NetToVeh(netid)}");
            } 
            else
            {
                if (!vehicleList.VehRegAttempts.ContainsKey(netid) || Game.GameTime - vehicleList.VehRegAttempts[netid].Item2 >= 15000 && vehicleList.VehRegAttempts[netid].Item1 < 5)
                {
                    if (!vehicleList.MakeSureItExists(netid, dataDic, out ELSVehicle veh1, PlayerId))
                    {
                        Utils.DebugWriteLine("Failed to register other clients vehicle");
                        return;
                    }
                    veh1.SetData(dataDic);
                }
                else
                {
                    Utils.DebugWriteLine("Attempting to register be patient");
                }
            }
            if (dataDic.ContainsKey("IndState") && !dataDic.ContainsKey("siren") && !dataDic.ContainsKey("lights"))
            {
                Utils.DebugWriteLine($"Ind sync data for {netid} is {dataDic["IndState"]}");
                Indicator.ToggleInicatorState((Vehicle)Vehicle.FromHandle(API.NetworkGetEntityFromNetworkId(netid)), Indicator.IndStateLib[dataDic["IndState"].ToString()]);
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

        internal static void SyncRequestReply(Commands command,int NetworkId,int PlayerId)
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
                int vehId = int.Parse(vehData["NetworkID"].ToString());
                vehicleList.MakeSureItExists(vehId,
                        vehData,
                        out ELSVehicle veh
                );
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
