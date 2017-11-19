using CitizenFX.Core;
using ELS.configuration;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        void AirHornControlsKB()
        {
            if ((Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard) ||
                (Game.IsControlJustPressed(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                AirHornLogic(true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, true,Game.Player.ServerId);
#endif
            }
            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                AirHornLogic(false);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, false,Game.Player.ServerId);
#endif
            }
        }

        void ManualTone1ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                ManualTone1Logic(true);

#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone1, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
        void ManualTone2ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                ManualTone2Logic(true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone2, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
        void ManualTone3ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                ManualTone3Logic(true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone3, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
        void ManualTone4ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                ManualTone4Logic(true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone4, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }

        void MainSirenToggleControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Toggle_SIRN))
            {
                MainSirenToggleLogic(true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MainSiren, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }

        void ManualSoundControlsKB()
        {
            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Sound_Manul);
                ManualSoundLogic(true);
#if !REMOTETEST
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.)
#endif
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                ManualSoundLogic(false);
#if !REMOTETEST
#endif
            }
        }

        void DualSirenControlsKB()
        {
            if (Game.IsControlJustReleased(0, ControlConfiguration.KeyBindings.Toggle_DSRN))
            {
                DualSirenLogic(true);
#if !REMOTETEST
#endif
            }
        }
    }
}