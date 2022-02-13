using CitizenFX.Core;
using System.Collections.Generic;
using ELS.FullSync;

namespace ELS.Siren
{
    public struct TonesFSData
    {
        public bool Horn { get; set; }
        public bool Tone1 { get; set; }
        public bool Tone2 { get; set; }
        public bool Tone3 { get; set; }
        public bool Tone4 { get; set; }
        public bool Panic { get; set; }
    }

    internal class Tones : IFullSyncComponent<TonesFSData>
    {
        internal Tone horn;
        internal Tone tone1;
        internal Tone tone2;
        internal Tone tone3;
        internal Tone tone4;
        internal Tone panicAlarm;

        public TonesFSData GetData()
        {
            //            var dic =
            //                new Dictionary<string, object>
            //                {
            //                    {"horn", horn._state},
            //                    {"tone1", tone1._state},
            //                    {"tone2", tone2._state},
            //                    {"tone3", tone3._state},
            //                    {"tone4", tone4._state},
            //                    {"panicAlarm",panicAlarm._state}
            //                };
            //#if REMOTETEST
            //            Utils.DebugWriteLine($"tone1: {tone1._state}\n" +
            //                $"tone2: {tone2._state}\n" +
            //                $"tone3: {tone3._state}\n" +
            //                $"tone4: {tone4._state}");
            //#endif

            return new TonesFSData()
            {
                Horn = horn._state,
                Tone1 = tone1._state,
                Tone2 = tone2._state,
                Tone3 = tone3._state,
                Tone4 = tone4._state,
                Panic = panicAlarm._state
            };
        }

        public void SetData(TonesFSData data)
        {
            horn.SetState(data.Horn);
            tone1.SetState(data.Tone1);
            tone2.SetState(data.Tone2);
            tone3.SetState(data.Tone3);
            tone4.SetState(data.Tone4);
            panicAlarm.SetState(data.Panic);
        }

    }
}