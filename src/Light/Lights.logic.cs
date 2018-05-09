using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using ELS.Light.Patterns;
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
        internal bool _scan = false;

        bool secLights = false;
        internal void ToggleSecLights()
        {
            //foreach (Extra.Extra ex in _extras.SECL.Values)
            for (int i = 0; i < _extras.SECL.Count; i++)
            {
                if (_extras.SECL.ElementAt(i).Value.IsPatternRunning)
                {
                    _extras.SECL.ElementAt(i).Value.IsPatternRunning = false;
                    _extras.SECL.ElementAt(i).Value.CleanUp();
                }
                else
                {
                    _extras.SECL.ElementAt(i).Value.IsPatternRunning = true;
                }
            }
            secLights = !secLights;
            ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
            ElsUiPanel.PlayUiSound(secLights);
        }

        bool wrnLights = false;
        internal void ToggleWrnLights()
        {
            //foreach (Extra.Extra ex in _extras.WRNL.Values)
            for (int i = 0; i < _extras.SECL.Count; i++)
            {
                if (_extras.WRNL.ElementAt(i).Value.IsPatternRunning)
                {
                    _extras.WRNL.ElementAt(i).Value.IsPatternRunning = false;
                    _extras.WRNL.ElementAt(i).Value.CleanUp();
                }
                else
                {
                    _extras.WRNL.ElementAt(i).Value.IsPatternRunning = true;
                }
            }
            wrnLights = !wrnLights;
            ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
            ElsUiPanel.PlayUiSound(wrnLights);
        }

        bool crsLights = false;
        internal async void ToggleCrs()
        {
            if (_vcfroot.CRUISE.DisableAtLstg3 && _stage.CurrentStage == 3)
            {
                foreach (Extra.Extra e in _extras.PRML.Values)
                {
                    e.TurnedOn = false;
                }
            }
            foreach (Extra.Extra e in _extras.PRML.Values)
            {
                switch (e.Id)
                {
                    case 1:
                        if (_vcfroot.CRUISE.UseExtras.Extra1)
                        {
                            e.TurnedOn = !e.TurnedOn;
                        }
                        else
                        {
                            e.TurnedOn = false;
                        }
                        break;
                    case 2:
                        if (_vcfroot.CRUISE.UseExtras.Extra2)
                        {
                            e.TurnedOn = !e.TurnedOn;
                        }
                        else
                        {
                            e.TurnedOn = false;
                        }
                        break;
                    case 3:
                        if (_vcfroot.CRUISE.UseExtras.Extra3)
                        {
                            e.TurnedOn = !e.TurnedOn;
                        }
                        else
                        {
                            e.TurnedOn = false;
                        }
                        break;
                    case 4:
                        if (_vcfroot.CRUISE.UseExtras.Extra4)
                        {
                            e.TurnedOn = !e.TurnedOn;
                        }
                        else
                        {
                            e.TurnedOn = false;
                        }
                        break;
                }
            }
            crsLights = !crsLights;
            ElsUiPanel.ToggleUiBtnState(crsLights, "CRS");
            ElsUiPanel.PlayUiSound(crsLights);
        }

        internal void ToggleTdl()
        {
            _extras.TDL.TurnedOn = !_extras.TDL.State;
            ElsUiPanel.ToggleUiBtnState(_extras.TDL.TurnedOn, "TDL");
            ElsUiPanel.PlayUiSound(_extras.TDL.TurnedOn);
        }

        internal void ToggleScl()
        {
            _extras.SCL.TurnedOn = !_extras.SCL.State;
            ElsUiPanel.ToggleUiBtnState(_extras.SCL.TurnedOn, "SCL");
            ElsUiPanel.PlayUiSound(_extras.SCL.TurnedOn);
        }

        internal async void ChgPrmPatt(bool decrement)
        {
            if (decrement)
            {
                if (CurrentPrmPattern == 0)
                {
                    CurrentPrmPattern = _prmPatterns - 1;
                }
                else
                {
                    CurrentPrmPattern--;
                }
                ElsUiPanel.PlayUiSound(false);
            }
            else
            {
                if (CurrentPrmPattern == _prmPatterns - 1)
                {
                    CurrentPrmPattern = 0;
                }
                else
                {
                    CurrentPrmPattern++;
                }
                ElsUiPanel.PlayUiSound(true);
            }
        }

        internal async void ChgSecPatt(bool decrement)
        {
            if (decrement)
            {
                if (CurrentSecPattern == 0)
                {
                    CurrentSecPattern = _secPatterns - 1;
                }
                else
                {
                    CurrentSecPattern--;
                }
                ElsUiPanel.PlayUiSound(false);
            }
            else
            {
                if (CurrentSecPattern == _secPatterns - 1)
                {
                    CurrentSecPattern = 0;
                }
                else
                {
                    CurrentSecPattern++;
                }
                ElsUiPanel.PlayUiSound(true);
            }
        }

        internal async void ChgWrnPatt(bool decrement)
        {
            if (decrement)
            {
                if (CurrentWrnPattern == 0)
                {
                    CurrentWrnPattern = _wrnPatterns - 1;
                }
                else
                {
                    CurrentWrnPattern--;
                }
                ElsUiPanel.PlayUiSound(false);
            }
            else
            {
                if (CurrentWrnPattern == _wrnPatterns - 1)
                {
                    CurrentWrnPattern = 0;
                }
                else
                {
                    CurrentWrnPattern++;
                }
                ElsUiPanel.PlayUiSound(true);
            }
        }

        int prmScan = 0;
        int secScan = 0;
        int wrnScan = 0;
        internal async Task ToggleScanPattern()
        {
            switch (_stage.CurrentStage)
            {
                case 0:
                    break;
                case 1:
                    if (_stage.SECL.ScanPatternCustomPool.Enabled)
                    {

                        if (_stage.SECL.ScanPatternCustomPool.Sequential)
                        {
                            CurrentSecPattern = SecScanPatts[secScan];
                            secScan++;
                            if (secScan > SecScanPatts.Count - 1)
                            {
                                secScan = 0;
                            }
                        }
                        else
                        {
                            Random rand = new Random();
                            CurrentSecPattern = SecScanPatts[rand.Next(0, SecScanPatts.Count - 1)];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                        {
                            if (CurrentSecPattern == _secPatterns - 1)
                            {
                                CurrentSecPattern = 0;
                            }
                            else
                            {
                                CurrentSecPattern++;
                            }
                        }
                    }
                    break;
                case 2:
                    #region SECL
                    if (_stage.SECL.ScanPatternCustomPool.Enabled)
                    {

                        if (_stage.SECL.ScanPatternCustomPool.Sequential)
                        {
                            CurrentSecPattern = SecScanPatts[secScan];
                            secScan++;
                            if (secScan > SecScanPatts.Count - 1)
                            {
                                secScan = 0;
                            }
                        }
                        else
                        {
                            Random rand = new Random();
                            CurrentSecPattern = SecScanPatts[rand.Next(0, SecScanPatts.Count - 1)];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg1.Pattern) && _stage.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                        {
                            if (CurrentSecPattern == _secPatterns - 1)
                            {
                                CurrentSecPattern = 0;
                            }
                            else
                            {
                                CurrentSecPattern++;
                            }
                        }
                    }
                    #endregion
                    #region PRML
                    if (_stage.PRML.ScanPatternCustomPool.Enabled)
                    {

                        if (_stage.PRML.ScanPatternCustomPool.Sequential)
                        {
                            CurrentPrmPattern = PrmScanPatts[prmScan];
                            prmScan++;
                            if (prmScan > PrmScanPatts.Count - 1)
                            {
                                prmScan = 0;
                            }
                        }
                        else
                        {
                            Random rand = new Random();
                            CurrentPrmPattern = PrmScanPatts[rand.Next(0, PrmScanPatts.Count - 1)];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg2.Pattern) && _stage.PRML.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                        {
                            if (CurrentPrmPattern == _prmPatterns - 1)
                            {
                                CurrentPrmPattern = 0;
                            }
                            else
                            {
                                CurrentPrmPattern++;
                            }
                        }
                    }
                    #endregion
                    break;
                case 3:
                    #region Wrn
                    if (_stage.WRNL.ScanPatternCustomPool.Enabled)
                    {

                        if (_stage.WRNL.ScanPatternCustomPool.Sequential)
                        {
                            CurrentWrnPattern = WrnScanPatts[wrnScan];
                            wrnScan++;
                            if (wrnScan > WrnScanPatts.Count - 1)
                            {
                                wrnScan = 0;
                            }
                        }
                        else
                        {
                            Random rand = new Random();
                            CurrentWrnPattern = WrnScanPatts[rand.Next(0, WrnScanPatts.Count - 1)];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_stage.WRNL.PresetPatterns.Lstg3.Pattern) && _stage.WRNL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                        {
                            if (CurrentWrnPattern == _wrnPatterns - 1)
                            {
                                CurrentWrnPattern = 0;
                            }
                            else
                            {
                                CurrentWrnPattern++;
                            }
                        }
                    }
                    #endregion
                    #region SECL
                    if (_stage.SECL.ScanPatternCustomPool.Enabled)
                    {

                        if (_stage.SECL.ScanPatternCustomPool.Sequential)
                        {
                            CurrentSecPattern = SecScanPatts[secScan];
                            secScan++;
                            if (secScan > SecScanPatts.Count - 1)
                            {
                                secScan = 0;
                            }
                        }
                        else
                        {
                            Random rand = new Random();
                            CurrentSecPattern = SecScanPatts[rand.Next(0, SecScanPatts.Count - 1)];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_stage.SECL.PresetPatterns.Lstg3.Pattern) && _stage.SECL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                        {
                            if (CurrentSecPattern == _secPatterns - 1)
                            {
                                CurrentSecPattern = 0;
                            }
                            else
                            {
                                CurrentSecPattern++;
                            }
                        }
                    }
                    #endregion
                    #region PRML
                    if (_stage.PRML.ScanPatternCustomPool.Enabled)
                    {

                        if (_stage.PRML.ScanPatternCustomPool.Sequential)
                        {
                            CurrentPrmPattern = PrmScanPatts[prmScan];
                            prmScan++;
                            if (prmScan > PrmScanPatts.Count - 1)
                            {
                                prmScan = 0;
                            }
                        }
                        else
                        {
                            Random rand = new Random();
                            CurrentPrmPattern = PrmScanPatts[rand.Next(0, PrmScanPatts.Count - 1)];
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_stage.PRML.PresetPatterns.Lstg3.Pattern) && _stage.PRML.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                        {
                            if (CurrentPrmPattern == _prmPatterns - 1)
                            {
                                CurrentPrmPattern = 0;
                            }
                            else
                            {
                                CurrentPrmPattern++;
                            }
                        }
                    }
                    #endregion
                    break;
            }
        }

        bool prmLights = false;
        internal async void ToggleLightStage()
        {
            await _stage.NextStage(false);
            Utils.DebugWriteLine($"Current Lightstage is {_stage.CurrentStage}");
            SetStage();
        }

        internal async void ToggleLightStageInverse()
        {
            await _stage.NextStage(true);
            Utils.DebugWriteLine($"Current Inverse Lightstage is {_stage.CurrentStage}");
            SetStage();
        }

        private void SetStage()
        {
            int[] extras = _stage.GetStage2Extras();
            switch (_stage.CurrentStage)
            {
                case 0:
                    SetGTASirens(false);
                    //foreach (Extra.Extra e in _extras.PRML.Values)
                    for (int i = 0; i < _extras.PRML.Count; i++)
                    {
                        _extras.PRML.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    //foreach (Extra.Extra e in _extras.SECL.Values)
                    for (int i = 0; i < _extras.SECL.Count; i++)
                    {
                        _extras.SECL.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    //foreach (Extra.Extra e in _extras.WRNL.Values)
                    for (int i = 0; i < _extras.WRNL.Count; i++)
                    {
                        _extras.WRNL.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    if (_vcfroot.SECL.LightingFormat.ToLower().Equals("chp"))
                    {
                        _extras.SBRN.SetState(false);
                    }
                    if (_vcfroot.MISC.UseSteadyBurnLights && _extras.SBRN != null)
                    {
                        _extras.SBRN.SetState(false);
                    }
                    prmLights = false;
                    secLights = false;
                    wrnLights = false;
                    ElsUiPanel.ToggleUiBtnState(prmLights, "PRML");
                    ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
                    ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
                    ElsUiPanel.PlayUiSound(false);
                    break;
                case 1:
                    if (_vcfroot.MISC.DfltSirenLtsActivateAtLstg == 1)
                    {
                        SetGTASirens(true);
                    }
                    //foreach (Extra.Extra e in _extras.SECL.Values)
                    for (int i = 0; i < _extras.SECL.Count; i++)
                    {
                        if (_stage.SECL.PresetPatterns.Lstg1.Enabled)
                        {
                            if (_stage.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentSecPattern = _stage.SECL.PresetPatterns.Lstg1.IntPattern;
                            }
                        }
                        else
                        {

                        }
                        _extras.SECL.ElementAt(i).Value.IsPatternRunning = true;
                    }
                    if (_vcfroot.SECL.LightingFormat.ToLower().Equals("chp"))
                    {
                        SetCHP();
                        //_extras.SBRN.Pattern = CHP.LightStage1[10];
                        //_extras.SBRN.IsPatternRunning = true;
                        _extras.SBRN.SetState(true);
                    }
                    //foreach (Extra.Extra e in _extras.PRML.Values)
                    for (int i = 0; i < _extras.PRML.Count; i++)
                    {
                        _extras.PRML.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    //foreach (Extra.Extra e in _extras.WRNL.Values)
                    for (int i = 0; i < _extras.WRNL.Count; i++)
                    {
                        _extras.WRNL.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    if (_vcfroot.MISC.UseSteadyBurnLights && _extras.SBRN != null)
                    {
                        _extras.SBRN.SetState(true);
                    }
                    prmLights = false;
                    secLights = true;
                    wrnLights = false;
                    ElsUiPanel.ToggleUiBtnState(prmLights, "PRML");
                    ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
                    ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
                    ElsUiPanel.PlayUiSound(true);
                    break;
                case 2:
                    if (_vcfroot.MISC.DfltSirenLtsActivateAtLstg == 2)
                    {
                        SetGTASirens(true);
                    }
                    //foreach (Extra.Extra e in _extras.PRML.Values)
                    for (int i = 0; i < _extras.PRML.Count; i++)
                    {
                        _extras.PRML.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    //foreach (Extra.Extra e in _extras.WRNL.Values)
                    for (int i = 0; i < _extras.WRNL.Count; i++)
                    {
                        _extras.WRNL.ElementAt(i).Value.IsPatternRunning = false;
                    }
                    //foreach (Extra.Extra e in _extras.SECL.Values)
                    for (int i = 0; i < _extras.SECL.Count; i++)
                    {
                        if (_stage.SECL.PresetPatterns.Lstg2.Enabled)
                        {
                            if (_stage.SECL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentSecPattern = _stage.SECL.PresetPatterns.Lstg2.IntPattern;
                            }
                        }
                        else
                        {
                        }
                        _extras.SECL.ElementAt(i).Value.IsPatternRunning = false;
                        _extras.SECL.ElementAt(i).Value.IsPatternRunning = true;
                    }
                    secLights = true;
                    //foreach (int i in extras)
                    for(int i = 0; i < extras.Length; i++) 
                    {
                        if (_extras.PRML.ContainsKey(i))
                        {
                            if (_stage.PRML.PresetPatterns.Lstg2.Enabled)
                            {
                                if (_stage.PRML.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                                {
                                    _scan = true;
                                }
                                else
                                {
                                    CurrentPrmPattern = _stage.PRML.PresetPatterns.Lstg2.IntPattern;
                                }
                            }
                            else
                            {

                            }
                            _extras.PRML.ElementAt(i).Value.IsPatternRunning = false;
                            _extras.PRML.ElementAt(i).Value.IsPatternRunning = true;
                        }
                    }
                    if (_vcfroot.PRML.LightingFormat.ToLower().Equals("chp"))
                    {
                        SetCHP();
                        //_extras.SBRN.Pattern = CHP.LightStage2[CurrentSecPattern][10];
                        //_extras.SBRN.IsPatternRunning = true;
                        _extras.SBRN.SetState(true);
                    }
                    if (_vcfroot.MISC.UseSteadyBurnLights && _extras.SBRN != null)
                    {
                        _extras.SBRN.SetState(true);
                    }
                    prmLights = true;
                    secLights = true;
                    wrnLights = false;
                    ElsUiPanel.ToggleUiBtnState(prmLights, "PRML");
                    ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
                    ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
                    ElsUiPanel.PlayUiSound(true);
                    break;
                case 3:
                    if (_vcfroot.MISC.DfltSirenLtsActivateAtLstg == 3)
                    {
                        SetGTASirens(true);
                    }
                    //foreach (Extra.Extra e in _extras.SECL.Values)
                    for (int i = 0; i < _extras.SECL.Count; i++)
                    {

                        if (_stage.SECL.PresetPatterns.Lstg3.Enabled)
                        {
                            if (_stage.SECL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentSecPattern = _stage.SECL.PresetPatterns.Lstg3.IntPattern;
                            }
                        }
                        if (_vcfroot.SECL.DisableAtLstg3)
                        {
                            _extras.SECL.ElementAt(i).Value.IsPatternRunning = false;
                            secLights = false;
                            ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
                        }
                        else
                        {
                            _extras.SECL.ElementAt(i).Value.IsPatternRunning = false;
                            _extras.SECL.ElementAt(i).Value.IsPatternRunning = true;
                            secLights = true;
                            ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
                        }
                    }
                    //foreach (Extra.Extra e in _extras.PRML.Values)
                    for (int i = 0; i < _extras.PRML.Count; i++)
                    {
                        if (_stage.PRML.PresetPatterns.Lstg3.Enabled)
                        {
                            if (_stage.PRML.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentPrmPattern = _stage.PRML.PresetPatterns.Lstg3.IntPattern;
                            }
                        }
                        if (_vcfroot.PRML.DisableAtLstg3)
                        {
                            _extras.PRML.ElementAt(i).Value.IsPatternRunning = false;
                            prmLights = false;
                            ElsUiPanel.ToggleUiBtnState(prmLights, "PRML");
                        }
                        else
                        {
                            _extras.PRML.ElementAt(i).Value.IsPatternRunning = false;
                            _extras.PRML.ElementAt(i).Value.IsPatternRunning = true;
                            prmLights = true;
                            ElsUiPanel.ToggleUiBtnState(prmLights, "PRML");
                        }
                    }
                    //foreach (Extra.Extra e in _extras.WRNL.Values)
                    for (int i = 0; i < _extras.WRNL.Count; i++)
                    {
                        if (_stage.WRNL.PresetPatterns.Lstg3.Enabled)
                        {
                            if (_stage.WRNL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentWrnPattern = _stage.WRNL.PresetPatterns.Lstg3.IntPattern;
                            }
                        }
                        if (_vcfroot.WRNL.DisableAtLstg3)
                        {
                            _extras.WRNL.ElementAt(i).Value.IsPatternRunning = false;
                            wrnLights = false;
                            ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
                        }
                        else
                        {
                            _extras.WRNL.ElementAt(i).Value.IsPatternRunning = false;
                            _extras.WRNL.ElementAt(i).Value.IsPatternRunning = true;
                            wrnLights = true;
                            ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
                        }
                    }
                    if (_vcfroot.PRML.LightingFormat.ToLower().Equals("chp"))
                    {
                        SetCHP();
                        //_extras.SBRN.Pattern = CHP.LightStage3[CurrentWrnPattern][10];
                        //_extras.SBRN.IsPatternRunning = true;
                        _extras.SBRN.SetState(true);
                    }
                    if (_vcfroot.MISC.UseSteadyBurnLights && _extras.SBRN != null)
                    {
                        _extras.SBRN.SetState(true);
                    }
                    ElsUiPanel.PlayUiSound(true);
                    break;
            }
        }
    }
}
