using System.Collections.Generic;
using CitizenFX.Core;
using ELS.FullSync;


namespace ELS.Siren
{
    public struct SirenFSData
    {
        public bool Dual { get; set; }
        public MainSirenFSData MainSiren { get; set; }
        public TonesFSData Tones { get; set; }
    }

    partial class Siren : IManagerEntry, IFullSyncComponent<SirenFSData>
    {
        public void SetData(SirenFSData data)
        {
            if (data.Equals(null))
            {
                return;
            }
            Utils.DebugWriteLine("Setting data for sirens");


            _mainSiren.SetData(data.MainSiren);


            _tones.SetData(data.Tones);

            dual_siren = data.Dual;

        }

        public SirenFSData GetData()
        {
            //var dic = new Dictionary<string, object>
            //{
            //    {"_mainSiren",_mainSiren.GetData() },
            //    { "_tones",_tones.GetData()},
            //    {"dual_siren",dual_siren }
            //};
            return new SirenFSData() { Dual = dual_siren, Tones = _tones.GetData(), MainSiren = _mainSiren.GetData() };
        }

    }
}