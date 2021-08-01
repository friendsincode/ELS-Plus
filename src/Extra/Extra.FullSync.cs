using ELS.FullSync;

namespace ELS.Extra
{
    public struct ExtraFSData
    {
        public bool PatternRunning { get; set; }
        public bool TurnedOn { get; set; }
        public int Pattern { get; set; }
    }

    internal partial class Extra : IFullSyncComponent<ExtraFSData>
    {

        public ExtraFSData GetData()
        {

            

            return new ExtraFSData() { PatternRunning = IsPatternRunning, TurnedOn = TurnedOn, Pattern = PatternNum };
        }

        public void SetData(ExtraFSData data)
        {
            Utils.DebugWriteLine($"Got data for extra {_Id} setting data");
            PatternNum = data.Pattern;

            IsPatternRunning = data.PatternRunning;
            TurnedOn = data.TurnedOn;
        }

    }
}
