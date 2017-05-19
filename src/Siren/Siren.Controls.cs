using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.configuration;
using Control = CitizenFX.Core.Control;

namespace ELS.Siren
{
    partial class Siren
    {
        private void AirHornControls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                if (_vcf.SOUNDS.MainHorn.InterruptsSiren)
                {
                    if (_mainSiren._state)
                    {
                        _mainSiren.interupted = true;
                        _mainSiren.SetState(false);
                    }
                    _tones.horn.SetState(true);
                }
                else
                {
                    _tones.horn.SetState(true);
                }
            }
            if (!pressed)
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
                if (_vcf.SOUNDS.MainHorn.InterruptsSiren)
                {
                    _tones.horn.SetState(false);
                    if (_mainSiren.interupted)
                    {
                        _mainSiren.interupted = false;
                        _mainSiren.SetState(true);
                    }
                }
                else
                {
                    _tones.horn.SetState(false);
                }
            }
        }

        private void ManualTone1Controls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon1);

                if (_mainSiren._state)
                {
                    _mainSiren.setMainTone(_tones.tone1);
                }
                else
                {
                    _tones.tone1.SetState(!_tones.tone1._state);
                }
            }

        }
        private void ManualTone2Controls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon2);

                if (_mainSiren._state)
                {
                    _mainSiren.setMainTone(_tones.tone2);
                }
                else
                {
                    _tones.tone2.SetState(!_tones.tone2._state);
                }
            }
        }
        private void ManualTone3Controls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon3);
                if (_mainSiren._state)
                {
                    _mainSiren.setMainTone(_tones.tone3);
                }
                else
                {
                    _tones.tone3.SetState(!_tones.tone3._state);
                }
            }
        }
        private void ManualTone4Controls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon4);
                if (_mainSiren._state)
                {
                    _mainSiren.setMainTone(_tones.tone4);
                }
                else
                {
                    _tones.tone4.SetState(!_tones.tone4._state);
                }
            }
        }

        private void MainSirenToggleControls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Toggle_SIRN);
                _mainSiren.SetState(!_mainSiren._state);
            }
        }

        private void ManualSoundControls(bool pressed)
        {
            if (pressed)
            {
                if (!_mainSiren._state || (!_mainSiren._state && _vcf.SOUNDS.MainHorn.InterruptsSiren &&
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
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Sound_Manul);
                if (!_mainSiren._state || (!_mainSiren._state && _vcf.SOUNDS.MainHorn.InterruptsSiren &&
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

        private void DualSirenControls(bool pressed)
        {
            if (pressed)
            {
                Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Toggle_DSRN);
                dual_siren = !dual_siren;
                Screen.ShowNotification($"Dual Siren {dual_siren}");
            }
        }
    }
}