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
        public Class1()
        {
            Debug.WriteLine("Server Running");
            EventHandlers["ELS:FullSync:Unicast"] += new Action(() => { });
            EventHandlers["ELS:FullSync:Broadcast"] += new Action<System.Dynamic.ExpandoObject>((dataDic) =>
            {
                var dd = (IDictionary<string,object> )dataDic;
                _cachedData[(int)dd["NetworkID"]] = dd;
                TriggerClientEvent("ELS:NewFullSyncData", dataDic);
            });
            EventHandlers["ELS:FullSync:Request:All"] += new Action<int>((int source) =>
            {
                Debug.WriteLine($"{source} is requesting Sync Data");
                TriggerClientEvent(Players[source], "ELS:FullSync:NewSpawnWithData", _cachedData);
            });
        }
    }
}
