using System;
using CitizenFX.Core;

namespace ELS.Siren
{
    partial class Siren
    {
        struct syncSettings
        {
            private bool dualsiren;
            private bool mainSiren;
        }
        public void updateLocalRemoteELSSettings()
        {
            
        }
        public void updateLocalRemoteSiren(string sirenString, bool state)
        {
#if DEBUG
            Debug.WriteLine(sirenString);
#endif
            ToneType tonetype;
            Enum.TryParse(sirenString, out tonetype);
            switch (tonetype)
            {
                case ToneType.Horn:
                    _tones.horn.SetRemoteState(state);
                    Debug.WriteLine("setting horn" + state);
                    break;
                case ToneType.SrnTon1:
                    _tones.tone1.SetRemoteState(state);
                    Debug.WriteLine("setting tone 1");
                    break;
                case ToneType.SrnTon2:
                    _tones.tone2.SetRemoteState(state);
                    Debug.WriteLine("setting tone 2");
                    break;
                case ToneType.SrnTon3:
                    _tones.tone3.SetRemoteState(state);
                    Debug.WriteLine("setting tone 3");
                    break;
                case ToneType.SrnTon4:
                    _tones.tone4.SetRemoteState(state);
                    Debug.WriteLine("setting tone 4");
                    break;
            }
        }
    }
}