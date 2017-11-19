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

        public Dictionary<string, string> ToDic()
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

        public void RequestData()
        {
            //throw new System.NotImplementedException();
        }

        public void RunSync()
        {
            FullSyncManager.SendData(this.GetType().Name, ToDic(), Game.Player.ServerId);
        }

        public void SetData(IDictionary<string, object> data)
        {
            
            tone1.SetState(bool.Parse(data["tone1"].ToString()));
            tone2.SetState(bool.Parse(data["tone2"].ToString()));
            tone3.SetState(bool.Parse(data["tone3"].ToString()));
            tone4.SetState(bool.Parse(data["tone4"].ToString()));
        }
    }
}