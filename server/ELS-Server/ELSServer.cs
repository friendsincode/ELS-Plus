using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ELS_Server
{
    public class ELSServer : BaseScript
    {
        private readonly Dictionary<int, string> _cachedData = new Dictionary<int, string>();
        // Bind stable ELS identity to a server entity, not to its owner or client-local handle.
        private readonly Dictionary<int, int> _entities = new Dictionary<int, int>();
        private readonly Dictionary<string, Tuple<int, int>> _recentControls = new Dictionary<string, Tuple<int, int>>();
        private readonly Dictionary<string, Tuple<int, int>> _rates = new Dictionary<string, Tuple<int, int>>();

        public ELSServer()
        {
            Utils.ReleaseWriteLine($"Welcome to ELS+ {Assembly.GetExecutingAssembly().GetName().Version} for FiveM");
            string serverId = API.LoadResourceFile(API.GetCurrentResourceName(), "ELSId");
            if (String.IsNullOrEmpty(serverId))
            {
                serverId = Guid.NewGuid().ToString();
                API.SaveResourceFile(API.GetCurrentResourceName(), "ELSId", serverId, -1);
            }
            API.SetConvarReplicated("ELSServerId", serverId);
            foreach (string group in Configuration.ElsVehicleGroups)
                API.ExecuteCommand($"add_ace group.{group} command.elscar allow");

            EventHandlers["ELS:VcfSync:Server"] += new Action<Player>(SendVcf);
            EventHandlers["ELS:FullSync:Resolve"] += new Action<Player, int, int>(ResolveIdentity);
            EventHandlers["entityRemoved"] += new Action<string, int>(EntityRemoved);
            EventHandlers["ELS:FullSync:Request:All"] += new Action<Player>(SendSnapshot);
            EventHandlers["ELS:FullSync:Broadcast"] += new Action<Player, string>(ReceiveState);
            EventHandlers["ELS:FullSync:RemoveStale"] += new Action<Player, int>(RemoveStale);
            EventHandlers["baseevents:enteredVehicle"] += new Action<Player, int, int, string, int>(VehicleEntered);
            EventHandlers["baseevents:leftVehicle"] += new Action<Player, int, int, string, int>(VehicleExited);
            EventHandlers["playerDropped"] += new Action<Player, string>(PlayerDropped);

            API.RegisterCommand("elscar", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (source <= 0 || args.Count == 0) return;
                TriggerClientEvent(Players[source], "ELS:SpawnCar", args[0].ToString());
            }), Configuration.ElsCarAdminOnly);
            API.RegisterCommand("vcfrefresh", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                await PreloadSyncData();
                foreach (Player player in Players) SendVcfData(player);
            }), true);
            API.RegisterCommand("resync", new Action<int, List<object>, string>((source, args, raw) =>
            {
                int target = source;
                if (args.Count > 0 && !int.TryParse(args[0].ToString(), out target)) return;
                if (target > 0 && Players[target] != null) SendSnapshotData(Players[target]);
            }), true);
            API.RegisterCommand("clearcache", new Action<int, List<object>, string>((source, args, raw) =>
            {
                _cachedData.Clear();
                _entities.Clear();
                _recentControls.Clear();
                TriggerClientEvent("ELS:clearVehicleList");
            }), true);
            Tick += MaintainVehicles;
            // These loaders complete synchronously; finish before serving player requests.
            PreloadSyncData().GetAwaiter().GetResult();
        }

        private async Task PreloadSyncData()
        {
            await VcfSync.CheckResources();
            await CustomPatterns.CheckCustomPatterns();
        }

        private bool AllowRequest(Player player, string kind, int limit)
        {
            if (player == null || String.IsNullOrEmpty(player.Handle)) return false;
            string key = player.Handle + ":" + kind;
            int now = unchecked((int)API.GetGameTimer());
            Tuple<int, int> rate;
            if (!_rates.TryGetValue(key, out rate) || unchecked(now - rate.Item1) >= 1000)
                rate = Tuple.Create(now, 0);
            if (rate.Item2 >= limit) return false;
            _rates[key] = Tuple.Create(rate.Item1, rate.Item2 + 1);
            return true;
        }

        private void SendVcf([FromSource] Player player)
        {
            if (AllowRequest(player, "vcf", 1)) SendVcfData(player);
        }

        private void SendVcfData(Player player)
        {
            TriggerClientEvent(player, "ELS:VcfSync:Client", VcfSync.VcfData);
            TriggerClientEvent(player, "ELS:PatternSync:Client", CustomPatterns.Patterns);
        }

        private void SendSnapshot([FromSource] Player player)
        {
            if (AllowRequest(player, "snapshot", 2)) SendSnapshotData(player);
        }

        private void SendSnapshotData(Player player)
        {
            TriggerClientEvent(player, "ELS:FullSync:NewSpawnWithData", JsonConvert.SerializeObject(_cachedData));
        }

        private int ControlledVehicle(Player player)
        {
            int ped = API.GetPlayerPed(player.Handle);
            if (ped == 0) return 0;
            int vehicle = API.GetVehiclePedIsIn(ped, false);
            if (vehicle == 0 || !API.DoesEntityExist(vehicle)) return 0;
            return API.GetPedInVehicleSeat(vehicle, -1) == ped || API.GetPedInVehicleSeat(vehicle, 0) == ped ? vehicle : 0;
        }

        private void ResolveIdentity([FromSource] Player player, int networkId, int requestHandle)
        {
            if (!AllowRequest(player, "identity", 2)) return;
            int entity = API.NetworkGetEntityFromNetworkId(networkId);
            if (entity == 0 || ControlledVehicle(player) != entity) return;
            int id = 0;
            foreach (var entry in _entities)
                if (entry.Value == entity) { id = entry.Key; break; }
            if (id == 0)
            {
                do { id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0) & int.MaxValue; }
                while (id == 0 || _entities.ContainsKey(id));
                _entities[id] = entity;
            }
            string state;
            _cachedData.TryGetValue(id, out state);
            TriggerClientEvent(player, "ELS:FullSync:Identity", requestHandle, networkId, id, state);
        }

        private void EntityRemoved([FromSource] string source, int entity)
        {
            // C# also exposes event handlers over the network. Accept only the engine's local event.
            if (!String.IsNullOrEmpty(source)) return;
            foreach (var entry in new List<KeyValuePair<int, int>>(_entities))
                if (entry.Value == entity) RemoveVehicle(entry.Key);
            foreach (var entry in new List<KeyValuePair<string, Tuple<int, int>>>(_recentControls))
                if (entry.Value.Item1 == entity) _recentControls.Remove(entry.Key);
        }

        private void ReceiveState([FromSource] Player player, string json)
        {
            if (!AllowRequest(player, "state", 40)) return;
            JObject data;
            if (!SyncPayload.TryParse(json, out data)) return;
            int id = (int)data["Id"];
            int entity = API.NetworkGetEntityFromNetworkId((int)data["NetworkId"]);
            if (entity == 0 || !API.DoesEntityExist(entity) || API.GetEntityType(entity) != 2) return;
            int now = unchecked((int)API.GetGameTimer());
            if (ControlledVehicle(player) == entity)
                _recentControls[player.Handle] = Tuple.Create(entity, now);
            else
            {
                // Permit the authenticated previous occupant's immediate siren-off/exit update.
                Tuple<int, int> recent;
                if (!_recentControls.TryGetValue(player.Handle, out recent) || recent.Item1 != entity ||
                    unchecked(now - recent.Item2) > 3000) return;
            }
            int previous;
            if (_entities.TryGetValue(id, out previous) && previous != entity && API.DoesEntityExist(previous)) return;
            foreach (var entry in _entities)
                if (entry.Value == entity && entry.Key != id) return;
            _entities[id] = entity;
            _cachedData[id] = json;
            TriggerClientEvent("ELS:NewFullSyncData", json, int.Parse(player.Handle));
        }

        private void RemoveStale([FromSource] Player player, int id)
        {
            if (!AllowRequest(player, "remove", 10)) return;
            int entity;
            // Clients cannot delete another car's cached state while that car is alive.
            if (_entities.TryGetValue(id, out entity) && (!API.DoesEntityExist(entity) || API.GetEntityHealth(entity) <= 0))
                RemoveVehicle(id);
        }

        private void RemoveVehicle(int id)
        {
            _entities.Remove(id);
            _cachedData.Remove(id);
            TriggerClientEvent("ELS:removeStaleFromList", id);
        }

        private void VehicleEntered([FromSource] Player player, int handle, int seat, string name, int netId)
        {
            if ((seat != -1 && seat != 0) || !AllowRequest(player, "entry", 4)) return;
            int entity = ControlledVehicle(player);
            if (entity != 0) _recentControls[player.Handle] = Tuple.Create(entity, unchecked((int)API.GetGameTimer()));
            // The handle came from this client, so only send it back to that client.
            TriggerClientEvent(player, "ELS:VehicleEntered", handle);
        }

        private void VehicleExited([FromSource] Player player, int handle, int seat, string name, int netId)
        {
            if ((seat == -1 || seat == 0) && AllowRequest(player, "exit", 4))
                TriggerClientEvent(player, "ELS:VehicleExited", handle);
        }

        private void PlayerDropped([FromSource] Player player, string reason)
        {
            _recentControls.Remove(player.Handle);
            foreach (string key in new List<string>(_rates.Keys))
                if (key.StartsWith(player.Handle + ":", StringComparison.Ordinal)) _rates.Remove(key);
        }

        private async Task MaintainVehicles()
        {
            await Delay(500);
            foreach (Player player in Players)
            {
                int entity = ControlledVehicle(player);
                if (entity != 0) _recentControls[player.Handle] = Tuple.Create(entity, unchecked((int)API.GetGameTimer()));
            }
            foreach (var entry in new List<KeyValuePair<int, int>>(_entities))
                if (!API.DoesEntityExist(entry.Value) || API.GetEntityHealth(entry.Value) <= 0) RemoveVehicle(entry.Key);
        }
    }
}
