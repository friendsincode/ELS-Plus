using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.FullSync;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    internal enum IndicatorState
    {
        Off,
        Left,
        Right,
        Hazard
    }

    internal class Indicator
    {

        public static IndicatorState CurrentIndicatorState(Vehicle veh) {
            return (IndicatorState)API.GetVehicleIndicatorLights(veh.Handle);
        }

        public static Dictionary<string, IndicatorState> IndStateLib = new Dictionary<string, IndicatorState>()
        {
            {"Off",IndicatorState.Off },
            {"Left", IndicatorState.Left },
            { "Right", IndicatorState.Right },
            { "Hazard", IndicatorState.Hazard }
        };

        public static int IndicatorDelay { get; set; }

        public static bool ActivateIndicatorTimer { get; set; }

        public static void ToggleInicatorState(Vehicle veh, IndicatorState state)
        {
            switch (state)
            {
                case IndicatorState.Off:
                    veh.IsLeftIndicatorLightOn = false;
                    veh.IsRightIndicatorLightOn = false;
                    break;
                case IndicatorState.Left:
                    veh.IsLeftIndicatorLightOn = true;
                    veh.IsRightIndicatorLightOn = false;
                    break;
                case IndicatorState.Right:
                    veh.IsLeftIndicatorLightOn = false;
                    veh.IsRightIndicatorLightOn = true;
                    break;
                case IndicatorState.Hazard:
                    veh.IsLeftIndicatorLightOn = true;
                    veh.IsRightIndicatorLightOn = true;
                    break;
            }
        }

        internal static void RunAsync(Vehicle veh)
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleLIND);
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleRIND);
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleHAZ);

            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleLIND) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                if (CurrentIndicatorState(veh) == IndicatorState.Left)
                {
                    Utils.DebugWriteLine("Toggle Off");
                    ToggleInicatorState(veh, IndicatorState.Off);
                    ActivateIndicatorTimer = false;
                    ElsUiPanel.PlayUiSound(false);
                    RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleInd, veh, true, Game.Player.ServerId);
                    return;
                }
                Utils.DebugWriteLine("Toggle Left");
                ToggleInicatorState(veh, IndicatorState.Left);
                ElsUiPanel.PlayUiSound(true);
                ActivateIndicatorTimer = true;
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleInd, veh, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleRIND) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                if (CurrentIndicatorState(veh) == IndicatorState.Right)
                {
                    Utils.DebugWriteLine("Toggle Off");
                    ToggleInicatorState(veh, IndicatorState.Off);
                    ActivateIndicatorTimer = false;
                    ElsUiPanel.PlayUiSound(false);
                    RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleInd, veh, true, Game.Player.ServerId);
                    return;
                }
                Utils.DebugWriteLine("Toggle Right");
                ToggleInicatorState(veh, IndicatorState.Right);
                ActivateIndicatorTimer = true;
                ElsUiPanel.PlayUiSound(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleInd, veh, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleHAZ) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                if (CurrentIndicatorState(veh) == IndicatorState.Hazard)
                {
                    Utils.DebugWriteLine("Toggle Off");
                    ToggleInicatorState(veh, IndicatorState.Off);
                    ActivateIndicatorTimer = false;
                    ElsUiPanel.PlayUiSound(false);
                    RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleInd, veh, true, Game.Player.ServerId);
                    return;
                }
                Utils.DebugWriteLine("Toggle Hazard");
                ToggleInicatorState(veh, IndicatorState.Hazard);
                ActivateIndicatorTimer = true;
                ElsUiPanel.PlayUiSound(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleInd, veh, true, Game.Player.ServerId);
            }


        }
    }
}
