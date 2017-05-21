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
                AirHornControls(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, true);
            }
            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                AirHornControls(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, false);
            }
        }

        void ManualTone1ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                ManualTone1Controls(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone1, _vehicle, true);
            }
        }
        void ManualTone2ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                ManualTone2Controls(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone2, _vehicle, true);
            }
        }
        void ManualTone3ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                ManualTone3Controls(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone3, _vehicle, true);
            }
        }
        void ManualTone4ControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                ManualTone4Controls(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone4, _vehicle, true);
            }
        }

        void MainSirenToggleControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Toggle_SIRN))
            {
                MainSirenToggleControls(true);
            }
        }

        void ManualSoundControlsKBProccess()
        {
            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Sound_Manul);

                ManualSoundControls(true);
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                ManualSoundControls(false);
            }
        }

        void DualSirenControlsKBProccess()
        {
            if (Game.IsControlJustReleased(0, ControlConfiguration.KeyBindings.Toggle_DSRN))
            {
                DualSirenControls(true);
            }
        }
    }
}