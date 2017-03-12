using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Sirens;
using ELS.Sirens.Tones;

namespace ELS
{
    /// <summary>
    /// Has diffrent tones
    /// </summary>
    public class Siren
    {
        private readonly SirenTypes _sirenTypes;
        public readonly Vehicle _vehicle;
        //private List<Tone> tones = new List<Tone>(5);//0=horn
        private struct tones
        {
            public Tone horn;
        }
        tones _tones;
        public Siren(Vehicle vehicle)
        {
            _vehicle = vehicle;
            _tones = new tones();
            _tones.horn = new Tone("SIRENS_AIRHORN", _vehicle);
        }

        public void ticker()
        {
            if (Game.IsControlJustPressed(0, Control.MpTextChatTeam)&&Game.CurrentInputMode==InputMode.MouseAndKeyboard)
            {
                Game.DisableControlThisFrame(0, Control.MpTextChatTeam);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(true);
            }
            if ((Game.IsControlJustReleased(0, Control.MpTextChatTeam) && Game.CurrentInputMode == InputMode.MouseAndKeyboard) 
                ||  (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad ))
            {
                Game.DisableControlThisFrame(0, Control.MpTextChatTeam);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                _tones.horn.SetState(false);
            }
        }
        
    }
}