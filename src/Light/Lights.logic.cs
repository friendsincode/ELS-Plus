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
            foreach (Extra.Extra ex in _extras.SECL.Values)
            {
                if (ex.IsPatternRunning)
                {
                    ex.IsPatternRunning = false;
                    ex.CleanUp();
                }
                else
                {
                    ex.IsPatternRunning = true;
                }
            }
            secLights = !secLights;
            ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
        }

        bool wrnLights = false;
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
                    ex.IsPatternRunning = true;
                }
            }
            wrnLights = !wrnLights;
            ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
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
                        } else
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
        }

        internal void ToggleTdl()
        {
            _extras.TDL.TurnedOn = !_extras.TDL.State;
            ElsUiPanel.ToggleUiBtnState(_extras.TDL.TurnedOn, "TDL");
        }

        internal void ToggleScl()
        {
            _extras.SCL.TurnedOn = !_extras.SCL.State;
            ElsUiPanel.ToggleUiBtnState(_extras.SCL.TurnedOn, "SCL");
        }

        internal async void ChgPrmPatt()
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

        internal async void ChgSecPatt()
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

        internal async void ChgWrnPatt()
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
                    if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Enabled))
                    {
                        
                        if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Sequential))
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
                            CurrentSecPattern = SecScanPatts[rand.Next(0,SecScanPatts.Count -1)];
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
                    if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Enabled))
                    {

                        if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Sequential))
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
                    if (bool.Parse(_stage.PRML.ScanPatternCustomPool.Enabled))
                    {

                        if (bool.Parse(_stage.PRML.ScanPatternCustomPool.Sequential))
                        {
                            CurrentPrmPattern = PrmScanPatts[prmScan];
                            prmScan++;
                            if (prmScan > PrmScanPatts.Count -1)
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
                    if (bool.Parse(_stage.WRNL.ScanPatternCustomPool.Enabled))
                    {

                        if (bool.Parse(_stage.WRNL.ScanPatternCustomPool.Sequential))
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
                    if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Enabled))
                    {

                        if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Sequential))
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
                    if (bool.Parse(_stage.PRML.ScanPatternCustomPool.Enabled))
                    {

                        if (bool.Parse(_stage.PRML.ScanPatternCustomPool.Sequential))
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


        internal async void ToggleLightStage()
        {
            await _stage.NextStage();
            Screen.ShowNotification($"Current Lightstage is {_stage.CurrentStage}");
            int[] extras = _stage.GetStage2Extras();
            switch (_stage.CurrentStage)
            {
                case 0:
                    SetGTASirens(false);
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
                    ElsUiPanel.ToggleUiBtnState(false, "PRML");
                    ElsUiPanel.ToggleUiBtnState(false, "SECL");
                    ElsUiPanel.ToggleUiBtnState(false, "WRNL");
                    secLights = false;
                    wrnLights = false;
                    break;
                case 1:
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg1.Enabled))
                        {
                            if (_stage.SECL.PresetPatterns.Lstg1.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentSecPattern = int.Parse(_stage.SECL.PresetPatterns.Lstg1.Pattern);
                            }
                        }
                        else
                        {

                        }
                        e.IsPatternRunning = true;
                    }
                    ElsUiPanel.ToggleUiBtnState(true, "SECL");
                    secLights = true;
                    break;
                case 2:
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {
                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg2.Enabled))
                        {
                            if (_stage.SECL.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentSecPattern = int.Parse(_stage.SECL.PresetPatterns.Lstg2.Pattern);
                            }
                        }
                        else
                        {
                        }
                        e.IsPatternRunning = false;
                        e.IsPatternRunning = true;
                    }
                    secLights = true;
                    foreach (int i in extras)
                    {
                        Extra.Extra e = _extras.PRML[i];
                        if (bool.Parse(_stage.PRML.PresetPatterns.Lstg2.Enabled))
                        {
                            if (_stage.PRML.PresetPatterns.Lstg2.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentPrmPattern = int.Parse(_stage.PRML.PresetPatterns.Lstg2.Pattern);
                            }
                        }
                        else
                        {

                        }
                        e.IsPatternRunning = false;
                        e.IsPatternRunning = true;
                    }
                    ElsUiPanel.ToggleUiBtnState(true, "PRML");
                    ElsUiPanel.ToggleUiBtnState(true, "SECL");
                    break;
                case 3:
                    SetGTASirens(true);
                    foreach (Extra.Extra e in _extras.SECL.Values)
                    {

                        if (bool.Parse(_stage.SECL.PresetPatterns.Lstg3.Enabled))
                        {
                            if (_stage.SECL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentSecPattern = int.Parse(_stage.SECL.PresetPatterns.Lstg3.Pattern);
                            }
                        }
                        if (bool.Parse(_vcfroot.SECL.DisableAtLstg3))
                        {
                            e.IsPatternRunning = false;
                            ElsUiPanel.ToggleUiBtnState(false, "SECL");
                        }
                        else
                        {
                            e.IsPatternRunning = false;
                            e.IsPatternRunning = true;
                            ElsUiPanel.ToggleUiBtnState(true, "SECL");
                        }
                    }
                    secLights = true;
                    foreach (Extra.Extra e in _extras.PRML.Values)
                    {
                        if (bool.Parse(_stage.PRML.PresetPatterns.Lstg3.Enabled))
                        {
                            if (_stage.PRML.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentPrmPattern = int.Parse(_stage.PRML.PresetPatterns.Lstg3.Pattern);
                            }
                        }
                        else
                        {
                        }
                        e.IsPatternRunning = false;
                        e.IsPatternRunning = true;
                    }

                    foreach (Extra.Extra e in _extras.WRNL.Values)
                    {
                        if (bool.Parse(_stage.WRNL.PresetPatterns.Lstg3.Enabled))
                        {
                            if (_stage.WRNL.PresetPatterns.Lstg3.Pattern.ToLower().Equals("scan"))
                            {
                                _scan = true;
                            }
                            else
                            {
                                CurrentWrnPattern = int.Parse(_stage.WRNL.PresetPatterns.Lstg3.Pattern);
                            }
                        }
                        e.IsPatternRunning = false;
                        e.IsPatternRunning = true;
                    }
                    wrnLights = true;
                    ElsUiPanel.ToggleUiBtnState(true, "PRML");
                    ElsUiPanel.ToggleUiBtnState(true, "WRNL");
                    break;
            }
        }
    }
}
