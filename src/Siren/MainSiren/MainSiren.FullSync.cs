using ELS.FullSync;


namespace ELS.Siren
{
    public struct MainSirenFSData
    {
        public bool Interrupted { get; set; }
        public int CurrentTone { get; set; }
        public bool State { get; set; }
    }

    partial class Siren
    {
        internal partial class MainSiren : IFullSyncComponent<MainSirenFSData>
        {


            public MainSirenFSData GetData()
            {
                //Dictionary<string, object> dic = new Dictionary<string, object>();
                ////JObject dic = new JObject();
                //dic.Add("interupted", interupted);
                //dic.Add("currentTone", currentTone);
                //dic.Add("state", _enable);
                ////dynamic dic = JSON.Serialize(new { interupted = interupted, currentTone = currentTone, state = _enable });
                return new MainSirenFSData() { Interrupted = interupted, CurrentTone = currentTone, State = _enable };
            }

            public void SetData(MainSirenFSData data)
            {
                currentTone = data.CurrentTone;
                interupted = data.Interrupted;
                _enable = data.State;
            }
        }
    }
}