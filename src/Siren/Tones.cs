using CitizenFX.Core;
using System.Collections.Generic;

namespace ELS.Siren
{
    internal struct Tones
    {
        internal Tone horn;
        internal Tone tone1;
        internal Tone tone2;
        internal Tone tone3;
        internal Tone tone4;

        internal void FullSync()
        {

            BaseScript.TriggerServerEvent("ELS:FullSync", "Tones",ToDic(), Game.Player.ServerId);
        }
        private Dictionary<string, string> ToDic()
        {
            var dic =
                new Dictionary<string, string>
                {
                    {"tone1", tone1._state.ToString()},
                    {"tone2", tone2._state.ToString()},
                    {"tone3", tone3._state.ToString()},
                    {"tone4", tone4._state.ToString()}
                };
#if REMOTETEST
            Debug.WriteLine($"tone1: {tone1._state}\n" +
                $"tone2: {tone2._state}\n" +
                $"tone3: {tone3._state}\n" +
                $"tone4: {tone4._state}");
#endif

            return dic;
        }
        internal void SetData(IDictionary<string, object> data)
        {
            
            tone1.SetState(bool.Parse(data["tone1"].ToString()));
            tone2.SetState(bool.Parse(data["tone2"].ToString()));
            tone3.SetState(bool.Parse(data["tone3"].ToString()));
            tone4.SetState(bool.Parse(data["tone4"].ToString()));
        }
    }
}