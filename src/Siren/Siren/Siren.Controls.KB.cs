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
                AirHornLogic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, true,Game.Player.ServerId);
#endif
            }
            if ((Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn) &&
                 Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                || (Game.IsControlJustReleased(2, Control.ScriptPadDown) && Game.CurrentInputMode == InputMode.GamePad))
            {
                AirHornLogic(false,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.AirHorn, _vehicle, false,Game.Player.ServerId);
#endif
            }
        }

        void ManualTone1ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon1))
            {
                ManualTone1Logic(true,true);

#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone1, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
        void ManualTone2ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon2))
            {
                ManualTone2Logic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone2, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
        void ManualTone3ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon3))
            {
                ManualTone3Logic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone3, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
        void ManualTone4ControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Snd_SrnTon4))
            {
                ManualTone4Logic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualTone4, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }

        void MainSirenToggleControlsKB()
        {
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Toggle_SIRN))
            {
                MainSirenToggleLogic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MainSiren, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }

        void ManualSoundControlsKB()
        {
            if (Game.IsControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                ManualSoundLogic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualSound, _vehicle, true, Game.Player.ServerId);
#endif
            }
            if (Game.IsControlJustReleased(0, configuration.ControlConfiguration.KeyBindings.Sound_Manul))
            {
                ManualSoundLogic(false,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ManualSound, _vehicle, false, Game.Player.ServerId);
#endif
            }
        }

        void DualSirenControlsKB()
        {
            if (Game.IsControlJustReleased(0, ControlConfiguration.KeyBindings.Toggle_DSRN))
            {
                DualSirenLogic(true,true);
#if !REMOTETEST
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.DualSiren, _vehicle, true, Game.Player.ServerId);
#endif
            }
        }
    }
}