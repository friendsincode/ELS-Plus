using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Light;
using ELS.Siren;
using ELS_Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static int assertions;
    static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        assertions++;
    }
    static JObject Payload(int id = 100001, int networkId = 42)
    {
        return JObject.FromObject(new
        {
            Id = id, NetworkId = networkId, Light = new Lights().GetData(),
            Siren = new { Dual = false, MainSiren = new { Interrupted = false, CurrentTone = 0, State = true },
                Tones = new { Horn = false, Tone1 = true, Tone2 = false, Tone3 = false, Tone4 = false, Panic = false } }
        });
    }
    static bool Valid(JObject data) => SyncPayload.TryParse(data.ToString(Formatting.None), out _);
    static void Send(ELSServer server, Player player, JObject data) =>
        server.EventHandlers["ELS:FullSync:Broadcast"].DynamicInvoke(player, data.ToString(Formatting.None));

    static async Task Main()
    {
        Check(Valid(Payload()), "Normal payload rejected");
        var bad = Payload(); bad["Siren"] = null; Check(!Valid(bad), "Null siren accepted");
        bad = Payload(); bad["Light"]["SEC"] = null; Check(Valid(bad), "Absent optional extras rejected");
        bad = Payload(); bad["Light"]["Stage"] = 4; Check(!Valid(bad), "Invalid stage accepted");
        bad = Payload(); bad["Siren"]["MainSiren"]["CurrentTone"] = 4; Check(!Valid(bad), "Invalid tone accepted");
        bad = Payload(); bad["NetworkId"] = -1; Check(!Valid(bad), "Invalid network ID accepted");
        bad = Payload(); bad["Id"] = "100001"; Check(!Valid(bad), "String ID accepted");
        bad = Payload(); bad["Light"]["Spotlight"]["Horizontal"] = double.NaN; Check(!Valid(bad), "NaN accepted");
        bad = Payload(); bad["Light"]["WRN"] = new JObject { ["99"] = new JObject() }; Check(!Valid(bad), "Invalid extra accepted");
        Check(!SyncPayload.TryParse(new string('x', 32769), out _), "Oversized payload accepted");
        Check(!SyncPayload.TryParse("{", out _), "Malformed JSON accepted");
        Check(!SyncPayload.TryParse(Payload() + "{}", out _), "Trailing JSON accepted");

        var lights = new Lights();
        Check(lights.GetData().SEC.Count == 0, "Missing optional equipment must serialize");
        var secondary = new ELS.Extra.Extra { Id = 7 };
        lights._extras.SECL[7] = secondary;
        lights.SetData(new LightFSData { SEC = new System.Collections.Generic.Dictionary<int, ELS.Extra.ExtraFSData>
            { [7] = new ELS.Extra.ExtraFSData { TurnedOn = true } }, WRN = null });
        Check(secondary.TurnedOn, "Secondary-only state was not applied");
        lights.SetData(new LightFSData { SEC = null, WRN = new System.Collections.Generic.Dictionary<int, ELS.Extra.ExtraFSData>() });
        lights.SetData(new LightFSData { SEC = new System.Collections.Generic.Dictionary<int, ELS.Extra.ExtraFSData>() });
        Check(secondary.TurnedOn, "Missing extra keys must not overwrite or throw");

        var car = new Vehicle(123); Game.PlayerPed.CurrentVehicle = car;
        var tone = new Tone("SIREN", car, ToneType.SrnTon1, true, null, null);
        tone.SetState(true);
        Check(API.Plays == 1, "Own vehicle siren did not play");
        Check(API.LastSoundSet == null, "Default sound set must be null");
        tone.SetState(true); Check(API.Plays == 1, "Repeated sync restarted playing sound");
        tone.SetState(false); tone.CleanUp(); tone.CleanUp();
        Check(API.Stops == 1 && API.Releases == 1, "Sound must be stopped/released exactly once");
        tone.SetState(true); car.Alive = false; tone.SetState(false);
        Check(API.Releases == 2, "Destroyed entity sound was not released");

        var server = new ELSServer();
        var owner = new Player("1"); var other = new Player("2");
        server.Players.Add(owner); server.Players.Add(other);
        API.Existing.Add(500); API.Network[42] = 500; API.Vehicles[1] = 500; API.Drivers[500] = 1;
        BaseScript.Sent.Clear(); Send(server, other, Payload());
        Check(BaseScript.Sent.Count == 0, "Nonoccupant changed state");
        Send(server, owner, Payload());
        Check(BaseScript.Sent.Single().Event == "ELS:NewFullSyncData", "Owner update rejected");
        Check((int)BaseScript.Sent[0].Args[1] == 1, "Sender was not authenticated");
        BaseScript.Sent.Clear();
        // Transfer ownership and change the transport ID, retaining the ELS decorator ID.
        API.Vehicles.Remove(1); API.Vehicles[2] = 500; API.Drivers[500] = 2;
        API.Network.Remove(42); API.Network[43] = 500;
        Send(server, other, Payload(networkId: 43));
        Check(BaseScript.Sent.Count == 1, "Ownership/network-ID change lost stable ELS identity");
        BaseScript.Sent.Clear(); Send(server, other, Payload(id: 100002, networkId: 43));
        Check(BaseScript.Sent.Count == 0, "Same entity acquired a second ELS identity");
        API.Existing.Add(600); API.Network[44] = 600; API.Vehicles[1] = 600; API.Drivers[600] = 1;
        Send(server, owner, Payload(networkId: 44));
        Check(BaseScript.Sent.Count == 0, "Another vehicle stole a live ELS identity");
        server.EventHandlers["ELS:FullSync:RemoveStale"].DynamicInvoke(owner, 100001);
        Check(BaseScript.Sent.Count == 0, "Client removed a live vehicle's state");
        server.EventHandlers["baseevents:enteredVehicle"].DynamicInvoke(owner, 1234, -1, "POLICE", 44);
        Check(BaseScript.Sent.Single().Target == "1", "Local handle was broadcast to other clients");
        BaseScript.Sent.Clear();
        API.Vehicles.Remove(2); API.Now = 500;
        Send(server, other, Payload(networkId: 43)); Check(BaseScript.Sent.Count == 1, "Immediate exit state rejected");
        BaseScript.Sent.Clear(); API.Now = 4000;
        Send(server, other, Payload(networkId: 43)); Check(BaseScript.Sent.Count == 0, "Expired exit authority accepted");
        API.Existing.Remove(500); await server.RunTick();
        Check(BaseScript.Sent.Any(e => e.Event == "ELS:removeStaleFromList"), "Deleted entity cache was not pruned");
        BaseScript.Sent.Clear();
        // Reusing a network ID must not resurrect the deleted car's ELS state.
        API.Network[43] = 600;
        Send(server, owner, Payload(id: 100003, networkId: 43));
        Check(BaseScript.Sent.Count == 1, "Reused network ID rejected a new independent ELS identity");
        BaseScript.Sent.Clear();
        server.EventHandlers["ELS:FullSync:Request:All"].DynamicInvoke(other);
        Check(BaseScript.Sent.Single().Target == "2", "Snapshot was not targeted to its requester");
        var snapshot = JObject.Parse((string)BaseScript.Sent[0].Args[0]);
        Check(snapshot["100001"] == null && snapshot["100003"] != null, "Snapshot retained deleted vehicle state");
        BaseScript.Sent.Clear(); API.Now = 10000;
        for (int i = 0; i < 45; i++) Send(server, owner, Payload(id: 100003, networkId: 43));
        Check(BaseScript.Sent.Count == 40, "State rate limit did not cap flooding");
        Check(API.Restricted["clearcache"] && API.Restricted["resync"] && API.Restricted["vcfrefresh"], "Admin commands unrestricted");
        Check(server.EventHandlers["ELS:FullSync:Broadcast"].Method.GetParameters()[0]
            .IsDefined(typeof(FromSourceAttribute), false), "Sender parameter is not marked FromSource");
        Console.WriteLine($"Passed {assertions} regression checks (native calls simulated).");
    }
}
