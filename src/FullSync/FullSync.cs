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

        internal static void SendDataBroadcast(string dic, int PlayerId)
        {
            Utils.DebugWriteLine($"ELS FS Dic is {dic.GetType()}");
            ELS.TriggerServerEvent("ELS:FullSync:Broadcast", dic, PlayerId);
        }
        internal static void SendDataUnicast(string dic, int PlayerID)
        {
            ELS.TriggerServerEvent("ELS:FullSync:Unicast", dic, PlayerID);
        }
    }
    internal static class SetData
    {
        //        SetData()
        //        {
        //
        //        }
    }

    internal interface IFullSyncComponent<T>
    {
        void SetData(T data);
         T GetData();
    }
}