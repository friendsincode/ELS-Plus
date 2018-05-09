using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS_Server
{
    public class Class1 : BaseScript
    {
        Dictionary<int, object> _cachedData = new Dictionary<int, object>();
        long GameTimer;
        public Class1()
        {
            Utils.ReleaseWriteLine("Welcome to ELS+ for FiveM");
            GameTimer = API.GetGameTimer();
            Utils.ReleaseWriteLine($"Setting Game time is {GameTimer}");
            foreach(string s in Configuration.ElsVehicleGroups)
            {
                API.ExecuteCommand($"add_ace group.{s} command.elscar allow");
            }
            API.RegisterCommand("vcfrefresh", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                Utils.ReleaseWriteLine($"{Players[source].Name} has activated a VCF Refresh");
                foreach (Player p in Players)
                {
                    TriggerEvent("ELS:VcfSync:Server", p.Handle);
                }
            }), false);

            API.RegisterCommand("elscar", new Action<int, List<object>, string>(async (source, arguments, raw) =>
            {
                if (arguments.Count < 1 || String.IsNullOrEmpty(arguments[0].ToString()))
                {
                    Utils.ReleaseWriteLine("No vehicle specified please try again");
                    return;
                }
                Utils.ReleaseWriteLine($"{Players[source].Name} has attempted to spawn {arguments[0]}");
                TriggerClientEvent(Players[source], "ELS:SpawnCar", arguments[0]);
            }), Configuration.ElsCarAdminOnly);

            EventHandlers["ELS:VcfSync:Server"] += new Action<int>(async (int source) =>
            {
                Utils.DebugWriteLine($"Sending Data to {Players[source].Name}"); 
                TriggerClientEvent(Players[source], "ELS:VcfSync:Client", VcfSync.VcfData);
                TriggerClientEvent(Players[source], "ELS:PatternSync:Client", CustomPatterns.Patterns);

            });

            EventHandlers["ELS:FullSync:RemoveStale"] += new Action<int>(async (int netid) =>
            {
                _cachedData.Remove(netid);
                Utils.DebugWriteLine($"Stale vehicle {netid} removed from cache");
            });

            EventHandlers["baseevents:enteredVehicle"] += new Action<int,int,string>((veh,seat,name) =>
            {
                Utils.DebugWriteLine("Vehicle Entered");
                TriggerClientEvent("ELS:VehicleEntered", veh);
            });
            EventHandlers["ELS:FullSync:Unicast"] += new Action(() => { });
            EventHandlers["ELS:FullSync:Broadcast"] += new Action<System.Dynamic.ExpandoObject, Int16>((dataDic, playerID) =>
             {
                 var dd = (IDictionary<string, object>)dataDic;
                 Utils.DebugWriteLine($"NetworkID {dd["NetworkID"]}");
                 _cachedData[int.Parse(dd["NetworkID"].ToString())] = dd;
                 BroadcastMessage(dataDic, playerID);
             });
            EventHandlers["ELS:FullSync:Request:All"] += new Action<int>((int source) =>
            {
                Utils.DebugWriteLine($"{source} is requesting Sync Data");
                TriggerClientEvent(Players[source], "ELS:FullSync:NewSpawnWithData", _cachedData);
            });
            API.RegisterCommand("resync", new Action<int, System.Collections.IList, string>((a, b, c) =>
            {
                Utils.DebugWriteLine($"{a}, {b}, {c}");
                TriggerClientEvent(Players[(int.Parse((string)b[0]))], "ELS:FullSync:NewSpawnWithData", _cachedData);
            }), false);
            API.RegisterCommand("clearcache", new Action<int, System.Collections.IList, string>((a, b, c) =>
            {
                _cachedData.Clear();
                Utils.ReleaseWriteLine("ELS Cache cleared");
            }),false);

            PreloadSyncData();
            Tick += Server_Tick;
        }

        async Task PreloadSyncData()
        {
            await VcfSync.CheckResources();
            await CustomPatterns.CheckCustomPatterns();
        }

        void BroadcastMessage(System.Dynamic.ExpandoObject dataDic, int SourcePlayerID)
        {
            TriggerClientEvent("ELS:NewFullSyncData", dataDic,SourcePlayerID);
        }

        private async Task Server_Tick()
        {
            if (API.GetGameTimer() >= GameTimer + Configuration.CacheClear)
            {
                _cachedData.Clear();
                GameTimer = API.GetGameTimer();
            }
        }
    }
}
