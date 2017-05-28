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
                throw new NotImplementedException();
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
                foreach (var mainTone in MainTones)
                {
                    if (mainTone == tone)
                    {
                        setTone(tone);
                    }
                }
            }

            private void setTone(Tone tone)
            {
                if (currentTone == tone)
                {
                    SetState(false);
                }
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