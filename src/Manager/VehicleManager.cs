
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

        private readonly HashSet<int> _confirmedNew = new HashSet<int>();
        private int _lastStateBroadcast;
        private int _lastSnapshotRequest = -1000;

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
            Vehicle vehicle = Game.PlayerPed.CurrentVehicle;
            if (vehicle == null || !vehicle.Exists() || !vehicle.IsEls() || vehicle.Driver != Game.PlayerPed ||
                !API.NetworkGetEntityIsNetworked(vehicle.Handle) || !API.NetworkHasControlOfEntity(vehicle.Handle)) return;
            int id = vehicle.GetElsId();
            if (id <= 0 || (!vehicleList.ContainsKey(id) && !_pendingSync.ContainsKey(id) && !_confirmedNew.Contains(id)))
            {
                // The server recovers the same identity after decorator/owner replication delays.
                // Echo the local handle only to match the reply to this exact client-side instance.
                ELS.TriggerServerEvent("ELS:FullSync:Resolve", API.NetworkGetNetworkIdFromEntity(vehicle.Handle), vehicle.Handle);
            }
        }

        internal void AcceptIdentity(int handle, int networkId, int id, string json)
        {
            Vehicle vehicle = Game.PlayerPed.CurrentVehicle;
            if (id <= 0 || vehicle == null || !vehicle.Exists() || vehicle.Handle != handle ||
                !vehicle.IsEls() || vehicle.Driver != Game.PlayerPed || !API.NetworkHasControlOfEntity(handle) ||
                API.NetworkGetNetworkIdFromEntity(handle) != networkId) return;
            if (!String.IsNullOrEmpty(json)) SetVehicleSyncData(json, Game.Player.ServerId);
            else _confirmedNew.Add(id);
            API.DecorSetInt(handle, "elsplus_id", id);
        }

        private static bool IsBound(int id, ELSVehicle vehicle)
        {
            return vehicle.Exists() && vehicle.GetVehicle.GetElsId() == id;
        }

        internal async Task SoundCleanUpTick()
        {
            await ELS.Delay(400);
            foreach (int id in new List<int>(vehicleList.Keys))
            {
                if (!IsBound(id, vehicleList[id])) RemoveVehicle(id, false);
                else vehicleList[id].CleanUP();
            }
        }

        internal async Task RegistrationTick()
        {
            // Entity visibility must not wait for the slower state-refresh interval.
            await ELS.Delay(250);
            ReconcileVisibleVehicles();
            Vehicle current = Game.PlayerPed.CurrentVehicle;
            if (unchecked(Game.GameTime - _lastStateBroadcast) >= Math.Max(1, Global.RegistrationDelay) * 1000 &&
                current != null && current.Driver == Game.PlayerPed && vehicleList.ContainsKey(current.GetElsId()))
            {
                _lastStateBroadcast = Game.GameTime;
                SyncRequestReply(Commands.MainSiren, current.GetElsId(), Game.Player.ServerId);
            }
        }

        private void ReconcileVisibleVehicles()
        {
            foreach (int id in new List<int>(vehicleList.Keys))
            {
                if (!IsBound(id, vehicleList[id])) RemoveVehicle(id, false);
            }
            bool needsSnapshot = false;
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                if (!vehicle.Exists() || !vehicle.IsEls()) continue;
                int id = vehicle.GetElsId();
                if (id <= 0 || vehicleList.ContainsKey(id)) continue;
                try
                {
                    ELSVehicleFSData data;
                    if (_pendingSync.TryGetValue(id, out data))
                        vehicleList.Add(vehicle.Handle, data);
                    else if (vehicle.Driver == Game.PlayerPed && _confirmedNew.Contains(id))
                        vehicleList.Add(vehicle.Handle);
                    else
                        needsSnapshot = true; // An observer must never initialize an unknown car to "off".
                }
                catch (Exception ex)
                {
                    Utils.ReleaseWriteLine($"Could not register ELS vehicle {id}: {ex.Message}");
                }
            }
            if (needsSnapshot && unchecked(Game.GameTime - _lastSnapshotRequest) >= 1000)
            {
                _lastSnapshotRequest = Game.GameTime;
                ELS.TriggerServerEvent("ELS:FullSync:Request:All");
            }
        }

        internal async Task ControlTickAsync()
        {
            if (Game.PlayerPed.CurrentVehicle != null && Game.PlayerPed.CurrentVehicle.IsEls())
            {
                if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed)
                {
                    int id = Game.PlayerPed.CurrentVehicle.GetElsId();
                    ELSVehicle managed;
                    if (vehicleList.TryGetValue(id, out managed) && IsBound(id, managed) &&
                        managed.GetVehicle.Handle == Game.PlayerPed.CurrentVehicle.Handle)
                        managed.RunControlTick();
                }
            }
        }



        internal async Task RunTickAsync()
        {
            foreach (int id in new List<int>(vehicleList.Keys))
            {
                ELSVehicle managed = vehicleList[id];
                if (!IsBound(id, managed))
                {
                    // Client scope loss only releases local effects. Keep accepted state for re-entry.
                    RemoveVehicle(id, false);
                    continue;
                }
                managed.RunExternalTick();
                if (vehicleList.ContainsKey(id)) managed.RunTick();
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
            if (forgetState)
            {
                _pendingSync.Remove(id);
                _confirmedNew.Remove(id);
            }
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
            _confirmedNew.Clear();
        }

        internal static void SyncUI(int id)
        {
            ELSVehicle vehicle;
            if (vehicleList.TryGetValue(id, out vehicle) && IsBound(id, vehicle)) vehicle.SyncUi();
        }

        internal static void SyncRequestReply(Commands command, int id, int PlayerId)
        {
            if (id <= 0 || !vehicleList.ContainsKey(id) || !IsBound(id, vehicleList[id]))
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
