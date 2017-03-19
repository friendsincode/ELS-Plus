using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Siren.Tones;
using System;

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
        configuration.ControlConfiguration.ELSControls keybindings = new configuration.ControlConfiguration.ELSControls();
        struct Tones
        {
            public Tone horn;
            public Tone tone1;
            public Tone tone2;
            public Tone tone3;
            public Tone tone4;
        }
        Tones _tones;
        public Siren(Vehicle vehicle)
        {
            if (EntityDecoration.ExistOn(vehicle, "HasELS"))
            {
                _vehicle = vehicle;
                _tones = new Tones()
                {
                    horn = new Tone(configuration.SoundConfig.SoundBindings.MainHorn, _vehicle,ToneType.Horn),
                    tone1 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone1, _vehicle, ToneType.SrnTon1),
                    tone2 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone2, _vehicle, ToneType.SrnTon2),
                    tone3 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone3, _vehicle, ToneType.SrnTon3),
                    tone4 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone4, _vehicle, ToneType.SrnTon4)
                };
            }
            else
            {
                _vehicle = vehicle;
                _tones = new Tones();
                EntityDecoration.RegisterProperty("HasELS", DecorationType.Bool);
                EntityDecoration.Set(_vehicle, "HasELS", true);
                _tones.horn = new Tone(configuration.SoundConfig.SoundBindings.MainHorn, _vehicle,ToneType.Horn);
                _tones.tone1 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone1, _vehicle, ToneType.SrnTon1);
                _tones.tone2 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone2, _vehicle, ToneType.SrnTon2);
                _tones.tone3 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone3, _vehicle, ToneType.SrnTon3);
                _tones.tone4 = new Tone(configuration.SoundConfig.SoundBindings.SrnTone4, _vehicle, ToneType.SrnTon4);
            }

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
                _tones.horn.SendMessage();
            }

            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(false);
                _tones.horn.SendMessage();
            }




            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1);

                _tones.tone1.SetState(true);
                _tones.tone1.SendMessage();
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                _tones.tone1.SetState(false);
                _tones.tone1.SendMessage();
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2);

                _tones.tone2.SetState(true);
                _tones.tone2.SendMessage();
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                _tones.tone2.SetState(false);
                _tones.tone2.SendMessage();
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3);

                _tones.tone3.SetState(true);
                _tones.tone3.SendMessage();
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                _tones.tone3.SetState(false);
                _tones.tone3.SendMessage();
            }



            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4);

                _tones.tone4.SetState(true);
                _tones.tone4.SendMessage();
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                _tones.tone4.SetState(false);
                _tones.tone4.SendMessage();
            }
        }

        internal void updateLocalRemoteSiren(string sirenString, bool state)
        {
            Debug.WriteLine(sirenString);
            Enum.TryParse(sirenString,out ToneType tonetype);
            switch (tonetype)
            {
                case ToneType.Horn:
                    _tones.horn.SetState(state);
                    Debug.WriteLine("setting horn" + state);
                    break;
                case ToneType.SrnTon1:
                    _tones.tone1.SetState(state);
                    Debug.WriteLine("setting 1");
                    break;
                case ToneType.SrnTon2:
                    _tones.tone2.SetState(state);
                    Debug.WriteLine("setting 2");
                    break;
                case ToneType.SrnTon3:
                    _tones.tone3.SetState(state);
                    Debug.WriteLine("setting 3");
                    break;
                case ToneType.SrnTon4:
                    _tones.tone4.SetState(state);
                    Debug.WriteLine("setting 4");
                    break;
            }
        }
    }
}