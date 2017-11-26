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

        internal static void SendData(string SirenType,IDictionary dic,int uniqueId)
        {
            BaseScript.TriggerServerEvent("ELS:FullSync",SirenType,dic,uniqueId);
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
        Dictionary<string, string> ToDic();
        void RequestData();
        void RunSendSync();
    }
}