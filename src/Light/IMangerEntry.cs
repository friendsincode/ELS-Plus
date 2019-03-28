using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    internal interface IManagerEntry
    {
        void CleanUP();
        void Ticker();
        void ControlTicker();
        void LightsControlsRemote();
        Vehicle _vehicle { get; set; }
    }
}
