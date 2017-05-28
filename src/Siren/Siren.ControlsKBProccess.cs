using CitizenFX.Core;
using ELS.configuration;

namespace ELS.Siren
{
    partial class Siren
    {
        void AirHornControlsKBProccess()
        {
            if ((Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard) ||
                (Game.IsControlJustPressed(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
#if !REMOTETEST
                AirHornControls(true);
#endif
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, true);
            }
            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
#if !REMOTETEST
                AirHornControls(false);
#endif
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, false);
            }
        }

        void ManualTone1ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
#if !REMOTETEST
                ManualTone1Controls(true);
#endif
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone1, _vehicle, true);
            }
        }
        void ManualTone2ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
#if !REMOTETEST
                ManualTone2Controls(true);
#endif
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone2, _vehicle, true);
            }
        }
        void ManualTone3ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
#if !REMOTETEST
                ManualTone3Controls(true);
#endif
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone3, _vehicle, true);
            }
        }
        void ManualTone4ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            { 
#if !REMOTETEST
                ManualTone4Controls(true);
#endif
            RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone4, _vehicle, true);
            }
        }

        void MainSirenToggleControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Toggle_SIRN))
            {
#if !REMOTETEST
                MainSirenToggleControls(true);
#endif
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MainSiren, _vehicle, true);
            }
        }

        void ManualSoundControlsKBProccess()
        {
            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Sound_Manul);
#if !REMOTETEST
                ManualSoundControls(true);
#endif
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
#if !REMOTETEST
                ManualSoundControls(false);
#endif
            }
        }

        void DualSirenControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, ControlConfiguration.KeyBindings.Toggle_DSRN))
            {
#if !REMOTETEST
                DualSirenControls(true);
#endif
            }
        }
    }
}