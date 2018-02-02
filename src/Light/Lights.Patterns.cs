using ELS.configuration;
using ELS.Light.Patterns;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    partial class Lights : IPatterns
    {
        private int _prmPatt = 0;
        private int _secPatt = 0;
        private int _wrnPatt = 0;
        private string _prefix;
        private int _prmPatterns;
        private int _secPatterns;
        private int _wrnPatterns;
        internal List<int> PrmScanPatts;
        internal List<int> SecScanPatts;
        internal List<int> WrnScanPatts;

        public int CurrentPrmPattern
        {
            get
            {
                return _prmPatt;
            }
            set
            {
                _prmPatt = value;
                foreach (Extra.Extra e in _extras.PRML.Values)
                {
                    try
                    {
                        switch (_vcfroot.PRML.LightingFormat.ToLower())
                        {
                            case "leds":
                                e.Pattern = Leds.PrimaryPatterns[CurrentPrmPattern][e.Id];
                                _prmPatterns = Leds.PrimaryPatterns.Count;
                                _prefix = "L";
                                break;
                            case "strb":
                                e.Pattern = Strobe.PrimaryPatterns[CurrentPrmPattern][e.Id];
                                _prmPatterns = Strobe.PrimaryPatterns.Count;
                                _prefix = "S";
                                break;
                            case "rota":
                                e.Pattern = Rotary.PrimaryPatterns[CurrentPrmPattern][e.Id];
                                _prmPatterns = Rotary.PrimaryPatterns.Count;
                                _prefix = "R";
                                break;
                            case "dro1":
                                if (e.Id == 1)
                                {
                                    e.SetState(true);
                                }
                                e.Pattern = DRO.PrimaryPatternsDro1[CurrentPrmPattern][e.Id];
                                _prmPatterns = DRO.PrimaryPatternsDro1.Count;
                                _prefix = "D";
                                break;
                            case "dro2":
                                if (e.Id == 1)
                                {
                                    e.SetState(false);
                                }
                                e.Pattern = DRO.PrimaryPatternsDro2[CurrentPrmPattern][e.Id];
                                _prmPatterns = DRO.PrimaryPatternsDro2.Count;
                                _prefix = "D";
                                break;
                            case "dro3":
                                if (e.Id == 1 || e.Id == 2)
                                {
                                    e.SetState(true);
                                }
                                e.Pattern = DRO.PrimaryPatternsDro3[CurrentPrmPattern][e.Id];
                                _prmPatterns = DRO.PrimaryPatternsDro3.Count;
                                _prefix = "D";
                                break;
                            case "chp":
                                switch (_stage.CurrentStage)
                                {
                                    case 1:
                                        e.Pattern = CHP.LightStage1[e.Id];
                                        _prmPatterns = 1;
                                        break;
                                    case 2:
                                        e.Pattern = CHP.LightStage2[CurrentSecPattern][e.Id];
                                        _prmPatterns = 1;
                                        _wrnPatterns = 1;
                                        _secPatterns = CHP.LightStage2.Count;
                                        break;
                                    case 3:
                                        e.Pattern = CHP.LightStage3[CurrentWrnPattern][e.Id];
                                        _prmPatterns = 1;
                                        _secPatterns = 1;
                                        _wrnPatterns = CHP.LightStage3.Count;
                                        break;
                                    default:
                                        e.Pattern = CHP.LightStage1[e.Id];
                                        _prmPatterns = 1;
                                        _wrnPatterns = 1;
                                        _secPatterns = 1;
                                        break;
                                }
                                _prefix = "C";
                                break;
                            default:
                                e.Pattern = Leds.PrimaryPatterns[CurrentPrmPattern][e.Id];
                                _prefix = "L";
                                break;
                        }
                        e.PatternNum = CurrentPrmPattern;
                        e.Delay = Global.PrimDelay;
                    }
                    catch (Exception ex)
                    {
                        CitizenFX.Core.Debug.WriteLine($"{e.Id} error {ex.Message}");
                    }
                }

               Utils.DebugWriteLine($"Current primary pattern is {CurrentPrmPattern}");
               ElsUiPanel.SetUiDesc(_prefix + CurrentPrmPattern.ToString().PadLeft(3,'0'), ExtraEnum.PRML.ToString());
            }
        }

        public int CurrentSecPattern
        {
            get
            {
                return _secPatt;
            }
            set
            {
                _secPatt = value;
                foreach (Extra.Extra e in _extras.SECL.Values)
                {
                    try
                    {
                        switch (_vcfroot.SECL.LightingFormat.ToLower())
                        {
                            case "leds":
                                e.Pattern = Leds.SecondaryPatterns[CurrentSecPattern][e.Id];
                                _secPatterns = Leds.SecondaryPatterns.Count;
                                _prefix = "L";
                                break;
                            case "strb":
                                e.Pattern = Strobe.SecondaryPatterns[CurrentSecPattern][e.Id];
                                _secPatterns = Strobe.SecondaryPatterns.Count;
                                _prefix = "S";
                                break;
                            case "traf":
                                e.Pattern = TrafficAdvisor.SecondaryPatterns[CurrentSecPattern][e.Id];
                                _secPatterns = TrafficAdvisor.SecondaryPatterns.Count;
                                _prefix = "T";
                                break;
                            case "arrw":
                                e.Pattern = Arrow.SecondaryPatterns[CurrentSecPattern][e.Id];
                                _secPatterns = Arrow.SecondaryPatterns.Count;
                                _prefix = "A";
                                break;
                            case "marq":
                                e.Pattern = Marquee.SecondaryPatterns[CurrentSecPattern][e.Id];
                                _secPatterns = Marquee.SecondaryPatterns.Count;
                                _prefix = "M";
                                break;
                            case "chp":
                                switch (_stage.CurrentStage)
                                {
                                    case 1:
                                        e.Pattern = CHP.LightStage1[e.Id];
                                        break;
                                    case 2:
                                        e.Pattern = CHP.LightStage2[CurrentSecPattern][e.Id];
                                        break;
                                    case 3:
                                        e.Pattern = CHP.LightStage3[CurrentWrnPattern][e.Id];
                                        break;
                                    default:
                                        e.Pattern = CHP.LightStage1[e.Id];
                                        break;
                                }
                                _prefix = "C";
                                break;
                            default:
                                e.Pattern = Leds.PrimaryPatterns[CurrentSecPattern][e.Id];
                                _prefix = "L";
                                break;
                        }
                        e.PatternNum = CurrentSecPattern;
                        e.Delay = Global.PrimDelay;
                    }
                    catch (Exception ex)
                    {
                        CitizenFX.Core.Debug.WriteLine($"{e.Id} error {ex.Message}");
                    }
                }
                ElsUiPanel.SetUiDesc(_prefix + CurrentSecPattern.ToString().PadLeft(3,'0'), ExtraEnum.SECL.ToString());
                 Utils.DebugWriteLine($"Current secondary pattern is {CurrentSecPattern}");
            }
        }

        public int CurrentWrnPattern
        {
            get
            {
                return _wrnPatt;
            }
            set
            {
                _wrnPatt = value;
                foreach (Extra.Extra e in _extras.WRNL.Values)
                {
                    try
                    {
                        switch (_vcfroot.WRNL.LightingFormat.ToLower())
                        {
                            case "leds":
                                e.Pattern = Leds.WarningPatterns[CurrentWrnPattern][e.Id];
                                _wrnPatterns = Leds.WarningPatterns.Count;
                                _prefix = "L";
                                break;
                            case "strb":
                                e.Pattern = Strobe.WarningPatterns[CurrentWrnPattern][e.Id];
                                _wrnPatterns = Strobe.WarningPatterns.Count;
                                _prefix = "S";
                                break;
                            case "chp":
                                switch (_stage.CurrentStage)
                                {
                                    case 1:
                                        e.Pattern = CHP.LightStage1[e.Id];
                                        break;
                                    case 2:
                                        e.Pattern = CHP.LightStage2[CurrentSecPattern][e.Id];
                                        break;
                                    case 3:
                                        e.Pattern = CHP.LightStage3[CurrentWrnPattern][e.Id];
                                        break;
                                    default:
                                        e.Pattern = CHP.LightStage1[e.Id];
                                        break;
                                }
                                _prefix = "C";
                                break;
                            default:
                                e.Pattern = Leds.WarningPatterns[CurrentWrnPattern][e.Id];
                                _prefix = "L";
                                break;
                        }
                        e.PatternNum = CurrentWrnPattern;
                        e.Delay = Global.PrimDelay;
                    }
                    catch (Exception ex)
                    {
                        CitizenFX.Core.Debug.WriteLine($"{e.Id} error {ex.Message}");
                    }
                }
                ElsUiPanel.SetUiDesc(_prefix + CurrentWrnPattern.ToString().PadLeft(3,'0'), ExtraEnum.WRNL.ToString());
                Utils.DebugWriteLine($"Current warning pattern is {CurrentWrnPattern}");
            }
        }

        private void SetupPatternsPrm()
        {
            CurrentPrmPattern = 0;
            PrmScanPatts = new List<int>();
            if (bool.Parse(_stage.PRML.ScanPatternCustomPool.Enabled)) {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"Adding Primary Scan pool patterns");
#endif
                foreach (string p in _stage.PRML.ScanPatternCustomPool.Pattern)
                {
                    PrmScanPatts.Add(int.Parse(p));
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Added {p} to primary scan patterns");
#endif
                }
                CurrentPrmPattern = PrmScanPatts[0];
            }
            else
            {

            }
        }

        private void SetupSecPatterns()
        {
            CurrentSecPattern = 0;
            SecScanPatts = new List<int>();
            if (bool.Parse(_stage.SECL.ScanPatternCustomPool.Enabled))
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"Adding Secondary Scan pool patterns");
#endif
                foreach (string p in _stage.SECL.ScanPatternCustomPool.Pattern)
                {
                    SecScanPatts.Add(int.Parse(p));
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Added {p} to secondary scan patterns");
#endif
                }
                CurrentSecPattern = SecScanPatts[0];
            }
        }

        private void SetupWrnPatterns()
        {
            CurrentWrnPattern = 0;
            WrnScanPatts = new List<int>();
            if (bool.Parse(_stage.WRNL.ScanPatternCustomPool.Enabled))
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"Adding Warning Scan pool patterns");
#endif
                foreach (string p in _stage.WRNL.ScanPatternCustomPool.Pattern)
                {
                    WrnScanPatts.Add(int.Parse(p));
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Added {p} to warning scan patterns");
#endif
                }
                CurrentWrnPattern = WrnScanPatts[0];
            }
        }

        internal void SetCHP()
        {
            CurrentPrmPattern = 0;
            CurrentSecPattern = 0;
            CurrentWrnPattern = 0;
        }
    }
}
