using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitizenFX.Core
{
    public enum VehicleSeat { Driver = -1, Passenger = 0 }
    public enum VehicleClass { Emergency, Boats, Trains, Planes, Helicopters }
    public class Ped { public Vehicle CurrentVehicle; }
    public class Player { public int ServerId = 1; }
    public static class Game
    {
        public static Ped PlayerPed = new Ped();
        public static Player Player = new Player();
        public static int GameTime;
    }
    public class EntityState
    {
        public int ElsId, NetworkId;
        public bool IsEls = true, Visible = true, HasControl;
        public Ped Driver, Passenger;
    }
    public class Vehicle
    {
        public static readonly Dictionary<int, EntityState> Entities = new Dictionary<int, EntityState>();
        public int Handle { get; }
        public Vehicle(int handle) { Handle = handle; }
        private EntityState State => Entities.TryGetValue(Handle, out var state) ? state : null;
        public bool Exists() => State != null && State.Visible;
        public int GetElsId() => State?.ElsId ?? 0;
        public bool IsEls() => State?.IsEls ?? false;
        public Ped Driver => State?.Driver;
        public Ped GetPedOnSeat(VehicleSeat seat) => seat == VehicleSeat.Driver ? State?.Driver : State?.Passenger;
        public VehicleClass ClassType => VehicleClass.Emergency;
    }
    public static class World
    {
        public static Vehicle[] GetAllVehicles() => Vehicle.Entities.Where(e => e.Value.Visible).Select(e => new Vehicle(e.Key)).ToArray();
    }
    public static class Debug { public static void WriteLine(string message) { } }
}
namespace CitizenFX.Core.Native
{
    public static class API
    {
        public static bool NetworkGetEntityIsNetworked(int handle) => NetworkGetNetworkIdFromEntity(handle) > 0;
        public static int NetworkGetNetworkIdFromEntity(int handle) => CitizenFX.Core.Vehicle.Entities.TryGetValue(handle, out var e) ? e.NetworkId : 0;
        public static bool NetworkDoesEntityExistWithNetworkId(int id) => CitizenFX.Core.Vehicle.Entities.Values.Any(e => e.Visible && e.NetworkId == id);
        public static bool NetworkHasControlOfEntity(int handle) => CitizenFX.Core.Vehicle.Entities.TryGetValue(handle, out var e) && e.HasControl;
        public static void NetworkRegisterEntityAsNetworked(int handle) { }
        public static void SetEntityAsMissionEntity(int handle, bool a, bool b) { }
        public static void SetNetworkIdCanMigrate(int id, bool value) { }
        public static void DecorSetInt(int handle, string key, int value) => CitizenFX.Core.Vehicle.Entities[handle].ElsId = value;
    }
}
namespace ELS
{
    public static class ELS
    {
        public static readonly List<(string Name, object[] Args)> Sent = new List<(string, object[])>();
        public static int LastDelay;
        public static Task Delay(int milliseconds) { LastDelay = milliseconds; return Task.CompletedTask; }
        public static void TriggerServerEvent(string name, params object[] args) => Sent.Add((name, args));
    }
    public static class Utils
    {
        public static void DebugWriteLine(string message) { }
        public static void ReleaseWriteLine(string message) { }
    }
    public static class Debug { public static void DebugText() { } }
    public static class RemoteEventManager { public enum Commands { MainSiren, ToggleInd } }
    public struct ELSVehicleFSData
    {
        public int Id, NetworkId, Stage;
    }
    // Local effects are simulated; the actual VehicleManager and VehicleList are under test.
    public class ELSVehicle
    {
        public readonly CitizenFX.Core.Vehicle GetVehicle;
        public ELSVehicleFSData State;
        public int Disposals, ControlTicks, UiSyncs, Ticks;
        public bool ConstructedWithState;
        public ELSVehicle(int handle, ELSVehicleFSData? data = null)
        {
            GetVehicle = new CitizenFX.Core.Vehicle(handle);
            State = data ?? new ELSVehicleFSData { Id = GetVehicle.GetElsId() };
            ConstructedWithState = data.HasValue;
        }
        public bool Exists() => GetVehicle.Exists();
        public void Dispose() => Disposals++;
        public void CleanUP() { }
        public void RunControlTick() => ControlTicks++;
        public void RunExternalTick() { }
        public void RunTick() => Ticks++;
        public void SyncUi() => UiSyncs++;
        public void SetData(ELSVehicleFSData data) => State = data;
        public ELSVehicleFSData GetData() => State;
    }
}
namespace ELS.FullSync
{
    public static class FullSyncManager
    {
        public static void SendDataBroadcast(string json, int player) => ELS.TriggerServerEvent("ELS:FullSync:Broadcast", json);
    }
}
namespace ELS.configuration { public static class Global { public static int RegistrationDelay = 15; } }
namespace ELS.Light
{
    public static class Indicator
    {
        public static void RunAsync(CitizenFX.Core.Vehicle vehicle) { }
        public static int CurrentIndicatorState(CitizenFX.Core.Vehicle vehicle) => 0;
    }
}
