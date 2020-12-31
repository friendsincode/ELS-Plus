
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
using System.Dynamic;

namespace ELS.Manager
{
    class VehicleManager
    {
        internal static VehicleList vehicleList;
        static bool notified = false;
        long lastServerSync;
        public VehicleManager()
        {
            vehicleList = new VehicleList();
            lastServerSync = Game.GameTime;
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
            List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            Utils.DebugWriteLine($"We have found {vehicles.Count} in the game");
            for(int i = 0; i < vehicles.Count; i++)
            {
                string plate = API.GetVehicleNumberPlateText(vehicles[i].Handle);
                if (vehicles[i].IsEls())
                {
                    if (vehicleList.ContainsKey(plate) && vehicles[i].Exists())
                    {
                        vehicleList[plate].RunControlTick();
                        vehicleList[plate].RunExternalTick();
                        vehicleList[plate].RunTick();
                    }
                    else
                    {
                        vehicleList.Add(vehicles[i].Handle);
                    }
                }
            }
            //            try
            //            {
            //                if (Game.PlayerPed.IsSittingInELSVehicle() &&
            //                        (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
            //                        || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed))
            //                {
            //                    //ELS.TriggerServerEvent("ELS:getServerNetworkId", Game.PlayerPed.CurrentVehicle.Handle, Game.PlayerPed.CurrentVehicle.GetNetworkId());
            //                    if (vehicleList.ContainsKey(Game.PlayerPed.CurrentVehicle.GetNetworkId()))
            //                    {
            //                        //Utils.DebugWriteLine("Vehicle is in the list running ticks");
            //                        ELSVehicle _currentVehicle = vehicleList[Game.PlayerPed.CurrentVehicle.GetNetworkId()];
            //                        _currentVehicle?.RunControlTick();
            //                        //vehicleList.RunExternalTick(_currentVehicle);
            //                        Game.PlayerPed.CurrentVehicle.SetExistOnAllMachines(true);
            //                    }
            //                    else
            //                    {
            //                        if (!vehicleList.VehRegAttempts.ContainsKey(Game.PlayerPed.CurrentVehicle.GetNetworkId()) || Game.GameTime - vehicleList.VehRegAttempts[Game.PlayerPed.CurrentVehicle.GetNetworkId()].Item2 >= 15000 && vehicleList.VehRegAttempts[Game.PlayerPed.CurrentVehicle.GetNetworkId()].Item1 < 5)
            //                        {
            //                            if (vehicleList.MakeSureItExists(API.VehToNet(Game.PlayerPed.CurrentVehicle.Handle), vehicle: out ELSVehicle _currentVehicle))
            //                            {
            //                                _currentVehicle?.RunControlTick();
            //                                // vehicleList.RunExternalTick(_currentVehicle);
            //                                Game.PlayerPed.CurrentVehicle.SetExistOnAllMachines(true);
            //                            }
            //                        }
            //                    }
            //                    vehicleList.RunTick();
            //                    vehicleList.RunExternalTick();
            //                    Indicator.RunAsync(Game.PlayerPed.CurrentVehicle);
            //#if DEBUG
            //                    //if (Game.IsControlJustPressed(0, Control.Cover))
            //                    //{
            //                    //    FullSync.FullSyncManager.SendDataBroadcast(
            //                    //        _currentVehicle.GetData(),
            //                    //        Game.Player.ServerId
            //                    //    );
            //                    //    CitizenFX.Core.UI.Screen.ShowNotification("FullSync™ ran");
            //                    //    CitizenFX.Core.Debug.WriteLine("FullSync™ ran");
            //                    //}
            //                    //Debug.DebugText();
            //#endif
            //                }
            //                else if (Game.PlayerPed.IsInVehicle() && (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
            //                    && (VehicleClass.Boats != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Trains != Game.PlayerPed.CurrentVehicle.ClassType
            //                    || VehicleClass.Planes != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Helicopters != Game.PlayerPed.CurrentVehicle.ClassType))
            //                {
            //                    if (Game.PlayerPed.CurrentVehicle.GetNetworkId() == 0)
            //                    {
            //                        makenetworked(Game.PlayerPed.CurrentVehicle);
            //                    }
            //                    Indicator.RunAsync(Game.PlayerPed.CurrentVehicle);
            //                    vehicleList.RunTick();
            //                }
            //                else
            //                {
            //                    vehicleList.RunTick();
            //                    vehicleList.RunExternalTick();
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                CitizenFX.Core.Debug.WriteLine($"VehicleManager Error: {e.Message} \n Stacktrace: {e.StackTrace}");
            //            }
            //            if (Game.GameTime - lastServerSync  >= 5000)
            //            {
            //                lastServerSync = Game.GameTime;
            //                ELS.TriggerServerEvent("ELS:FullSync:Request:All");
            //                Utils.DebugWriteLine("Requested Sync Data from server");
            //            }
            //TODO Chnage how I check for the panic alarm

        }

        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        async internal void SetVehicleSyncData(IDictionary<string, object> dataDic, int PlayerId)
        {
            if (Game.Player.ServerId == PlayerId)
            {
                Utils.DebugWriteLine("We are player exiting set sync data");
                return;
            }

            Utils.DebugWriteLine($"{PlayerId} has sent us data parsing");
            string plate = dataDic["plate"].ToString();
            if (vehicleList.ContainsKey(plate) && dataDic.ContainsKey("siren") || dataDic.ContainsKey("lights"))
            {
                vehicleList[plate].SetData(dataDic);
                Utils.DebugWriteLine($" Applying vehicle data with NETID of {plate}");
                return;
            }
            //if (!vehicleList.VehRegAttempts.ContainsKey(plate) || Game.GameTime - vehicleList.VehRegAttempts[plate].Item2 >= 15000 && vehicleList.VehRegAttempts[plate].Item1 < 5)
            //{
            //    if (!vehicleList.MakeSureItExists(netid, dataDic, out ELSVehicle veh1, PlayerId))
            //    {
            //        Utils.DebugWriteLine("Failed to register other clients vehicle");
            //        return;
            //    }
            //    //veh1.SetData(dataDic);
            //}
            //else
            //{
            //    Utils.DebugWriteLine("Attempting to register be patient");
            //}

            //if (dataDic.ContainsKey("IndState") && !dataDic.ContainsKey("siren") && !dataDic.ContainsKey("lights"))
            //{
            //    Utils.DebugWriteLine($"Ind sync data for {netid} is {dataDic["IndState"]}");
            //    Vehicle veh = (Vehicle)Vehicle.FromHandle(API.NetworkGetEntityFromNetworkId(netid));
            //    if (veh != null)
            //    {
            //        Indicator.ToggleInicatorState(veh, Indicator.IndStateLib[dataDic["IndState"].ToString()]);
            //    }
            //}
        }

        internal static void SyncUI(string plate)
        {
            if (String.IsNullOrEmpty(plate))
            {
                Utils.DebugWriteLine("Vehicle plate is empty");
                return;
            }
            vehicleList[plate].SyncUi();
        }

        internal static void SyncRequestReply(Commands command, string plate, int PlayerId)
        {
            if (String.IsNullOrEmpty(plate))
            {
                Utils.DebugWriteLine("ERROR sending vehicle data NetwordID equals 0\n");
                return;
            }
            switch (command)
            {
                //case Commands.ToggleInd:
                //    Dictionary<string, object> dict = new Dictionary<string, object>
                //    {
                //        {"plate",plate },
                //        {"IndState", Indicator.CurrentIndicatorState((Vehicle)Vehicle.FromHandle(API.NetworkGetEntityFromNetworkId(NetworkId))).ToString() }
                //    };
                //    Utils.DebugWriteLine($"Sending sync data for {dict["NetworkID"]} is {dict["IndState"]}");
                //    FullSync.FullSyncManager.SendDataBroadcast(dict, PlayerId);
                //    break;
                default:
                    FullSync.FullSyncManager.SendDataBroadcast(vehicleList[plate].GetData(), PlayerId);
                    break;

            }
        }
        internal void SyncAllVehiclesOnFirstSpawn(ExpandoObject data)
        {
            Utils.DebugWriteLine("Got broadcast to sync all data");
            dynamic k = data;
            var y = data.ToArray();
            List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            //Utils.DebugWriteLine("0");
            foreach (var struct1 in y)
            {
                //Utils.DebugWriteLine("1");
                string plate = struct1.Key;
                //Utils.DebugWriteLine("2");
                var vehData = (IDictionary<string, object>)struct1.Value;
                //Utils.DebugWriteLine("3");
                Utils.DebugWriteLine($"Setting data for plate {plate} and {vehData["NetworkID"]}");
                Vehicle veh = vehicles.Find(v => API.GetVehicleNumberPlateText(v.Handle).Equals(plate));
                vehicleList.Add(veh.Handle, vehData);
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
