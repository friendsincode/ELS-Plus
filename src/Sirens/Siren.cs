using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Sirens;
using ELS.Sirens.Tones;

namespace ELS.Sirens
{
    public delegate void StateChangedHandler(Tone tone);

    /// <summary>
    /// Has diffrent tones
    /// </summary>
    class Siren
    {
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
        public void ticker()
        {
            if (Game.IsControlJustPressed(0, Control.MpTextChatTeam)&&Game.CurrentInputMode==InputMode.MouseAndKeyboard)
            { 
                Game.DisableControlThisFrame(0, Control.MpTextChatTeam);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(true);
                Debug.WriteLine("set decoration");
                EntityDecoration.Set(_vehicle, "HornState", true);
                Debug.WriteLine("set decoration");
                RemoteEventManager.SendEvent(RemoteEventManager.MessageTypes.SirenUpdate, _vehicle);
            }

            if ((Game.IsControlJustReleased(0, Control.MpTextChatTeam) && Game.CurrentInputMode == InputMode.MouseAndKeyboard) 
                ||  (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad ))
            {
                Game.DisableControlThisFrame(0, Control.MpTextChatTeam);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(false);
                Debug.WriteLine("set decoration");
                EntityDecoration.Set(_vehicle, "HornState", false);
                Debug.WriteLine("set decoration");
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