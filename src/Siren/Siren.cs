using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Siren;
using ELS.Siren.Tones;

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
        struct tones
        {
            public Tone horn;
            public Tone tone1;
            public Tone tone2;
            public Tone tone3;
            public Tone tone4;
        }
        tones _tones;
        public Siren(Vehicle vehicle)
        {
            if (EntityDecoration.ExistOn(vehicle, "HasELS"))
            {
                _vehicle = vehicle;
                _tones = new tones();
                _tones.horn = new Tone("SIRENS_AIRHORN", _vehicle);

                _tones.tone1 = new Tone("", _vehicle,EntityDecoration.Get<bool>(_vehicle, "HornState"));
                _tones.tone2 = new Tone("", _vehicle);
                _tones.tone3 = new Tone("", _vehicle);
                _tones.tone4 = new Tone("", _vehicle);
                updateLocalRemoteSiren();
            }
            else
            {
                _vehicle = vehicle;
                _tones = new tones();
                EntityDecoration.RegisterProperty("HasELS", DecorationType.Bool);
                EntityDecoration.Set(_vehicle, "HasELS",true);
                _tones.horn = new Tone("SIRENS_AIRHORN", _vehicle);
                EntityDecoration.RegisterProperty("HornState", DecorationType.Bool);

                _tones.tone1 = new Tone("", _vehicle);
                _tones.tone2 = new Tone("", _vehicle);
                _tones.tone3 = new Tone("", _vehicle);
                _tones.tone4 = new Tone("", _vehicle);
            }
           
        }
        public void CleanUP()
        {
            _tones.horn.CleanUp();
            _tones.tone1.CleanUp();
            _tones.tone2.CleanUp();
            _tones.tone3.CleanUp();
            _tones.tone4.CleanUp();
            EntityDecoration.Set(_vehicle, "HornState", false);
            
        }
        public void ticker()
        {
            Function.Call(Hash.SET_HORN_ENABLED, _vehicle, false);
            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&Game.CurrentInputMode==InputMode.MouseAndKeyboard)
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(true);
                Debug.WriteLine("set decoration");
                EntityDecoration.Set(_vehicle, "HornState", true);
                RemoteEventManager.SendEvent(RemoteEventManager.MessageTypes.SirenUpdate, _vehicle);
            }

            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) && Game.CurrentInputMode == InputMode.MouseAndKeyboard) 
                ||  (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad ))
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(false);
                Debug.WriteLine("set decoration");
                EntityDecoration.Set(_vehicle, "HornState", false);
                RemoteEventManager.SendEvent(RemoteEventManager.MessageTypes.SirenUpdate, _vehicle);
            }
        }

        internal void updateLocalRemoteSiren()
        {
            var state = EntityDecoration.Get<bool>(this._vehicle, "HornState");
            _tones.horn.SetState(state);
        }
    }
}