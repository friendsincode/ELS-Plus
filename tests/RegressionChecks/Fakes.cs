using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CitizenFX.Core
{
    [AttributeUsage(AttributeTargets.Parameter)] public class FromSourceAttribute : Attribute { }
    public class Player { public string Handle; public Player(string handle) { Handle = handle; } }
    public class PlayerList : List<Player> { public new Player this[int id] => Find(p => p.Handle == id.ToString()); }
    public class HandlerEntry
    {
        public Delegate Callback;
        public System.Reflection.MethodInfo Method => Callback.Method;
        public object DynamicInvoke(params object[] args) => Callback.DynamicInvoke(args);
        public static HandlerEntry operator +(HandlerEntry entry, Delegate callback) =>
            new HandlerEntry { Callback = Delegate.Combine(entry?.Callback, callback) };
    }
    public class Handlers
    {
        private readonly Dictionary<string, HandlerEntry> entries = new Dictionary<string, HandlerEntry>();
        public HandlerEntry this[string key] { get => entries.TryGetValue(key, out var d) ? d : null; set => entries[key] = value; }
    }
    public class BaseScript
    {
        public readonly Handlers EventHandlers = new Handlers();
        public readonly PlayerList Players = new PlayerList();
        public event Func<Task> Tick;
        public static readonly List<(string Target, string Event, object[] Args)> Sent = new List<(string, string, object[])>();
        public static void TriggerClientEvent(string name, params object[] args) => Sent.Add(("all", name, args));
        public static void TriggerClientEvent(Player player, string name, params object[] args) => Sent.Add((player.Handle, name, args));
        public static Task Delay(int milliseconds) => Task.CompletedTask;
        public Task RunTick() => Tick?.Invoke() ?? Task.CompletedTask;
    }
    public class Vehicle
    {
        public int Handle;
        public bool IsSirenActive;
        public bool Alive = true;
        public Vehicle(int handle) { Handle = handle; }
        public bool Exists() => Alive;
        public int GetElsId() => Handle;
    }
    public class Ped { public Vehicle CurrentVehicle; }
    public static class Game { public static Ped PlayerPed = new Ped(); }
    public static class Audio
    {
        public static bool HasSoundFinished(int id) => false;
        public static void StopSound(int id) => Native.API.Stops++;
        public static void ReleaseSound(int id) => Native.API.Releases++;
    }
}
namespace CitizenFX.Core.Native
{
    public static class API
    {
        public static int Now, Plays, Stops, Releases;
        public static string LastSoundSet;
        public static readonly Dictionary<int, int> Network = new Dictionary<int, int>();
        public static readonly Dictionary<int, int> Vehicles = new Dictionary<int, int>();
        public static readonly Dictionary<int, int> Drivers = new Dictionary<int, int>();
        public static readonly Dictionary<int, int> Passengers = new Dictionary<int, int>();
        public static readonly HashSet<int> Existing = new HashSet<int>();
        public static readonly Dictionary<string, bool> Restricted = new Dictionary<string, bool>();
        public static long GetGameTimer() => Now;
        public static int GetPlayerPed(string player) => int.Parse(player);
        public static int GetVehiclePedIsIn(int ped, bool last) => Vehicles.TryGetValue(ped, out var v) ? v : 0;
        public static int GetPedInVehicleSeat(int vehicle, int seat) => (seat == -1 ? Drivers : Passengers).TryGetValue(vehicle, out var p) ? p : 0;
        public static bool DoesEntityExist(int entity) => Existing.Contains(entity);
        public static int GetEntityHealth(int entity) => 100;
        public static int GetEntityType(int entity) => 2;
        public static int NetworkGetEntityFromNetworkId(int id) => Network.TryGetValue(id, out var e) ? e : 0;
        public static string GetCurrentResourceName() => "els-plus";
        public static string LoadResourceFile(string resource, string path) => "test-server";
        public static void SaveResourceFile(string resource, string path, string data, int length) { }
        public static void SetConvarReplicated(string key, string value) { }
        public static void ExecuteCommand(string command) { }
        public static void RegisterCommand(string name, Delegate callback, bool restricted) => Restricted[name] = restricted;
        public static int GetSoundId() => 1;
        public static void PlaySoundFromEntity(int id, string sound, int entity, string set, bool networked, int range)
        { Plays++; LastSoundSet = set; }
        public static void RequestScriptAudioBank(string bank, bool networked) { }
    }
}
namespace ELS_Server
{
    public static class Utils { public static void ReleaseWriteLine(string text) { } }
    public static class Configuration
    {
        public static List<string> ElsVehicleGroups = new List<string>();
        public static bool ElsCarAdminOnly = true;
    }
    public static class VcfSync { public static object VcfData = new object(); public static Task CheckResources() => Task.CompletedTask; }
    public static class CustomPatterns { public static object Patterns = new object(); public static Task CheckCustomPatterns() => Task.CompletedTask; }
}
namespace ELS
{
    public static class Utils { public static void DebugWriteLine(string text) { } }
}
namespace ELS.configuration { public static class Global { public static List<string> RegisteredSoundBanks = new List<string>(); } }
namespace ELS.FullSync { public interface IFullSyncComponent<T> { T GetData(); void SetData(T data); } }
namespace ELS.Extra
{
    internal partial class Extra
    {
        public int Id, PatternNum;
        private int _Id => Id;
        public bool IsPatternRunning, TurnedOn;
    }
}
namespace ELS.Board
{
    public struct ArrowBoardFSData { public bool Raised { get; set; } }
    internal class ArrowBoard
    {
        public ArrowBoardFSData GetData() => default;
        public void SetData(ArrowBoardFSData data) { }
    }
}
namespace ELS.Light
{
    public struct SceneFSData { public bool TurnedOn { get; set; } }
    public struct SpotLightFSData { public float Horizontal { get; set; } public float Vertical { get; set; } public bool TurnedOn { get; set; } }
    internal class Scene { public SceneFSData GetData() => default; public void SetData(SceneFSData data) { } }
    internal class SpotLight { public SpotLightFSData GetData() => default; public void SetData(SpotLightFSData data) { } }
    internal class Stage { public int CurrentStage; public void SetStage(int stage) => CurrentStage = stage; }
    internal class Extras
    {
        public Dictionary<int, Extra.Extra> PRML = new Dictionary<int, Extra.Extra>();
        public Dictionary<int, Extra.Extra> SECL = new Dictionary<int, Extra.Extra>();
        public Dictionary<int, Extra.Extra> WRNL = new Dictionary<int, Extra.Extra>();
        public Extra.Extra SBRN, SCL, TDL;
        public Board.ArrowBoard BRD;
    }
    internal partial class Lights
    {
        internal CitizenFX.Core.Vehicle _vehicle = new CitizenFX.Core.Vehicle(1);
        internal void SetGTASirens(bool enabled) => _vehicle.IsSirenActive = enabled;
        internal Extras _extras = new Extras();
        internal Stage _stage = new Stage();
        internal Scene scene;
        internal SpotLight spotLight;
        internal int CurrentPrmPattern, CurrentSecPattern, CurrentWrnPattern;
    }
}
