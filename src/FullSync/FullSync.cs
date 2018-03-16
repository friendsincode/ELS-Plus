using System.Collections;
using System.Collections.Generic;
using CitizenFX.Core;

namespace ELS.FullSync
{
    public class FullSync
    {
        protected FullSync()
        {

        }
    }

    internal class FullSyncManager
    {
        internal FullSyncManager()
        {
            
        }

        internal static void RequestData(long NetworkID)
        {
            ELS.TriggerServerEvent("ELS:FullSync:Request", NetworkID);
        }

        internal static void SendDataBroadcast(IDictionary dic,int PlayerId)
        {
            ELS.TriggerServerEvent("ELS:FullSync:Broadcast",dic,PlayerId);
        }
        internal static void SendDataUnicast(IDictionary dic,int PlayerID)
        {
            ELS.TriggerServerEvent("ELS:FullSync:Unicast", dic,PlayerID);
        }
    }
    internal static class SetData
    {
//        SetData()
//        {
//
//        }
    }

    internal interface IFullSyncComponent
    {
        void SetData(IDictionary<string, object> data);
        Dictionary<string, object> GetData();
    }
}