using System;
using System.Collections.Generic;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        internal class MainSiren
        {
            internal bool _state { get; private set; }
            internal bool interupted = false;
            private Tone currentTone;

            internal void FullSync()
            {
                CitizenFX.Core.BaseScript.TriggerServerEvent("ELS:FullSync:MainSiren", ToDic());
            }
            private Dictionary<string, string> ToDic()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("interupted", this.interupted.ToString());
                dic.Add("currentTone", this.MainTones.IndexOf(currentTone).ToString());
                dic.Add("state", this._state.ToString());
                return dic;
            }
            internal void SetData(Dictionary<string, string> data)
            {
                currentTone = MainTones[int.Parse(data["currentTone"])];
                interupted = bool.Parse(data["interupted"]);
                _state=(bool.Parse(data["state"]));
            }
            private List<Tone> MainTones;
            internal MainSiren(Tones tonesl)
            {
                MainTones = new List<Tone>(new[] { tonesl.tone1, tonesl.tone2, tonesl.tone3, tonesl.tone4 });
                currentTone = MainTones[0];
            }

            internal void SetState(bool state)
            {
                _state = state;
                if (_state) //turning the main siren on
                {
                    currentTone.SetState(true);
                }
                else
                {
                    currentTone.SetState(false);
                }
            }

            internal void setMainTone(Tone tone)
            {
                setTone(MainTones.Find((maintone) => maintone == tone));
            }

            private void setTone(Tone tone)
            {
                if (currentTone == tone) SetState(false);
                else
                {
                    currentTone.SetState(false);
                    currentTone = tone;
                    currentTone.SetState(true);
                }
            }
            internal void nextTone()
            {

                currentTone.SetState(false);
                currentTone = MainTones[MainTones.IndexOf(currentTone) + 1];
                if (_state) currentTone.SetState(true);

            }

            internal void previousTone()
            {
                currentTone.SetState(false);
                currentTone = MainTones[MainTones.IndexOf(currentTone) - 1];
                if (_state) currentTone.SetState(true);
            }

        }

    }
}