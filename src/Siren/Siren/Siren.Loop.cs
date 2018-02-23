using CitizenFX.Core;
using ELS.Light;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        public void Ticker()
        {
            AirHornControlsKB();
            ManualTone1ControlsKB();
            ManualTone2ControlsKB();
            ManualTone3ControlsKB();
            ManualTone4ControlsKB();
            ManualSoundControlsKB();
            MainSirenToggleControlsKB();
            DualSirenControlsKB();
        }
        public void ExternalTicker()
        {
            PanicAlarmControlsKB();
        }
    }
}
