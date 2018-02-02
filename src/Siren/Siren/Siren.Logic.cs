using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.configuration;
using Control = CitizenFX.Core.Control;
using ELS.Light;
using ELS.NUI;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        void AirHornLogic(bool pressed, bool disableControls = false)
        {
            if (disableControls)
            {
                Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
            }
            if (pressed)
            {
                ElsUiPanel.ToggleUiBtnState(true, "HRN");
                ElsUiPanel.SetUiDesc("AH", "HRN");
                if (bool.Parse(_vcf.SOUNDS.MainHorn.InterruptsSiren))
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
                ElsUiPanel.ToggleUiBtnState(false, "HRN");
                _tones.horn.SetState(false);
                if (bool.Parse(_vcf.SOUNDS.MainHorn.InterruptsSiren )&&
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
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Snd_SrnTon1);

            if (pressed)
            {
                if (_mainSiren._enable) //Is the MainSiren Active
                {
                    if (_mainSiren.currentTone != _tones.tone1)
                    {
                        _mainSiren.setMainTone(_tones.tone1);
                    }
                    else if (dual_siren && _tones.tone2._state || _tones.tone4._state || _tones.tone3._state)
                    {
                        _tones.tone1.SetState(!_tones.tone1._state);
                    }
                    ElsUiPanel.SetUiDesc(_mainSiren.currentTone.Type, "SRN");
                }
            }

        }
        void SirenTone2Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Snd_SrnTon2);

            if (pressed)
            {

                if (_mainSiren._enable)
                {
                    if (_mainSiren.currentTone != _tones.tone2)
                    {
                        _mainSiren.setMainTone(_tones.tone2);
                    }
                    ElsUiPanel.SetUiDesc(_mainSiren.currentTone.Type, "SRN");
                }
                else if (dual_siren && _tones.tone1._state || _tones.tone4._state || _tones.tone3._state)
                {
                   _tones.tone2.SetState(!_tones.tone2._state);
                }
            }
        }
        void SirenTone3Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Snd_SrnTon3);
            if (pressed)
            {
                if (_mainSiren._enable)
                {
                    if (_mainSiren.currentTone != _tones.tone3)
                    {
                        _mainSiren.setMainTone(_tones.tone3);
                    }
                    ElsUiPanel.SetUiDesc(_mainSiren.currentTone.Type, "SRN");
                }
                else if (dual_siren && _tones.tone1._state || _tones.tone2._state || _tones.tone4._state)
                {
                   _tones.tone3.SetState(!_tones.tone3._state);
                }
            }
        }
        void SirenTone4Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Snd_SrnTon4);
            if (pressed)
            {
                if (_mainSiren._enable)
                {
                    if (_mainSiren.currentTone != _tones.tone4)
                    {
                        _mainSiren.setMainTone(_tones.tone4);
                    }
                    ElsUiPanel.SetUiDesc(_mainSiren.currentTone.Type, "SRN");
                }
                else if(dual_siren && _tones.tone1._state || _tones.tone2._state || _tones.tone3._state)
                {
                    _tones.tone4.SetState(!_tones.tone4._state);
                }
            }
        }

        private void MainSirenToggleLogic(bool toggle, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Toggle_SIRN);
            if (toggle)
            {
                _mainSiren.SetEnable(!_mainSiren._enable);
                ElsUiPanel.ToggleUiBtnState(_mainSiren._enable, "SRN");
                ElsUiPanel.SetUiDesc(_mainSiren.currentTone.Type, "SRN");
            }
        }

        void ManualSoundLogic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Sound_Manul);
            if (pressed)
            {
                if (!_mainSiren._enable || (!_mainSiren._enable && bool.Parse(_vcf.SOUNDS.MainHorn.InterruptsSiren )&&
                                           _tones.horn._state))
                {
                    _tones.tone1.SetState(true);
                }
                else 
                {
                    _mainSiren.nextTone();
                }
            }
            else
            {
                if (!_mainSiren._enable || (!_mainSiren._enable && bool.Parse(_vcf.SOUNDS.MainHorn.InterruptsSiren )&&
                                           _tones.horn._state))
                {
                    _tones.tone1.SetState(false);
                }
                else
                {
                    _mainSiren.previousTone();
                }
            }
        }

        void DualSirenLogic(bool toggle, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Toggle_DSRN);
            if (toggle)
            {
                dual_siren = !dual_siren;
                Screen.ShowNotification($"Dual Siren {dual_siren}");
            }
        }
        void PanicAlarmLogic(bool toggle,bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ElsConfiguration.KeyBindings.Snd_SrnPnic);
            CitizenFX.Core.UI.Screen.ShowNotification($"{toggle}");
            this._tones.panicAlarm.SetState(toggle);
            if (toggle)
            {
                //Screen.ShowNotification($"{toggle}");
            }
        }
    }
}