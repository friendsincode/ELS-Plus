using CitizenFX.Core;
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
                            case "custom":
                                e.Pattern = _vcfroot.CustomPatterns.Values.ElementAt(CurrentPrmPattern).PatternData[e.Id];
                                e.Delay = _vcfroot.CustomPatterns.Values.ElementAt(CurrentPrmPattern).PrmDelay;
                                _prmPatterns = _vcfroot.CustomPatterns.Count;
                                Utils.DebugWriteLine($"Got custom patterns for extra {e.Id} with {e.Pattern}  at {e.Delay}");
                                break;
                            default:
                                e.Pattern = Leds.PrimaryPatterns[CurrentPrmPattern][e.Id];
                                _prefix = "L";
                                break;
                        }
                        e.PatternNum = CurrentPrmPattern;
                        
                    }
                    catch (Exception ex)
                    {
                        CitizenFX.Core.Debug.WriteLine($"{e.Id} error {ex.Message}");
                    }
                }

               Utils.DebugWriteLine($"Current primary pattern is {CurrentPrmPattern}");
                if (Game.PlayerPed.IsInPoliceVehicle && Game.PlayerPed.CurrentVehicle.GetNetworkId() == _vehicle.GetNetworkId())
                {
                    if (_vcfroot.PRML.LightingFormat.Equals("custom"))
                    {
                        string input = _vcfroot.CustomPatterns.Keys.ElementAt(CurrentPrmPattern);
                        ElsUiPanel.SetUiDesc(input.First().ToString().ToUpper() + input.Substring(1), ExtraEnum.PRML.ToString());
                    }
                    else
                    {
                        ElsUiPanel.SetUiDesc(_prefix + CurrentPrmPattern.ToString().PadLeft(3, '0'), ExtraEnum.PRML.ToString());
                    }
                }
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
                            case "custom":
                                e.Pattern = _vcfroot.CustomPatterns.Values.ElementAt(CurrentSecPattern).PatternData[e.Id];
                                e.Delay = _vcfroot.CustomPatterns.Values.ElementAt(CurrentSecPattern).SecDelay;
                                _secPatterns = _vcfroot.CustomPatterns.Count;
                                Utils.DebugWriteLine($"Got custom patterns for extra {e.Id} with {e.Pattern}  at {e.Delay}");
                                break;
                            default:
                                e.Pattern = Leds.PrimaryPatterns[CurrentSecPattern][e.Id];
                                _prefix = "L";
                                break;
                        }
                        e.PatternNum = CurrentSecPattern;
                    }
                    catch (Exception ex)
                    {
                        CitizenFX.Core.Debug.WriteLine($"{e.Id} error {ex.Message}");
                    }
                }
                if (Game.PlayerPed.IsInPoliceVehicle && Game.PlayerPed.CurrentVehicle.GetNetworkId() == _vehicle.GetNetworkId())
                {
                    if (_vcfroot.SECL.LightingFormat.Equals("custom"))
                    {
                        string input = _vcfroot.CustomPatterns.Keys.ElementAt(CurrentSecPattern);
                        ElsUiPanel.SetUiDesc(input.First().ToString().ToUpper() + input.Substring(1), ExtraEnum.SECL.ToString());
                    }
                    else
                    {
                        ElsUiPanel.SetUiDesc(_prefix + CurrentSecPattern.ToString().PadLeft(3, '0'), ExtraEnum.SECL.ToString());
                    }
                }
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
                            case "custom":
                                e.Pattern = _vcfroot.CustomPatterns.Values.ElementAt(CurrentWrnPattern).PatternData[e.Id];
                                e.Delay = _vcfroot.CustomPatterns.Values.ElementAt(CurrentWrnPattern).SecDelay * 1000;
                                _wrnPatterns = _vcfroot.CustomPatterns.Count;
                                Utils.DebugWriteLine($"Got custom patterns for extra {e.Id} with {e.Pattern}  at {e.Delay}");
                                break;
                            default:
                                e.Pattern = Leds.WarningPatterns[CurrentWrnPattern][e.Id];
                                _prefix = "L";
                                break;
                        }
                        e.PatternNum = CurrentWrnPattern;
                    }
                    catch (Exception ex)
                    {
                        CitizenFX.Core.Debug.WriteLine($"{e.Id} error {ex.Message}");
                    }
                }
                if (Game.PlayerPed.IsInPoliceVehicle && Game.PlayerPed.CurrentVehicle.GetNetworkId() == _vehicle.GetNetworkId())
                {
                    if (_vcfroot.SECL.LightingFormat.Equals("custom"))
                    {
                        string input = _vcfroot.CustomPatterns.Keys.ElementAt(CurrentWrnPattern);
                        ElsUiPanel.SetUiDesc(input.First().ToString().ToUpper() + input.Substring(1), ExtraEnum.WRNL.ToString());
                    }
                    else
                    {
                        ElsUiPanel.SetUiDesc(_prefix + CurrentWrnPattern.ToString().PadLeft(3, '0'), ExtraEnum.WRNL.ToString());
                    }
                }
                Utils.DebugWriteLine($"Current warning pattern is {CurrentWrnPattern}");
            }
        }

        public int CurrentStage
        { get { return _stage.CurrentStage; } }

        private void SetupPatternsPrm()
        {
            CurrentPrmPattern = 0;
            PrmScanPatts = new List<int>();
            if (_stage.PRML.ScanPatternCustomPool.Enabled) {
                Utils.DebugWriteLine($"Adding Primary Scan pool patterns");
                foreach (int p in _stage.PRML.ScanPatternCustomPool.Pattern)
                {
                    PrmScanPatts.Add(p);
                    Utils.DebugWriteLine($"Added {p} to primary scan patterns");
                }
                CurrentPrmPattern = PrmScanPatts[0];
            }
            if (_extras.PRML.Count == 0)
            {
                _prmPatterns = 0;
                ElsUiPanel.SetUiDesc("--", ExtraEnum.PRML.ToString());
            }
        }

        private void SetupSecPatterns()
        {
            CurrentSecPattern = 0;
            SecScanPatts = new List<int>();
            if (_stage.SECL.ScanPatternCustomPool.Enabled)
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"Adding Secondary Scan pool patterns");
#endif
                foreach (int p in _stage.SECL.ScanPatternCustomPool.Pattern)
                {
                    SecScanPatts.Add(p);
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Added {p} to secondary scan patterns");
#endif
                }
                CurrentSecPattern = SecScanPatts[0];
            }
            if (_extras.PRML.Count == 0)
            {
                _secPatterns = 0;
                ElsUiPanel.SetUiDesc("--", ExtraEnum.SECL.ToString());
            }
        }

        private void SetupWrnPatterns()
        {
            CurrentWrnPattern = 0;
            WrnScanPatts = new List<int>();
            if (_stage.WRNL.ScanPatternCustomPool.Enabled)
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine($"Adding Warning Scan pool patterns");
#endif
                foreach (int p in _stage.WRNL.ScanPatternCustomPool.Pattern)
                {
                    WrnScanPatts.Add(p);
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Added {p} to warning scan patterns");
#endif
                }
                CurrentWrnPattern = WrnScanPatts[0];
            }
            if (_extras.PRML.Count == 0)
            {
                _wrnPatterns = 0;
                ElsUiPanel.SetUiDesc("--", ExtraEnum.WRNL.ToString());
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
