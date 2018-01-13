using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
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
        public async void Ticker()
        {
            ToggleSecLKB();
            ToggleWrnLKB();
            ToggleBrdKB();
            ToggleCrsKB();
            ToggleLightStateKB();
            ExternalTicker();
        }

        public async void ExternalTicker()
        {
            if (_extras.BRD != null && _extras.BRD.AnimateBoard)
            {
                ToggleBrd();
            }
            foreach (Extra.Extra prim in _extras.PRML.Values)
            {
                prim.ExtraTicker();
            }
            foreach (Extra.Extra sec in _extras.SECL.Values)
            {
                sec.ExtraTicker();
            }
            foreach (Extra.Extra wrn in _extras.WRNL.Values)
            {
                wrn.ExtraTicker();
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
            API.SetVehicleSiren(_vehicle.Handle, !API.IsVehicleSirenOn(_vehicle.Handle));
            foreach (Extra.Extra ex in _extras.SECL.Values)
            {
                if (ex.IsPatternRunning)
                {
                    ex.IsPatternRunning = false;
                    ex.CleanUp();
                }
                else
                {

                    if (ex.Id == 5)
                    {
                        ex.PatternNum = 4;
                    }
                    else
                    {
                        ex.PatternNum = 5;
                    }
                    ex.IsPatternRunning = true;
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
                    ex.CleanUp();
                }
                else
                {
                    
                    switch (ex.Id)
                    {
                        case 7:
                            ex.PatternNum = 25;
                            break;
                        case 8:
                            ex.PatternNum = 27;
                            break;
                        case 9:
                            ex.PatternNum = 26;
                            break;
                    }
                    ex.IsPatternRunning = true;
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
                e.SetState(!e.State);
            }
        }

        internal void ToggleLightStateKB()
        {
            Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.ToggleLstg);
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleLstg))
            {
                ToggleLightStage();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleLstg, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal async void ToggleLightStage()
        {
            await _stage.NextStage();
            Screen.ShowNotification($"Current Lightstage is {_stage.CurrentStage}");
            switch(_stage.CurrentStage)
            {
                case 0:
                    API.SetVehicleSiren(_vehicle.Handle, false);
                    foreach (Extra.Extra e in _extras.PRML.Values)
                    {
                        e.IsPatternRunning = false;
                    }
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {
                        e.IsPatternRunning = false;
                    }
                    foreach (Extra.Extra e in _extras.WRNL.Values)
                    {
                        e.IsPatternRunning = false;
                    }
                    break;
                case 1:
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg1.Enabled))
                        {
                            e.PatternNum = int.Parse(_stage.SECL.PresetPatterns.Lstg1.Pattern);
                        } else
                        {
                            switch (e.Id)
                            {
                                case 7:
                                    e.PatternNum = 26;
                                    break;
                                case 8:
                                    e.PatternNum = 27;
                                    break;
                                case 9:
                                    e.PatternNum = 25;
                                    break;
                            }
                        }
                        e.IsPatternRunning = true;
                    }
                    break;
                case 2:
                    API.SetVehicleSiren(_vehicle.Handle, true);
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg2.Enabled))
                        {
                            e.PatternNum = int.Parse(_stage.SECL.PresetPatterns.Lstg2.Pattern);
                        } else
                        {
                            switch (e.Id)
                            {
                                case 7:
                                    e.PatternNum = 25;
                                    break;
                                case 8:
                                    e.PatternNum = 27;
                                    break;
                                case 9:
                                    e.PatternNum = 26;
                                    break;
                            }
                        }
                        e.IsPatternRunning = true;
                    }
                    int[] extras = _stage.GetStage2Extras();
                    foreach (int i in extras)
                    {
                        Extra.Extra e = _extras.PRML[i];
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg1.Enabled))
                        {
                            e.PatternNum = int.Parse(_stage.SECL.PresetPatterns.Lstg1.Pattern);
                        }
                        else
                        {
                            Patterns.Leds.SetDefaultPattern(_extras);
                        }
                        e.IsPatternRunning = true;
                    }
                    break;
                case 3:
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg3.Enabled))
                        {
                            e.PatternNum = int.Parse(_stage.SECL.PresetPatterns.Lstg3.Pattern);
                        }
                        e.IsPatternRunning = true;
                    }
                    
                    foreach (Extra.Extra e in _extras.PRML.Values)
                    { 
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg3.Enabled))
                        {
                            e.PatternNum = int.Parse(_stage.SECL.PresetPatterns.Lstg3.Pattern);
                        }
                        e.IsPatternRunning = true;
                    }
                    foreach (Extra.Extra e in _extras.WRNL.Values)
                    {
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg3.Enabled))
                        {
                            e.PatternNum = int.Parse(_stage.WRNL.PresetPatterns.Lstg3.Pattern);
                        }
                        e.IsPatternRunning = true;
                    }
                    break;
            }
        }
    }
}
