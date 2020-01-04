using CitizenFX.Core;
using System;
using System.Collections.Generic;
using ELS.FullSync;
using ELS.NUI;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        internal partial class MainSiren : IFullSyncComponent
        {
            internal bool _enable { get; private set; }
            internal bool interupted = false;
            internal int currentTone;

            internal List<Tone> MainTones;
            internal MainSiren(ref Tones tonesl)
            {
                MainTones = new List<Tone>(new[] { tonesl.tone1, tonesl.tone2, tonesl.tone3, tonesl.tone4 });
                currentTone = 0;
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
                    MainTones[currentTone].SetState(true);
                    if (Game.PlayerPed.IsSittingInELSVehicle())
                    {
                        ElsUiPanel.ToggleUiBtnState(_enable, "SRN");
                    }
                }
                else
                {
                    MainTones[currentTone].SetState(false);
                    if (Game.PlayerPed.IsSittingInELSVehicle())
                    {
                        ElsUiPanel.ToggleUiBtnState(_enable, "SRN");
                    }
                }
            }

            /// <summary>
            /// Stops the current tone and disables the siren if it is equal to <paramref name="tone"/>
            /// else it will disable the current tone and enable the <paramref name="tone"/>
            /// </summary>
            /// <param name="tone"></param>
            internal void setMainTone(int tone)
            {
                _setMainTone(tone);
            }

            private void _setMainTone(int tone)
            {
                if (currentTone == tone) { SetEnable(false); }
                else
                {
                    MainTones[currentTone].SetState(false);
                    currentTone = tone;
                    MainTones[currentTone].SetState(true);
                }
            }
            internal void nextTone()
            {

                MainTones[currentTone].SetState(false);
                //OverflowCatch
                if (currentTone + 1 >= MainTones.Count) { currentTone = 0; }
                else
                {
                    currentTone += 1;
                    if (_enable) { MainTones[currentTone].SetState(true); }
                }
            }

            internal void previousTone()
            {
                MainTones[currentTone].SetState(false);
                //Underflow Check
                if (currentTone - 1 < 0) { currentTone = MainTones.Count - 1; }
                else
                {
                    currentTone = currentTone - 1;
                    if (_enable) { MainTones[currentTone].SetState(true); }
                }
               
            }

        }

    }
}