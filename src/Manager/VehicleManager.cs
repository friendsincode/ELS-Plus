
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Light;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ELS.RemoteEventManager;

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


        internal async Task RegistrationTick()
        {
            int count = 0;
            do
            {
                Utils.DebugWriteLine($"Delay {count} at {Game.GameTime}");
                await ELS.Delay(1000);
                count++;
            } while (count < 10);

            List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            for (int i = 0; i < vehicles.Count; i++)
            {
                if (vehicles[i].IsEls() && !API.DecorExistOn(vehicles[i].Handle, "elsplus_id") && vehicles[i].GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
                {
                    Random rnd1 = new Random();
                    int rnd = rnd1.Next(Game.GameTime);
                    Utils.DebugWriteLine($"Registering Decor Int of {rnd} for vehicle elsplus id");
                    API.DecorSetInt(vehicles[i].Handle, "elsplus_id", rnd);
                    vehicleList.Add(vehicles[i].Handle);
                }
                else if (vehicles[i].IsEls() && API.DecorExistOn(vehicles[i].Handle, "elsplus_id"))
                {
                    int id = vehicles[i].GetElsId();
                    int currVeh = 0;
                    if (Game.PlayerPed.CurrentVehicle != null)
                    {
                        currVeh = Game.PlayerPed.CurrentVehicle.GetElsId();
                    }
                    Utils.DebugWriteLine($"Got ELS Id for this vehicle of {id}");
                    //if (!vehicleList.ContainsKey(id))
                    //{
                    //    Utils.DebugWriteLine($"ELS Vehicle found with id of {id} attempting to get data from server");
                    //    //ELS.TriggerServerEvent("ELS:FullSync:UniCast", Game.Player.ServerId, id);
                    //    //await ELS.Delay(5000);
                    //}
                    string json = API.GetConvar("elsplus_data", "");
                    //Utils.DebugWriteLine($"Got Registration Json of: {json}");
                    if (!String.IsNullOrEmpty(json))
                    {
                        Dictionary<int, string> vehlist = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                        if (!vehicleList.ContainsKey(id) && vehlist.ContainsKey(id))
                        {
                            Utils.DebugWriteLine($"Veh not in client list registering new vehicle for id {id}");
                            vehicleList.Add(vehicles[i].Handle, JsonConvert.DeserializeObject<ELSVehicleFSData>(vehlist[id]));
                        }
                        else if (!vehicleList.ContainsKey(id) && !vehlist.ContainsKey(id))
                        {
                            Utils.DebugWriteLine($"Veh not in client list nor server list registering vehicle");
                            vehicleList.Add(vehicles[i].Handle);
                        } 
                        else if (vehicleList.ContainsKey(id) && currVeh != id)
                        {
                            vehicleList.Add(vehicles[i].Handle, vehicleList[id].GetData());
                        }
                    }
                }
            }
        }



        internal async Task RunTickAsync()
        {
            List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            for (int i = 0; i < vehicles.Count; i++)
            {
                if (vehicles[i].IsEls() && API.DecorExistOn(vehicles[i].Handle, "elsplus_id"))
                {
                    int id = API.DecorGetInt(vehicles[i].Handle, "elsplus_id");
                    if (vehicleList.ContainsKey(id) && vehicles[i].Exists())
                    {
                        //Utils.DebugWriteLine($"We have found {id} in the list running tick");
                        if ((vehicles[i].GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
                                   || vehicles[i].GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed) && Game.PlayerPed.CurrentVehicle.GetElsId() == id)
                        {
                            vehicleList[id].RunControlTick();
                        }
                        vehicleList[id].RunExternalTick();
                        vehicleList[id].RunTick();
                    }
                    //else if (vehicles[i].Exists())
                    //{
                    //    //Utils.DebugWriteLine($"We have not found {plate} in the list adding and running tick");
                    //    vehicleList.Add(vehicles[i].Handle);
                    //    vehicleList[plate].RunExternalTick();
                    //    vehicleList[plate].RunTick();
                    //}
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
            if (Game.PlayerPed.IsInVehicle() && (Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
               && (VehicleClass.Boats != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Trains != Game.PlayerPed.CurrentVehicle.ClassType
               || VehicleClass.Planes != Game.PlayerPed.CurrentVehicle.ClassType || VehicleClass.Helicopters != Game.PlayerPed.CurrentVehicle.ClassType))
            {
                Indicator.RunAsync(Game.PlayerPed.CurrentVehicle);
            }

            //            }
            //            catch (Exception e)
            //            {
            //                CitizenFX.Core.Debug.WriteLine($"VehicleManager Error: {e.Message} \n Stacktrace: {e.StackTrace}");
            //            }
            //if (Game.GameTime - lastServerSync >= 5000)
            //{
            //    lastServerSync = Game.GameTime;
            //    ELS.TriggerServerEvent("ELS:FullSync:Request:All");
            //    Utils.DebugWriteLine("Requested Sync Data from server");
            //}
            //TODO Chnage how I check for the panic alarm
#if DEBUG
            
                Debug.DebugText();
            
#endif
        }

        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        async internal void SetVehicleSyncData(string json, int PlayerId)
        {
            ELSVehicleFSData data = JsonConvert.DeserializeObject<ELSVehicleFSData>(json);
            if (Game.Player.ServerId == PlayerId)
            {
                Utils.DebugWriteLine("We are player exiting set sync data");
                return;
            }
            //dynamic dataDic = JsonConvert.Deserialize<>(data);
            int id = data.Id;
            Utils.DebugWriteLine($"{PlayerId} has sent us data for {id} parsing");
            
            if (vehicleList.ContainsKey(id) && !data.Siren.Equals(null) || !data.Light.Equals(null))
            {
                Utils.DebugWriteLine($" Applying vehicle data with els id of of {id}");
                try
                {
                    vehicleList[id].SetData(data);
                }catch (KeyNotFoundException e)
                {
                    Utils.DebugWriteLine("Key not found for some dumbass reason lets just regiseter it");
                    json = API.GetConvar("elsplus_data", "");
                    if (!String.IsNullOrEmpty(json))
                    {

                        Dictionary<int, string> vehlist = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                        List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
                        for (int i = 0; i < vehicles.Count; i++)
                        {
                            if (API.DecorExistOn(vehicles[i].Handle, "elsplus_id"))
                            {
                                int vehId = vehicles[i].GetElsId();
                                if (id == vehId)
                                {
                                    vehicleList.Add(vehicles[i].Handle, JsonConvert.DeserializeObject<ELSVehicleFSData>(vehlist[id]));
                                }
                            }
                        }
                    }
                }
                return;
            }
            else
            {
                json = API.GetConvar("elsplus_data", "");
                if (!String.IsNullOrEmpty(json))
                {

                    Dictionary<int, string> vehlist = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                    List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        if (API.DecorExistOn(vehicles[i].Handle, "elsplus_id"))
                        {
                            int vehId = vehicles[i].GetElsId();
                            if (id == vehId)
                            {
                                vehicleList.Add(vehicles[i].Handle, JsonConvert.DeserializeObject<ELSVehicleFSData>(vehlist[id]));
                            }
                        }
                    }
                }
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

            //if (!data..Equals(null) && !data.ContainsKey("siren") && !data.ContainsKey("lights"))
            //{
            //    List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            //    for (int i = 0; i < vehicles.Count; i++)
            //    {
            //        if (vehicles[i].GetPedOnSeat(VehicleSeat.Driver).Handle == API.GetPlayerFromServerId(PlayerId))
            //        {
            //            Indicator.ToggleInicatorState(vehicles[i], Indicator.IndStateLib[data["IndState"].ToString()]);
            //        }
            //    }

            //}
        }

        internal static void SyncUI(int id)
        {
            if (id != -1)
            {
                Utils.DebugWriteLine("Vehicle plate is empty");
                return;
            }
            vehicleList[id].SyncUi();
        }

        internal static void SyncRequestReply(Commands command, int id, int PlayerId)
        {
            if (id == -1)
            {
                Utils.DebugWriteLine("ERROR sending vehicle data plate is null\n");
                return;
            }
            switch (command)
            {
                //case Commands.ToggleInd:
                    
                         
                //    Dictionary<string, object> dict = new Dictionary<string, object>
                //    {
                //        {"IndState", Indicator.CurrentIndicatorState(Game.PlayerPed.CurrentVehicle) }
                //    };
                //    Utils.DebugWriteLine($"Sending sync data for is {dict["IndState"]}");
                //    FullSync.FullSyncManager.SendDataBroadcast(dict, PlayerId);
                //    break;
                default:
                    string data = JsonConvert.SerializeObject(vehicleList[id].GetData());
                    FullSync.FullSyncManager.SendDataBroadcast(data, PlayerId) ;
                    Utils.DebugWriteLine($"Sync Request JSON: {data}");
                    break;

            }
        }

        internal void SyncAllVehiclesOnFirstSpawn(Dictionary<int, ELSVehicleFSData> data)
        {
            Utils.DebugWriteLine("Got broadcast to sync all data");
            List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            foreach (object struct1 in data)
            {
                Dictionary<string, object> parsed = (Dictionary<string, object>)struct1;
               
                Utils.DebugWriteLine(parsed.ToString());

                //int id = parsed["id"];
                ////Utils.DebugWriteLine("2");
                //var vehData = (Dictionary<string, object>)struct1.Value;
                ////Utils.DebugWriteLine("3");
                //Utils.DebugWriteLine($"Setting data for els id {id}");
                //Vehicle veh = vehicles.Find(v => v.GetElsId() == id);
                //if (!vehicleList.ContainsKey(id))
                //{
                //    vehicleList.Add(id, vehData);
                //}
            }
        }

        internal void AddVehicle(int id, ELSVehicleFSData data)
        {
            Utils.DebugWriteLine("AddVehicle:Start vehicle add");
            List<Vehicle> vehicles = new List<Vehicle>(World.GetAllVehicles());
            Utils.DebugWriteLine("AddVehicle:converted to dynamic");
            int idKey = data.Id;
            Vehicle veh = vehicles.Find(v => v.GetElsId() == id);
            Utils.DebugWriteLine($"AddVehicle:Attempting to register vehicle with id of {idKey}");
            if (idKey != id)
            {
                Utils.DebugWriteLine("AddVehicle:IDs do not match lets try again");
                return;
            }

            if (!vehicleList.ContainsKey(id))
            {
                vehicleList.Add(veh.Handle, data);
                Utils.DebugWriteLine("AddVehicle:Vehicle added");
            }
            else
            {
                vehicleList[id].SetData(data);
                Utils.DebugWriteLine("AddVehicle:Vehicle in listed sent new data");
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
