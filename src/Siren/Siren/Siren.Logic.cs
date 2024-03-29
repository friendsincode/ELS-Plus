using CitizenFX.Core;
using ELS.configuration;
using ELS.NUI;
using Control = CitizenFX.Core.Control;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        int[] patts = { 0, 0, 0 };
        void AirHornLogic(bool pressed, bool disableControls = false)
        {
            if (disableControls)
            {
                Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
            }
            if (pressed)
            {
                //Vehicles.MoveTraffic();
                patts[0] = _patternController.CurrentPrmPattern;
                patts[1] = _patternController.CurrentSecPattern;
                patts[2] = _patternController.CurrentWrnPattern;
                ElsUiPanel.ToggleUiBtnState(true, "HRN");
                ElsUiPanel.SetUiDesc("AH", "HRN");
                if (Global.BtnClicksBtwnHrnTones)
                {
                    ElsUiPanel.PlayUiSound("sirenclickoff");
                }
                if (_vcf.PRML.ForcedPatterns.MainHorn.Enabled)
                {
                    _patternController.CurrentPrmPattern = _vcf.PRML.ForcedPatterns.MainHorn.IntPattern;
                }
                if (_vcf.SECL.ForcedPatterns.MainHorn.Enabled)
                {
                    _patternController.CurrentSecPattern = _vcf.SECL.ForcedPatterns.MainHorn.IntPattern;
                }
                if (_vcf.WRNL.ForcedPatterns.MainHorn.Enabled)
                {
                    _patternController.CurrentWrnPattern = _vcf.WRNL.ForcedPatterns.MainHorn.IntPattern;
                }
                if (_vcf.SOUNDS.MainHorn.InterruptsSiren)
                {
                    if (_mainSiren._enable)
                    {
                        _mainSiren.interupted = true;
                        _mainSiren.SetEnable(false);
                    }
                }
                _tones.horn.SetState(true);
            }
            if (!pressed)
            {
                if (_vcf.PRML.ForcedPatterns.MainHorn.Enabled)
                {
                    _patternController.CurrentPrmPattern = patts[0];
                }
                if (_vcf.SECL.ForcedPatterns.MainHorn.Enabled)
                {
                    _patternController.CurrentSecPattern = patts[1];
                }
                if (_vcf.WRNL.ForcedPatterns.MainHorn.Enabled)
                {
                    _patternController.CurrentWrnPattern = patts[2];
                }
                ElsUiPanel.ToggleUiBtnState(false, "HRN");
                _tones.horn.SetState(false);
                if (_vcf.SOUNDS.MainHorn.InterruptsSiren &&
                    _mainSiren.interupted)
                {
                    _mainSiren.interupted = false;
                    _mainSiren.SetEnable(true);
                }
                ElsUiPanel.SetUiDesc("--", "HRN");
            }
        }

        void SirenTone1Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Snd_SrnTon1);

            if (pressed && _tones.tone1.AllowUse)
            {
                if (_mainSiren._enable) //Is the MainSiren Active
                {
                    if (_mainSiren.currentTone != 0)
                    {
                        _mainSiren.setMainTone(0);
                        if (_vcf.PRML.ForcedPatterns.SrnTone1.Enabled)
                        {
                            _patternController.CurrentPrmPattern = _vcf.PRML.ForcedPatterns.SrnTone1.IntPattern;
                        }
                        if (_vcf.SECL.ForcedPatterns.SrnTone1.Enabled)
                        {
                            _patternController.CurrentSecPattern = _vcf.SECL.ForcedPatterns.SrnTone1.IntPattern;
                        }
                        if (_vcf.WRNL.ForcedPatterns.SrnTone1.Enabled)
                        {
                            _patternController.CurrentWrnPattern = _vcf.WRNL.ForcedPatterns.SrnTone1.IntPattern;
                        }
                    }
                    if (_mainSiren._enable && dual_siren)
                    {
                        Utils.DebugWriteLine("Setting Dual Siren tone 2");
                        _tones.tone4.SetState(false);
                        _tones.tone1.SetState(false);
                        _tones.tone2.SetState(true);
                        _tones.tone3.SetState(false);
                    }
                    ElsUiPanel.SetUiDesc(_mainSiren.MainTones[_mainSiren.currentTone].Type, "SRN");
                    ElsUiPanel.ToggleUiBtnState(_mainSiren._enable, "SRN");
                    if (Global.BtnClicksBtwnSrnTones)
                    {
                        ElsUiPanel.PlayUiSound("sirenclickoff");
                    }
                }
            }

        }
        void SirenTone2Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Snd_SrnTon2);

            if (pressed && _tones.tone2.AllowUse)
            {

                if (_mainSiren._enable)
                {
                    if (_mainSiren.currentTone != 1)
                    {
                        _mainSiren.setMainTone(1);
                        if (_vcf.PRML.ForcedPatterns.SrnTone2.Enabled)
                        {
                            _patternController.CurrentPrmPattern = _vcf.PRML.ForcedPatterns.SrnTone2.IntPattern;
                        }
                        if (_vcf.SECL.ForcedPatterns.SrnTone2.Enabled)
                        {
                            _patternController.CurrentSecPattern = _vcf.SECL.ForcedPatterns.SrnTone2.IntPattern;
                        }
                        if (_vcf.WRNL.ForcedPatterns.SrnTone2.Enabled)
                        {
                            _patternController.CurrentWrnPattern = _vcf.WRNL.ForcedPatterns.SrnTone2.IntPattern;
                        }
                    }
                    
                }
                if (_mainSiren._enable && dual_siren)
                {
                    Utils.DebugWriteLine("Setting Dual Siren tone 3");
                    _tones.tone4.SetState(false);
                    _tones.tone1.SetState(false);
                    _tones.tone2.SetState(false);
                    _tones.tone3.SetState(true);
                }
                ElsUiPanel.SetUiDesc(_mainSiren.MainTones[_mainSiren.currentTone].Type, "SRN");
                ElsUiPanel.ToggleUiBtnState(_mainSiren._enable, "SRN");
                if (Global.BtnClicksBtwnSrnTones)
                {
                    ElsUiPanel.PlayUiSound("sirenclickoff");
                }
            }
        }
        void SirenTone3Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Snd_SrnTon3);
            if (pressed && _tones.tone3.AllowUse)
            {
                if (_mainSiren._enable)
                {
                    if (_mainSiren.currentTone != 2)
                    {
                        _mainSiren.setMainTone(2);
                        if (_vcf.PRML.ForcedPatterns.SrnTone3.Enabled)
                        {
                            _patternController.CurrentPrmPattern = _vcf.PRML.ForcedPatterns.SrnTone3.IntPattern;
                        }
                        if (_vcf.SECL.ForcedPatterns.SrnTone3.Enabled)
                        {
                            _patternController.CurrentSecPattern = _vcf.SECL.ForcedPatterns.SrnTone3.IntPattern;
                        }
                        if (_vcf.WRNL.ForcedPatterns.SrnTone3.Enabled)
                        {
                            _patternController.CurrentWrnPattern = _vcf.WRNL.ForcedPatterns.SrnTone3.IntPattern;
                        }
                    }
                }
                if (_mainSiren._enable && dual_siren)
                {
                    Utils.DebugWriteLine("Setting Dual Siren tone 4");
                    _tones.tone3.SetState(false);
                    _tones.tone1.SetState(false);
                    _tones.tone2.SetState(false);
                    _tones.tone4.SetState(true);
                }
                ElsUiPanel.SetUiDesc(_mainSiren.MainTones[_mainSiren.currentTone].Type, "SRN");
                ElsUiPanel.ToggleUiBtnState(_mainSiren._enable, "SRN");
                if (Global.BtnClicksBtwnSrnTones)
                {
                    ElsUiPanel.PlayUiSound("sirenclickoff");
                }
            }
            if (Global.BtnClicksBtwnSrnTones)
            {
                ElsUiPanel.PlayUiSound("sirenclickoff");
            }
        }
        void SirenTone4Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Snd_SrnTon4);
            if (pressed && _tones.tone4.AllowUse)
            {
                if (_mainSiren._enable)
                {
                    if (_mainSiren.currentTone != 3)
                    {
                        _mainSiren.setMainTone(3);
                        if (_vcf.PRML.ForcedPatterns.SrnTone4.Enabled)
                        {
                            _patternController.CurrentPrmPattern = _vcf.PRML.ForcedPatterns.SrnTone4.IntPattern;
                        }
                        if (_vcf.SECL.ForcedPatterns.SrnTone4.Enabled)
                        {
                            _patternController.CurrentSecPattern = _vcf.SECL.ForcedPatterns.SrnTone4.IntPattern;
                        }
                        if (_vcf.WRNL.ForcedPatterns.SrnTone4.Enabled)
                        {
                            _patternController.CurrentWrnPattern = _vcf.WRNL.ForcedPatterns.SrnTone4.IntPattern;
                        }
                    }
                }
                
                if (_mainSiren._enable && dual_siren)
                {
                    Utils.DebugWriteLine("Setting Dual Siren tone 1");
                    _tones.tone4.SetState(true);
                    _tones.tone1.SetState(false);
                    _tones.tone2.SetState(false);
                    _tones.tone3.SetState(false);
                }
                ElsUiPanel.SetUiDesc(_mainSiren.MainTones[_mainSiren.currentTone].Type, "SRN");
                ElsUiPanel.ToggleUiBtnState(_mainSiren._enable, "SRN");
                if (Global.BtnClicksBtwnSrnTones)
                {
                    ElsUiPanel.PlayUiSound("sirenclickoff");
                }
            }
        }

        private void MainSirenToggleLogic(bool toggle, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Toggle_SIRN);
            if (toggle)
            {
                Utils.DebugWriteLine($"Setting main siren for vehicle {_vehicle.NetworkId} to {toggle}");
                _mainSiren.SetEnable(!_mainSiren._enable);
                if (dual_siren)
                {
                    dual_siren = _mainSiren._enable;
                }
                ElsUiPanel.SetUiDesc(_mainSiren.MainTones[_mainSiren.currentTone].Type, "SRN");
                ElsUiPanel.PlayUiSound("sirenclickoff");
            }
            
        }

        void ManualSoundLogic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Sound_Manul);
            if (pressed)
            {
                if (!_mainSiren._enable || (!_mainSiren._enable && _vcf.SOUNDS.MainHorn.InterruptsSiren &&
                                           _tones.horn._state))
                {
                    _tones.tone1.SetState(true);
                }
                else 
                {
                    _mainSiren.nextTone();
                }
                ElsUiPanel.ToggleUiBtnState(true, "MAN");
                if (Global.BtnClicksBtwnSrnTones)
                {
                    ElsUiPanel.PlayUiSound("sirenclickoff");
                }
            }
            else
            {
                if (!_mainSiren._enable || (!_mainSiren._enable && _vcf.SOUNDS.MainHorn.InterruptsSiren &&
                                           _tones.horn._state))
                {
                    _tones.tone1.SetState(false);
                }
                else
                {
                    _mainSiren.previousTone();
                }
                ElsUiPanel.ToggleUiBtnState(false, "MAN");
            }
        }

        void DualSirenLogic(bool toggle, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Toggle_DSRN);
            if (toggle)
            {
                dual_siren = !dual_siren;
                switch(_mainSiren.MainTones[_mainSiren.currentTone].Type)
                {
                    case "WL":
                        _tones.tone2.SetState(dual_siren);
                        break;
                    case "YP":
                        _tones.tone3.SetState(dual_siren);
                        break;
                    case "A1":
                        _tones.tone4.SetState(dual_siren);
                        break;
                    case "A2":
                        _tones.tone1.SetState(dual_siren);
                        break;
                }
                Utils.DebugWriteLine($"Dual Siren {dual_siren}");
            }
            ElsUiPanel.ToggleUiBtnState(dual_siren, "DUAL");
            ElsUiPanel.PlayUiSound("sirenclickoff");
        }

        void PanicAlarmLogic(bool toggle,bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KBBindings.Snd_SrnPnic);
            //CitizenFX.Core.UI.Screen.ShowNotification($"{toggle}");
            this._tones.panicAlarm.SetState(toggle);
            if (toggle)
            {

                //Screen.ShowNotification($"{toggle}");
            }
        }
    }
}