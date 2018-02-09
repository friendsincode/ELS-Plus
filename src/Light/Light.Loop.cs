using CitizenFX.Core;
using ELS.configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    partial class Lights
    {

        public async void Ticker()
        {
            //KB Controls
            ToggleSecLKB();
            ToggleWrnLKB();
            ToggleBrdKB();
            ToggleCrsKB();
            ChgPrmPattKB();
            ChgWrnPattKB();
            ChgSecPattKB();
            ToggleTdlKB();
            ToggleSclKB();
            ToggleLightStageKB();
            
            //Part that runs the ticks
            if (_extras.BRD.HasBoard)
            {
                _extras.BRD.BoardTicker();
            }
            if (_extras.SCL != null)
            {
                _extras.SCL.ExtraTicker();
            }
            if (_extras.TDL != null)
            {
                _extras.TDL.ExtraTicker();
            }
            if (_extras.SBRN != null)
            {
                _extras.SBRN.ExtraTicker();
            }
            foreach (Extra.Extra prim in _extras.PRML.Values)
            {
                prim.ExtraTicker();
                if (_stage != null)
                {
                    switch (_stage.CurrentStage)
                    {
                        case 1:
                            if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg1.Pattern) && _stage.PRML.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg1.Pattern) && _stage.PRML.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 3:
                            if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg1.Pattern) && _stage.PRML.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                    }
                }
            }
            foreach (Extra.Extra sec in _extras.SECL.Values)
            {
                sec.ExtraTicker();
                if (_stage != null)
                {
                    switch (_stage.CurrentStage)
                    {
                        case 1:
                            if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 3:
                            if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                    }
                }
            }
            foreach (Extra.Extra wrn in _extras.WRNL.Values)
            {
                wrn.ExtraTicker();
                if (_stage != null)
                {
                    switch (_stage.CurrentStage)
                    {
                        case 1:
                            if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg1.Pattern) && _stage.WRNL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg1.Pattern) && _stage.WRNL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 3:
                            if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg1.Pattern) && _stage.WRNL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                    }
                }
            }
        }

        public async void ExternalTicker()
        {
            if (crsLights || prmLights || secLights || wrnLights || 
                (_extras.TDL != null && _extras.TDL.TurnedOn) || (_extras.SCL != null && _extras.SCL.TurnedOn) 
                && !_vehicle.IsEngineRunning)
            {
                _vehicle.IsEngineRunning = true;
            }
            if (_extras.BRD.HasBoard)
            {
                _extras.BRD.BoardTicker();
            }
            if (_extras.SBRN != null)
            {
                _extras.SBRN.ExtraTicker();
            }
            if (_extras.SCL != null)
            {
                _extras.SCL.ExtraTicker();
            }
            if (_extras.TDL != null)
            {
                _extras.TDL.ExtraTicker();
            }
            foreach (Extra.Extra prim in _extras.PRML.Values)
            {
                prim.ExtraTicker();
                if (_stage != null)
                {
                    switch (_stage.CurrentStage)
                    {
                        case 1:
                            if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg1.Pattern) && _stage.PRML.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg1.Pattern) && _stage.PRML.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 3:
                            if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg1.Pattern) && _stage.PRML.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                    }
                }
            }
            foreach (Extra.Extra sec in _extras.SECL.Values)
            {
                sec.ExtraTicker();
                if (_stage != null)
                {
                    switch (_stage.CurrentStage)
                    {
                        case 1:
                            if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 3:
                            if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                    }
                }
            }
            foreach (Extra.Extra wrn in _extras.WRNL.Values)
            {
                wrn.ExtraTicker();
                if (_stage != null)
                {
                    switch (_stage.CurrentStage)
                    {
                        case 1:
                            if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg1.Pattern) && _stage.WRNL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 2:
                            if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg1.Pattern) && _stage.WRNL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                        case 3:
                            if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg1.Pattern) && _stage.WRNL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan") && _scan)
                            {
                                ScanPatternTicker();
                            }
                            break;
                    }
                }
            }
        }

        int _patternStart = 0;
        internal void ScanPatternTicker()
        {
            if (Game.GameTime - _patternStart > 15000)
            {
                _patternStart = Game.GameTime;
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Toggling scan pattern");
#endif
                ToggleScanPattern();
            }
            else if (_patternStart == 0)
            {
                _patternStart = Game.GameTime;
            }
        }

        internal void ToggleSecLKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleSecL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleSecL))
            {
                ToggleSecLights();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleSecL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleWrnLKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleWrnL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleWrnL))
            {
                ToggleWrnLights();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleBrdKB()
        {
            Game.DisableControlThisFrame(0, Control.CharacterWheel);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleBoard) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"Is Board raised  {_extras.BRD.BoardRaised}");
#endif
                if (_extras.BRD.BoardRaised)
                {
                    _extras.BRD.RaiseBoardNow = false;
                }
                else
                {
                    _extras.BRD.RaiseBoardNow = true;
                }
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleCrsKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleCrsL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleCrsL) && !Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ToggleCrs();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleCrsL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleTdlKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleTdl);
            if ((Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleTdl) && !Game.IsControlPressed(0, Control.CharacterWheel)) || (Global.AllowController && Game.IsControlJustReleased(2, Control.Talk) && Game.CurrentInputMode == InputMode.GamePad))
            {
                if (_extras.TDL != null)
                {
                    ToggleTdl();
                }
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleTDL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleSclKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleTdl);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleTdl) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
                if (_extras.SCL != null)
                {
                    ToggleScl();
                }
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleSCL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ChgPrmPattKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ChgPattPrmL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ChgPattPrmL) && !Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ChgPrmPatt(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattPrmL, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ChgPattPrmL) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ChgPrmPatt(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattPrmL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ChgSecPattKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ChgPattSecL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ChgPattSecL) && !Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ChgSecPatt(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChangeSecPatt, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ChgPattSecL) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ChgSecPatt(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChangeSecPatt, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ChgWrnPattKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ChgPattWrnL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ChgPattWrnL) && !Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ChgWrnPatt(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattWrnL, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ChgPattWrnL) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
                ChgWrnPatt(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleLightStageKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.ToggleLstg);
            if ((Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleLstg) && !Game.IsControlPressed(0, Control.CharacterWheel)) || (Global.AllowController && Game.IsControlJustReleased(2, Control.Detonate) && Game.CurrentInputMode == InputMode.GamePad))
            {
                Utils.DebugWriteLine("Toggle Light stage");
                ToggleLightStage();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleLstg, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KeyBindings.ToggleLstg) && Game.IsControlPressed(0, Control.CharacterWheel))
            {
                Utils.DebugWriteLine("Toggle Light stage Inverse");
                ToggleLightStageInverse();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleLstg, _vehicle, true, Game.Player.ServerId);
            }
        }

    }
}
