
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Light;
using ELS.configuration;
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
        private readonly Dictionary<int, ELSVehicleFSData> _pendingSync = new Dictionary<int, ELSVehicleFSData>();

        public VehicleManager()
        {
            vehicleList = new VehicleList();
        }

        internal static async Task<bool> makenetworked(Vehicle veh)
        {
            for (int attempt = 0; attempt < 20 && veh.Exists(); attempt++)
            {
                if (!API.NetworkGetEntityIsNetworked(veh.Handle))
                    API.NetworkRegisterEntityAsNetworked(veh.Handle);
                int netId = API.NetworkGetNetworkIdFromEntity(veh.Handle);
                if (netId > 0 && API.NetworkDoesEntityExistWithNetworkId(netId))
                {
                    API.SetEntityAsMissionEntity(veh.Handle, false, false);
                    API.SetNetworkIdCanMigrate(netId, true);
                    return true;
                }
                await ELS.Delay(100);
            }
            Utils.ReleaseWriteLine("Failed to register ELS vehicle on the network");
            return false;
        }

        internal async Task ElsIDSetTick()
        {
            await ELS.Delay(1000);
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                if (!vehicle.IsEls() || vehicle.Driver != Game.PlayerPed || !API.NetworkGetEntityIsNetworked(vehicle.Handle))
                    continue;
                // ELS identity survives ownership changes; network IDs only locate entities.
                if (vehicle.GetElsId() <= 0)
                {
                    int id;
                    do { id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0) & int.MaxValue; }
                    while (id == 0 || vehicleList.ContainsKey(id));
                    API.DecorSetInt(vehicle.Handle, "elsplus_id", id);
                }
            }
        }

        internal async Task SoundCleanUpTick()
        {
            await ELS.Delay(400);
            foreach (ELSVehicle vehicle in vehicleList.Values)
                vehicle.CleanUP();
        }

        internal async Task RegistrationTick()
        {
            await ELS.Delay(Math.Max(1, Global.RegistrationDelay) * 1000);
            foreach (int id in new List<int>(vehicleList.Keys))
            {
                if (!vehicleList[id].Exists() || vehicleList[id].GetVehicle.GetElsId() != id)
                    RemoveVehicle(id, false);
            }
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                int id = vehicle.GetElsId();
                if (!vehicle.IsEls() || id <= 0)
                    continue;
                if (vehicleList.ContainsKey(id)) continue;
                try
                {
                    ELSVehicleFSData data;
                    if (_pendingSync.TryGetValue(id, out data))
                        vehicleList.Add(vehicle.Handle, data);
                    else
                        vehicleList.Add(vehicle.Handle);
                }
                catch (Exception ex)
                {
                    Utils.ReleaseWriteLine($"Could not register ELS vehicle {id}: {ex.Message}");
                }
            }
            // Retry the owner's initial state after server-side entity replication catches up.
            Vehicle current = Game.PlayerPed.CurrentVehicle;
            if (current != null && current.Driver == Game.PlayerPed && vehicleList.ContainsKey(current.GetElsId()))
                SyncRequestReply(Commands.MainSiren, current.GetElsId(), Game.Player.ServerId);
        }

        internal async Task ControlTickAsync()
        {
            if (Game.PlayerPed.CurrentVehicle != null && Game.PlayerPed.CurrentVehicle.IsEls())
            {
                if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed)
                {
                    int id = Game.PlayerPed.CurrentVehicle.GetElsId();
                    if (vehicleList.ContainsKey(id))
                    {
                        vehicleList[id].RunControlTick();
                    }
                }
            }
        }



        internal async Task RunTickAsync()
        {
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                int id = vehicle.GetElsId();
                ELSVehicle managed;
                if (vehicle.IsEls() && vehicleList.TryGetValue(id, out managed))
                {
                    // A streamed-in entity can have a different local handle.
                    if (!managed.Exists() || managed.GetVehicle.Handle != vehicle.Handle) continue;
                    managed.RunExternalTick();
                    if (vehicleList.ContainsKey(id)) managed.RunTick();
                }
            }
            Vehicle current = Game.PlayerPed.CurrentVehicle;
            if (current != null && current.Driver == Game.PlayerPed &&
                current.ClassType != VehicleClass.Boats && current.ClassType != VehicleClass.Trains &&
                current.ClassType != VehicleClass.Planes && current.ClassType != VehicleClass.Helicopters)
                Indicator.RunAsync(current);
#if DEBUG
            Debug.DebugText();
#endif
        }

        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        internal void SetVehicleSyncData(string json, int playerId)
        {
            try
            {
                ELSVehicleFSData data = JsonConvert.DeserializeObject<ELSVehicleFSData>(json);
                if (data.Id <= 0) return;
                _pendingSync[data.Id] = data;
                if (Game.Player.ServerId != playerId) AddVehicle(data.Id, data);
            }
            catch (Exception ex)
            {
                Utils.ReleaseWriteLine($"Ignoring invalid ELS sync: {ex.Message}");
            }
        }

        internal void RemoveVehicle(int id, bool forgetState = true)
        {
            if (forgetState) _pendingSync.Remove(id);
            ELSVehicle vehicle;
            if (vehicleList.TryGetValue(id, out vehicle))
            {
                vehicle.Dispose();
                vehicleList.Remove(id);
            }
        }

        internal void ClearVehicles()
        {
            foreach (int id in new List<int>(vehicleList.Keys)) RemoveVehicle(id);
            _pendingSync.Clear();
        }

        internal static void SyncUI(int id)
        {
            ELSVehicle vehicle;
            if (vehicleList.TryGetValue(id, out vehicle)) vehicle.SyncUi();
        }

        internal static void SyncRequestReply(Commands command, int id, int PlayerId)
        {
            if (id <= 0 || !vehicleList.ContainsKey(id))
            {
                Utils.DebugWriteLine("ERROR sending vehicle data plate is null\n");
                return;
            }
            switch (command)
            {
                case Commands.ToggleInd:


                    Dictionary<string, object> dict = new Dictionary<string, object>
                    {
                        {"IndState", Indicator.CurrentIndicatorState(Game.PlayerPed.CurrentVehicle) }
                    };
                    Utils.DebugWriteLine($"Sending sync data for is {dict["IndState"]}");
                    //FullSync.FullSyncManager.SendDataBroadcast(dict, PlayerId);
                    break;
                default:
                    string data = JsonConvert.SerializeObject(vehicleList[id].GetData());
                    FullSync.FullSyncManager.SendDataBroadcast(data, PlayerId) ;
                    Utils.DebugWriteLine($"Sync Request JSON: {data}");
                    break;

            }
        }

        internal void SyncAllVehiclesOnFirstSpawn(string json)
        {
            try
            {
                var snapshot = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
                if (snapshot == null) return;
                _pendingSync.Clear();
                foreach (var entry in snapshot) SetVehicleSyncData(entry.Value, -1);
            }
            catch (JsonException ex)
            {
                Utils.ReleaseWriteLine($"Ignoring invalid ELS snapshot: {ex.Message}");
            }
        }

        internal void AddVehicle(int id, ELSVehicleFSData data)
        {
            if (id <= 0 || data.Id != id) return;
            ELSVehicle existing;
            if (vehicleList.TryGetValue(id, out existing))
            {
                if (existing.Exists() && existing.GetVehicle.GetElsId() == id)
                {
                    existing.SetData(data);
                    return;
                }
                RemoveVehicle(id, false);
            }
            // Resolve by stable ELS identity, never assume a stored handle or net ID is current.
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                if (vehicle.Exists() && vehicle.IsEls() && vehicle.GetElsId() == id)
                {
                    vehicleList.Add(vehicle.Handle, data);
                    break;
                }
            }
        }

        internal void CleanUP()
        {
            ClearVehicles();
        }

    }
}
