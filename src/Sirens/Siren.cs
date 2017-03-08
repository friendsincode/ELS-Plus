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
    public class Siren
    {
        private readonly SirenTypes _sirenTypes;
        public readonly Vehicle _vehicle;
        //private List<Tone> tones = new List<Tone>(5);//0=horn
        private struct tones
        {
            public static Tone horn;
        }
        public Siren(Vehicle vehicle)
        {
            _vehicle = vehicle;
            tones.horn = new Tone("SIRENS_AIRHORN", _vehicle);
        }

        public void ticker()
        {
            if (Game.IsControlJustPressed(0, Control.MpTextChatTeam) || Game.IsControlJustPressed(1, Control.FrontendAccept))
            {
                Game.DisableControlThisFrame(0, Control.MpTextChatTeam);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                tones.horn.SetState(true);
            }
            if (Game.IsControlJustReleased(0, Control.MpTextChatTeam) || Game.CurrentInputMode == InputMode.GamePad && (Game.IsControlJustReleased(2, Control.ScriptPadDown)))
            {
                Game.DisableControlThisFrame(0, Control.MpTextChatTeam);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                tones.horn.SetState(false);
            }
        }
        
    }
}