using CitizenFX.Core;
using ELS.configuration;
using System;
using System.Linq;

namespace ELS.Light
{
    partial class Lights
    {

        public async void ControlTicker()
        {
            //KB Controls
            if (!Global.NonElsOnly)
            {
                ToggleSecLKB();
                ToggleWrnLKB();
                ToggleBrdKB();
                ToggleCrsKB();
                ChgPrmPattKB();
                ChgWrnPattKB();
                ChgSecPattKB();
            }
            ToggleTdlKB();
            ToggleSclKB();
            ToggleLightStageKB();
            if (spotLight != null && spotLight.TurnedOn)
            {
                spotLight.RunControlTick();
            }
        }



        public async void Ticker()
        {
            if (!Global.NonElsOnly)
            {
                if (crsLights || prmLights || secLights || wrnLights ||
                    (_extras.TDL != null && _extras.TDL.TurnedOn) || (_extras.SCL != null && _extras.SCL.TurnedOn)
                    && !_vehicle.IsEngineRunning)
                {
                    _vehicle.IsEngineRunning = true;
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
                //foreach (Extra.Extra prim in _extras.PRML.Values)
                for (int i = 0; i < _extras.PRML.Count; i++)
                {
                    _extras.PRML.ElementAt(i).Value.ExtraTicker();
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
                //foreach (Extra.Extra sec in _extras.SECL.Values)
                for (int i = 0; i < _extras.SECL.Count; i++)
                {
                    _extras.SECL.ElementAt(i).Value.ExtraTicker();
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
                //foreach (Extra.Extra wrn in _extras.WRNL.Values)
                for (int i = 0; i < _extras.WRNL.Count; i++)
                {
                    _extras.WRNL.ElementAt(i).Value.ExtraTicker();
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

            if (spotLight != null && spotLight.TurnedOn)
            {
                spotLight.RunTick();
            }
            if (scene != null && scene.TurnedOn)
            {
                scene.RunTick();
            }
        }

        int _patternStart = 0;
        internal void ScanPatternTicker()
        {
            if (Game.GameTime - _patternStart > 15000)
            {
                _patternStart = Game.GameTime;
                Utils.DebugWriteLine("Toggling scan pattern");
                ToggleScanPattern();
            }
            else if (_patternStart == 0)
            {
                _patternStart = Game.GameTime;
            }
        }

        internal void ToggleSecLKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ToggleSecL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleSecL) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ToggleSecLights();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleSecL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleWrnLKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ToggleWrnL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleWrnL) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ToggleWrnLights();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleBrdKB()
        {
            //Game.DisableControlThisFrame(0, Control.CharacterWheel);
            if (Game.IsControlJustPressed(0, ElsConfiguration.KBBindings.ToggleBoard) && Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
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
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ToggleCrsL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleCrsL) && !Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ToggleCrs();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleCrsL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleTdlKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ToggleTdl);
            if ((Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleTdl) && !Game.IsControlPressed(0, Control.CharacterWheel)) && Game.CurrentInputMode == InputMode.MouseAndKeyboard || (Global.AllowController && Game.IsControlJustPressed(2, ElsConfiguration.GPBindings.ToggleTdl) && Game.CurrentInputMode == InputMode.GamePad))
            {
                ToggleTdl();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleTDL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleSclKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ToggleTdl);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleTdl) && Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ToggleScl();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleSCL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ChgPrmPattKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ChgPattPrmL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ChgPattPrmL) && !Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ChgPrmPatt(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattPrmL, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ChgPattPrmL) && Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ChgPrmPatt(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattPrmL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ChgSecPattKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ChgPattSecL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ChgPattSecL) && !Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ChgSecPatt(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChangeSecPatt, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ChgPattSecL) && Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ChgSecPatt(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChangeSecPatt, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ChgWrnPattKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ChgPattWrnL);
            if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ChgPattWrnL) && !Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ChgWrnPatt(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattWrnL, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ChgPattWrnL) && Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                ChgWrnPatt(true);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ChgPattWrnL, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void ToggleLightStageKB()
        {
            Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.ToggleLstg);
            if ((Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleLstg) && !Game.IsControlPressed(0, Control.CharacterWheel)) && Game.CurrentInputMode == InputMode.MouseAndKeyboard || (Global.AllowController && Game.IsControlJustPressed(2, ElsConfiguration.GPBindings.ToggleLstg) && Game.CurrentInputMode == InputMode.GamePad))
            {
                Utils.DebugWriteLine("Toggle Light stage");
                ToggleLightStage();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleLstg, _vehicle, true, Game.Player.ServerId);
            }
            else if (Game.IsDisabledControlJustPressed(0, ElsConfiguration.KBBindings.ToggleLstg) && Game.IsControlPressed(0, Control.CharacterWheel) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
            {
                Utils.DebugWriteLine("Toggle Light stage Inverse");
                ToggleLightStageInverse();
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.ToggleLstg, _vehicle, true, Game.Player.ServerId);
            }
        }

    }
}
