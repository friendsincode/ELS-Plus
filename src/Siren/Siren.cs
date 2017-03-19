using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Siren.Tones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ELS.Siren
{
    public delegate void StateChangedHandler(Tone tone);

    /// <summary>
    /// Has diffrent tones
    /// </summary>
    class Siren
    {
        private configuration.ControlConfiguration.ELSControls keyBinding;
        public readonly Vehicle _vehicle;
        private MainSiren _mainSiren;
        configuration.ControlConfiguration.ELSControls keybindings = new configuration.ControlConfiguration.ELSControls();

        internal struct Tones
        {
            public Tone horn;
            public Tone tone1;
            public Tone tone2;
            public Tone tone3;
            public Tone tone4;
        }
        Tones _tones;

        internal class MainSiren
        {
            public bool _state { get; private set; }
            private Tone currentTone;
            private List<Tone> MainTones;
            public MainSiren(Tones tonesl)
            {
                MainTones = new List<Tone>(new[] { tonesl.tone1, tonesl.tone2, tonesl.tone3, tonesl.tone4 });
                currentTone = MainTones[0];
            }

            public void SetState(bool state)
            {
                _state = state;
                if (_state)
                {
                    currentTone.SetState(true);
                }
                else
                {
                    currentTone.SetState(false);
                }
            }
            internal void nextTone()
            {
                if (MainTones.IndexOf(currentTone) == MainTones.Count - 1)
                {
                    currentTone.SetState(false);
                    currentTone = MainTones[0];
                    currentTone.SetState(true);
                }
                else
                {
                    currentTone.SetState(false);
                    currentTone = MainTones[MainTones.IndexOf(currentTone) + 1];
                    currentTone.SetState(true);
                }
            }

        }
        public Siren(Vehicle vehicle)
        {
            _vehicle = vehicle;
            _tones = new Tones();
            _tones.horn = new Tone(configuration.SoundConfig.SoundBindings.MainHorn, _vehicle, ToneType.Horn);
            _tones.tone1 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone1, _vehicle, ToneType.SrnTon1);
            _tones.tone2 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone2, _vehicle, ToneType.SrnTon2);
            _tones.tone3 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone3, _vehicle, ToneType.SrnTon3);
            _tones.tone4 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone4, _vehicle, ToneType.SrnTon4);
            _mainSiren = new MainSiren(_tones);

        }
        public void CleanUP()
        {
            _tones.horn.CleanUp();
            _tones.tone1.CleanUp();
            _tones.tone2.CleanUp();
            _tones.tone3.CleanUp();
            _tones.tone4.CleanUp();

        }
        public void ticker()
        {
            Function.Call(Hash.SET_HORN_ENABLED, _vehicle, false);
            if ((Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) && Game.CurrentInputMode == InputMode.MouseAndKeyboard) ||
            (Game.IsControlJustPressed(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(true);
            }
            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(false);
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1);

                _tones.tone1.SetState(true);
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                _tones.tone1.SetState(false);
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2);

                _tones.tone2.SetState(true);
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                _tones.tone2.SetState(false);
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3);

                _tones.tone3.SetState(true);
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                _tones.tone3.SetState(false);
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4);

                _tones.tone4.SetState(true);
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                _tones.tone4.SetState(false);
            }


            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                if (!_mainSiren._state)
                {
                    _tones.tone1.SetState(true);
                }
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                if (_mainSiren._state)
                {
                    _mainSiren.nextTone();
                }
                else
                {
                    _tones.tone1.SetState(false);
                }
            }




            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Toggle_SIRN))
            {
                _mainSiren.SetState(!_mainSiren._state);
            }
        }

        internal void updateLocalRemoteSiren(string sirenString, bool state)
        {
            Debug.WriteLine(sirenString);
            Enum.TryParse(sirenString, out ToneType tonetype);
            switch (tonetype)
            {
                case ToneType.Horn:
                    _tones.horn.SetRemoteState(state);
                    Debug.WriteLine("setting horn" + state);
                    break;
                case ToneType.SrnTon1:
                    _tones.tone1.SetRemoteState(state);
                    Debug.WriteLine("setting 1");
                    break;
                case ToneType.SrnTon2:
                    _tones.tone2.SetRemoteState(state);
                    Debug.WriteLine("setting 2");
                    break;
                case ToneType.SrnTon3:
                    _tones.tone3.SetRemoteState(state);
                    Debug.WriteLine("setting 3");
                    break;
                case ToneType.SrnTon4:
                    _tones.tone4.SetRemoteState(state);
                    Debug.WriteLine("setting 4");
                    break;
            }
        }
    }
}