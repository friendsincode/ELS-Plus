using CitizenFX.Core;
using System.Collections.Generic;
using ELS.FullSync;

namespace ELS.Siren
{
    internal class Tones : IFullSyncComponent
    {
        internal Tone horn;
        internal Tone tone1;
        internal Tone tone2;
        internal Tone tone3;
        internal Tone tone4;
        internal Tone panicAlarm;

        public Dictionary<string, object> ToDic()
        {
            var dic =
                new Dictionary<string, object>
                {
                    {"horn", horn._state.ToString()},
                    {"tone1", tone1._state.ToString()},
                    {"tone2", tone2._state.ToString()},
                    {"tone3", tone3._state.ToString()},
                    {"tone4", tone4._state.ToString()},
                    {"panicAlarm",panicAlarm._state.ToString()}
                };
#if REMOTETEST
            CitizenFX.Core.Debug.WriteLine($"tone1: {tone1._state}\n" +
                $"tone2: {tone2._state}\n" +
                $"tone3: {tone3._state}\n" +
                $"tone4: {tone4._state}");
#endif

            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
            horn.SetState(bool.Parse(data["horn"].ToString()));
            tone1.SetState(bool.Parse(data["tone1"].ToString()));
            tone2.SetState(bool.Parse(data["tone2"].ToString()));
            tone3.SetState(bool.Parse(data["tone3"].ToString()));
            tone4.SetState(bool.Parse(data["tone4"].ToString()));
            panicAlarm.SetState(bool.Parse(data["panicAlarm"].ToString()));
        }
    }
}