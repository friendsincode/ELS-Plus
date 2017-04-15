using CitizenFX.Core;
using ELS.configuration;

namespace ELS.Siren
{
    partial class Siren
    {
        private void AirHornControlsProccess()
        {
            if ((Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard) ||
                (Game.IsControlJustPressed(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                AirHornControls(true);
            }
            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                AirHornControls(false);
            }
        }

        private void ManualTone1ControlsProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                ManualTone1Controls(true);
            }
        }
        private void ManualTone2ControlsProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                ManualTone2Controls(true);
            }
        }
        private void ManualTone3ControlsProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                ManualTone3Controls(true);
            }
        }
        private void ManualTone4ControlsProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                ManualTone4Controls(true);
            }
        }

        private void MainSirenToggleControlsProccess()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Toggle_SIRN))
            {
                MainSirenToggleControls(true);
            }
        }

        private void ManualSoundControlsProccess()
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

        private void DualSirenControlsProccess()
        {
            if (Game.IsControlJustReleased(0, ControlConfiguration.KeyBindings.Toggle_DSRN))
            {
                DualSirenControls(true);
            }
        }
    }
}