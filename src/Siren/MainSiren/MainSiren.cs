using CitizenFX.Core;
using System;
using System.Collections.Generic;
using ELS.FullSync;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        private class MainSiren : IFullSyncComponent
        {
            internal bool _enable { get; private set; }
            internal bool interupted = false;
            private Tone currentTone;

            void IFullSyncComponent.SetData(IDictionary<string, object> data)
            {
                SetData(data);
            }

            Dictionary<string, string> IFullSyncComponent.ToDic()
            {
                return ToDic();
            }

            public void RequestData()
            {
                // throw new NotImplementedException();
            }

            public void RunSync()
            {
                FullSyncManager.SendData(this.GetType().Name, ToDic(), Game.Player.ServerId);
            }

            private Dictionary<string, string> ToDic()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("interupted", this.interupted.ToString());
                dic.Add("currentTone", this.MainTones.IndexOf(currentTone).ToString());
                dic.Add("state", this._enable.ToString());
                return dic;
            }
            internal void SetData(IDictionary<string, object> data)
            {
                currentTone = MainTones[int.Parse(data["currentTone"].ToString())];
                interupted = bool.Parse(data["interupted"].ToString());
                _enable = (bool.Parse(data["state"].ToString()));
            }
            private List<Tone> MainTones;
            internal MainSiren(Tones tonesl)
            {
                MainTones = new List<Tone>(new[] { tonesl.tone1, tonesl.tone2, tonesl.tone3, tonesl.tone4 });
                currentTone = MainTones[0];
            }
            /// <summary>
            /// Enables or disables the MainSiren
            /// Will also enable or disable the currentTone
            /// </summary>
            /// <param name="enable"></param>
            internal void SetEnable(bool enable)
            {
                _enable = enable;
                if (_enable) //turning the main siren on
                {
                    currentTone.SetState(true);
                }
                else
                {
                    currentTone.SetState(false);
                }
            }

            /// <summary>
            /// Stops the current tone and disables the siren if it is equal to <paramref name="tone"/>
            /// else it will disable the current tone and enable the <paramref name="tone"/>
            /// </summary>
            /// <param name="tone"></param>
            internal void setMainTone(Tone tone)
            {
                _setMainTone(MainTones.Find((maintone) => maintone == tone));
            }

            private void _setMainTone(Tone tone)
            {
                if (currentTone == tone) SetEnable(false);
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
                //OverflowCatch
                if (MainTones.IndexOf(currentTone) + 1 >= MainTones.Count) currentTone = MainTones[0];
                else
                {
                    currentTone = MainTones[MainTones.IndexOf(currentTone) + 1];
                    if (_enable) currentTone.SetState(true);
                }


            }

            internal void previousTone()
            {
                currentTone.SetState(false);
                //Underflow Check
                if (MainTones.IndexOf(currentTone) - 1 < 0) currentTone = MainTones[MainTones.Count-1];
                else
                {
                    currentTone = MainTones[MainTones.IndexOf(currentTone) - 1];
                    if (_enable) currentTone.SetState(true);
                }
               
            }

        }

    }
}