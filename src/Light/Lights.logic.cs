using CitizenFX.Core;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ELS.Light
{
    partial class Lights : IManagerEntry
    {
        public void Ticker()
        {
            ToggleSecLKB();
            ToggleWrnLKB();
            ToggleBrdKB();
            ToggleCrsKB();

            if (_extras.BRD != null && _extras.BRD.AnimateBoard)
            {
                ToggleBrd();
            }
        }

        internal void ToggleSecLKB()
        {
            Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.ToggleSecL);
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleSecL))
            {
                ToggleSecLights();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleSecL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleSecLights()
        {
            foreach (Extra.Extra ex in _extras.SECL.Values)
            {
                if (ex.IsPatternRunning)
                {
                    ex.IsPatternRunning = false;

                }
                else
                {
                    ex.IsPatternRunning = true;
                    if (ex.Id == 5)
                    {
                        ex.PatternNum = 4;
                    }
                    else
                    {
                        ex.PatternNum = 5;
                    }
                }
            }
        }

        internal void ToggleWrnLKB()
        {
            Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.ToggleWrnL);
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleWrnL))
            {
                ToggleWrnLights();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleWrnLights()
        {
            foreach (Extra.Extra ex in _extras.WRNL.Values)
            {
                if (ex.IsPatternRunning)
                {
                    ex.IsPatternRunning = false;

                }
                else
                {
                    ex.IsPatternRunning = true;
                    switch (ex.Id)
                    {
                        case 7:
                            ex.PatternNum = 25;
                            break;
                        case 8:
                            ex.PatternNum = 26;
                            break;
                        case 9:
                            ex.PatternNum = 25;
                            break;
                    }

                }
            }
        }

        internal void ToggleBrdKB()
        {
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleCrsL) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
                _extras.BRD.AnimateBoard = !_extras.BRD.AnimateBoard;
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleBrd()
        {
            if (_extras.BRD.BoardRaised)
            {
                _extras.BRD.LowerBoard();
            }
            else
            {
                CitizenFX.Core.Debug.WriteLine("Raising Board");
                _extras.BRD.RaiseBoard();
            }
        }

        internal void ToggleCrsKB()
        {
            Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.ToggleCrsL);
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleCrsL))
            {
                ToggleCrs();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleCrsL, _vehicle, true, Game.Player.ServerId);
            }
        }

        bool test = false;
        internal async void ToggleCrs()
        {
            foreach (Extra.Extra e in _extras.PRML.Values)
            {
                e.SetState(true);
            }
        }
    }
}
