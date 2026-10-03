using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using ELS;
using ELS.Manager;
using Newtonsoft.Json;

class Program
{
    static int assertions;
    static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
        assertions++;
    }
    static string State(int id, int networkId, int stage) => JsonConvert.SerializeObject(new ELSVehicleFSData
        { Id = id, NetworkId = networkId, Stage = stage });
    static EntityState Add(int handle, int id, int netId)
    {
        var entity = new EntityState { ElsId = id, NetworkId = netId };
        Vehicle.Entities[handle] = entity;
        return entity;
    }
    static void Reset()
    {
        Vehicle.Entities.Clear(); ELS.ELS.Sent.Clear(); Game.PlayerPed.CurrentVehicle = null; Game.GameTime = 1000;
    }

    static async Task Main()
    {
        Reset(); var manager = new VehicleManager();
        // State may arrive before GTA creates the remote clone.
        manager.SetVehicleSyncData(State(1001, 42, 3), 2);
        Check(VehicleManager.vehicleList.Count == 0, "Out-of-scope state created a vehicle wrapper");
        Add(10, 1001, 42);
        await manager.RegistrationTick();
        var first = VehicleManager.vehicleList[1001];
        Check(first.ConstructedWithState && first.State.Stage == 3, "Stream-in lost cached lights/siren state");
        Check(ELS.ELS.LastDelay == 250, "Stream-in still waits for the 15-second heartbeat");
        ELS.ELS.Sent.Clear();
        Vehicle.Entities.Remove(10);
        await manager.RunTickAsync();
        Check(first.Disposals == 1 && VehicleManager.vehicleList.Count == 0, "Stream-out did not release local effects promptly");
        Check(!ELS.ELS.Sent.Any(e => e.Name.Contains("Remove")), "Client scope loss deleted global state");
        manager.SetVehicleSyncData(State(1001, 42, 2), 2);
        Add(20, 1001, 42);
        await manager.RegistrationTick();
        var second = VehicleManager.vehicleList[1001];
        Check(second.GetVehicle.Handle == 20 && second.State.Stage == 2, "New clone handle did not restore latest out-of-scope update");
        await manager.RegistrationTick();
        Check(ReferenceEquals(second, VehicleManager.vehicleList[1001]), "Repeated scans recreated a live wrapper");

        // A clone can disappear/reappear between scans and with a new network owner/ID.
        Vehicle.Entities.Remove(20); Add(30, 1001, 43);
        await manager.RegistrationTick();
        Check(second.Disposals == 1 && VehicleManager.vehicleList[1001].GetVehicle.Handle == 30,
            "Rapid re-entry retained old local handle");
        Check(VehicleManager.vehicleList[1001].State.Stage == 2, "Owner/network-ID change reset stable ELS state");

        // A reused client handle with a different decorator must not receive old controls or ticks.
        var stale = VehicleManager.vehicleList[1001];
        Vehicle.Entities[30].ElsId = 2002;
        Vehicle.Entities[30].Driver = Game.PlayerPed;
        Game.PlayerPed.CurrentVehicle = new Vehicle(30);
        await manager.ControlTickAsync(); VehicleManager.SyncUI(1001);
        VehicleManager.SyncRequestReply(ELS.RemoteEventManager.Commands.MainSiren, 1001, 1);
        Check(stale.ControlTicks == 0 && stale.UiSyncs == 0, "Reused handle received stale controls/UI");
        await manager.RunTickAsync();
        Check(stale.Disposals == 1 && stale.Ticks == 0, "Reused handle retained old local effects");

        // Delayed decorators are normal replication ordering, not a reason to bind by net ID alone.
        Reset(); manager = new VehicleManager(); Add(40, 0, 42);
        manager.SetVehicleSyncData(State(3003, 42, 3), 2);
        await manager.RegistrationTick();
        Check(VehicleManager.vehicleList.Count == 0, "Bound before stable decorator arrived");
        Vehicle.Entities[40].ElsId = 3003;
        await manager.RegistrationTick();
        Check(VehicleManager.vehicleList[3003].State.Stage == 3, "Delayed decorator never recovered cached state");
        manager.RemoveVehicle(3003);
        Vehicle.Entities.Remove(40); Add(50, 4004, 42);
        await manager.RegistrationTick();
        Check(!VehicleManager.vehicleList.ContainsKey(4004), "Recycled net ID inherited previous car's state");
        Check(ELS.ELS.Sent.Any(e => e.Name == "ELS:FullSync:Request:All"), "Missing observer state did not request a snapshot");
        manager.SyncAllVehiclesOnFirstSpawn(JsonConvert.SerializeObject(new Dictionary<int, string> { [4004] = State(4004, 42, 1) }));
        Check(VehicleManager.vehicleList[4004].State.Stage == 1, "Snapshot did not initialize a visible observer car");

        // Driver identity is restored by an authorized server response, never invented on migration.
        Reset(); manager = new VehicleManager(); var own = Add(60, 0, 77);
        own.Driver = Game.PlayerPed; Game.PlayerPed.CurrentVehicle = new Vehicle(60);
        await manager.ElsIDSetTick();
        Check(ELS.ELS.Sent.Count == 0, "Client without control attempted identity creation");
        own.HasControl = true; await manager.ElsIDSetTick();
        Check(ELS.ELS.Sent.Single().Name == "ELS:FullSync:Resolve" && own.ElsId == 0, "Driver invented identity before resolution");
        manager.AcceptIdentity(59, 77, 5005, State(5005, 77, 3));
        Check(own.ElsId == 0, "Old instance's identity response applied to current car");
        manager.AcceptIdentity(60, 78, 5005, State(5005, 77, 3));
        Check(own.ElsId == 0, "Mismatched network identity response applied");
        manager.AcceptIdentity(60, 77, 5005, State(5005, 77, 3));
        await manager.RegistrationTick();
        Check(own.ElsId == 5005 && VehicleManager.vehicleList[5005].State.Stage == 3, "Resolved identity lost existing state");
        ELS.ELS.Sent.Clear(); await manager.ElsIDSetTick();
        Check(ELS.ELS.Sent.Count == 0, "Existing stable identity was regenerated");
        var ownedWrapper = VehicleManager.vehicleList[5005];
        manager.ClearVehicles();
        Check(ownedWrapper.Disposals == 1 && VehicleManager.vehicleList.Count == 0, "Resource/cache cleanup left effects alive");
        // An incoming driver must not overwrite existing state while its snapshot is in flight.
        Reset(); manager = new VehicleManager(); own = Add(70, 6006, 88);
        own.Driver = Game.PlayerPed; own.HasControl = true;
        Game.PlayerPed.CurrentVehicle = new Vehicle(70);
        await manager.RegistrationTick();
        Check(VehicleManager.vehicleList.Count == 0, "Incoming driver initialized unknown state to off");
        await manager.ElsIDSetTick();
        Check(ELS.ELS.Sent.Any(e => e.Name == "ELS:FullSync:Resolve"), "Incoming driver did not resolve authoritative state");
        manager.AcceptIdentity(70, 88, 6006, State(6006, 88, 3));
        await manager.RegistrationTick();
        Check(VehicleManager.vehicleList[6006].State.Stage == 3, "Driver handoff lost active state");
        manager.ClearVehicles();
        manager.AcceptIdentity(70, 88, 7007, null);
        await manager.RegistrationTick();
        Check(VehicleManager.vehicleList.ContainsKey(7007) && !VehicleManager.vehicleList[7007].ConstructedWithState,
            "Server-confirmed new car could not initialize defaults");
        Console.WriteLine($"Passed {assertions} streaming checks (actual manager; simulated entities/effects).");
    }
}
