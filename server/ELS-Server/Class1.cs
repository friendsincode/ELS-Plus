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
        Dictionary<int, object> _cachedData = new Dictionary<int,object>();
        VcfSync _vcfSync;
        public Class1()
        {
            API.ExecuteCommand("add_ace group.admin command.elscar allow");
            API.ExecuteCommand("add_ace group.superadmin command.elscar allow");
            Debug.WriteLine("Welcome to ELS for FiveM");

            API.RegisterCommand("vcfrefresh", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                Debug.WriteLine($"{Players[source].Name} has activated a VCF Refresh");
                foreach (Player p in Players) {
                    TriggerEvent("ELS:VcfSync:Server", p.Handle);
                }
            }), false);

            API.RegisterCommand("elscar", new Action<int, List<object>, string>(async (source, arguments, raw) =>
            {
                Debug.WriteLine($"{Players[source].Name} has attempted to spawn {arguments[0]}");
                TriggerClientEvent(Players[source], "ELS:SpawnCar", arguments[0]);
            }), true);
           
            EventHandlers["ELS:VcfSync:Server"] += new Action<int>((int source) =>
            {
                _vcfSync = new VcfSync();
                _vcfSync.CheckVCF(Players[source]);
            });
            EventHandlers.Add("onResourceStart", new Action<string>((resource) =>
            {
                if (VcfSync.ElsResources.Exists(res => res.Equals(resource)))
                {
                    foreach (Player p in Players)
                    {
                        VcfSync.LoadFilesPromScript(resource, p);
                    }
                }
            }));
            EventHandlers["ELS:FullSync:Unicast"] += new Action(() => { });
            EventHandlers["ELS:FullSync:Broadcast"] += new Action<System.Dynamic.ExpandoObject,int>((dataDic,playerID) =>
            {
                var dd = (IDictionary<string,object> )dataDic;
#if DEBUG
                Debug.WriteLine($"NetworkID {dd["NetworkID"]}");
#endif
                _cachedData[(int)dd["NetworkID"]] = dd;
                BroadcastMessage(dataDic, playerID);
            });
            EventHandlers["ELS:FullSync:Request:All"] += new Action<int>((int source) =>
            {
#if DEBUG
                Debug.WriteLine($"{source} is requesting Sync Data");
#endif
                TriggerClientEvent(Players[source], "ELS:FullSync:NewSpawnWithData", _cachedData);
            });
            API.RegisterCommand("resync", new Action<int, System.Collections.IList, string>((a,b,c)=> {
#if DEBUG
                Debug.WriteLine($"{a}, {b}, {c}");
#endif
                TriggerClientEvent(Players[(int.Parse((string)b[0]))], "ELS:FullSync:NewSpawnWithData", _cachedData);
            }), false);

            

            
        }
        void BroadcastMessage(System.Dynamic.ExpandoObject dataDic, int SourcePlayerID)
        {
            
            foreach(var ply in Players)
            {
#if DEBUG
                Debug.WriteLine($"comparing {ply.EndPoint} with {Players[SourcePlayerID].EndPoint}");
#endif
                if (!ply.EndPoint.Equals(Players[SourcePlayerID].EndPoint))
                {
                    TriggerClientEvent("ELS:NewFullSyncData", dataDic);
                }
            }
        }
    }
}
