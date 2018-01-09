using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace ELS_Server
{
    public class Class1 : BaseScript
    {
        Dictionary<int, object> _cachedData = new Dictionary<int,object>();
        VcfSync _vcfSync;
        public Class1()
        {
            
            Debug.WriteLine("Server Running");
            EventHandlers["ELS:VcfSync:Server"] += new Action<int>((int source) =>
            {
                _vcfSync = new VcfSync();
                _vcfSync.CheckVCF(Players[source]);
            });
            EventHandlers["ELS:FullSync:Unicast"] += new Action(() => { });
            EventHandlers["ELS:FullSync:Broadcast"] += new Action<System.Dynamic.ExpandoObject,int>((dataDic,playerID) =>
            {
                var dd = (IDictionary<string,object> )dataDic;
                Debug.WriteLine($"NetworkID {dd["NetworkID"]}");
                _cachedData[(int)dd["NetworkID"]] = dd;
                BroadcastMessage(dataDic, playerID);
            });
            EventHandlers["ELS:FullSync:Request:All"] += new Action<int>((int source) =>
            {
                Debug.WriteLine($"{source} is requesting Sync Data");
                TriggerClientEvent(Players[source], "ELS:FullSync:NewSpawnWithData", _cachedData);
            });
            CitizenFX.Core.Native.API.RegisterCommand("resync", new Action<int, System.Collections.IList, string>((a,b,c)=> {
                CitizenFX.Core.Debug.WriteLine($"{a}, {b}, {c}");
                TriggerClientEvent(Players[(int.Parse((string)b[0]))], "ELS:FullSync:NewSpawnWithData", _cachedData);
            }), false);

            

            
        }
        void BroadcastMessage(System.Dynamic.ExpandoObject dataDic, int SourcePlayerID)
        {
            
            foreach(var ply in Players)
            {
                Debug.WriteLine($"comparing {ply} with {Players[SourcePlayerID]}");
                if (ply.EndPoint.Equals(Players[SourcePlayerID].EndPoint))
                {
                    TriggerClientEvent("ELS:NewFullSyncData", dataDic);
                }
            }
        }
    }
}
