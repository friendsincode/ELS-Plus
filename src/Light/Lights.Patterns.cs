using ELS.Light.Patterns;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    partial class Lights
    {
        private Dictionary<int, Pattern> _prmPatterns;
        private Dictionary<int, Pattern> _secPatterns;
        private Dictionary<int, Pattern> _wrnPatterns;
        private int _prmPatt = 0;
        private int _secPatt = 0;
        private int _wrnPatt = 0;

        private int CurrentPrmPattern
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
                    switch (_prmPatterns[CurrentPrmPattern].BasePattern)
                    {
                        case 0:
                            if (e.Id == 1 || e.Id == 2)
                            {
                                e.Pattern = _prmPatterns[CurrentPrmPattern].StringPattern;
                            }
                            else
                            {
                                e.Pattern = PatternUtils.Reverse(_prmPatterns[CurrentPrmPattern].StringPattern);
                            }
                            break;
                        case 1:
                            if (e.Id == 1 || e.Id == 4)
                            {
                                e.Pattern = _prmPatterns[CurrentPrmPattern].StringPattern;
                            }
                            else
                            {
                                e.Pattern = PatternUtils.Reverse(_prmPatterns[CurrentPrmPattern].StringPattern);
                            }
                            break;
                        case 2:
                            if (e.Id == 1 || e.Id == 3)
                            {
                                e.Pattern = _prmPatterns[CurrentPrmPattern].StringPattern;
                            }
                            else
                            {
                                e.Pattern = PatternUtils.Reverse(_prmPatterns[CurrentPrmPattern].StringPattern);
                            }
                            break;
                        case 3:
                            e.Pattern = _prmPatterns[CurrentPrmPattern].StringPattern;
                            break;
                    }
                    e.PatternNum = CurrentPrmPattern;
                    e.Delay = _prmPatterns[CurrentPrmPattern].Delay;
                }
                ElsUiPanel.SetUiPatternNumber(CurrentPrmPattern, ExtraEnum.PRML.ToString());
            }
        }

        private int CurrentSecPattern
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
                    e.PatternNum = CurrentSecPattern;
                    e.Pattern = _secPatterns[CurrentSecPattern].StringPattern;
                }
                ElsUiPanel.SetUiPatternNumber(CurrentSecPattern, ExtraEnum.SECL.ToString());
            }
        }

        private int CurrentWrnPattern
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
                    e.PatternNum = CurrentWrnPattern;
                    e.Pattern = _wrnPatterns[CurrentWrnPattern].StringPattern;
                }
                ElsUiPanel.SetUiPatternNumber(CurrentWrnPattern, ExtraEnum.WRNL.ToString());
            }
        }

        private void SetupPatternsPrm()
        {
            _prmPatterns = new Dictionary<int, Pattern>();
            int delay = 500;
            int count = 0;
            int patt = 0;
            for (int i = 0; i < 64; i++)
            {
                if (count == 4)
                {
                    count = 0;
                    delay = delay - 50;
                }
                if (delay < 200)
                {
                    delay = 500;
                    patt++;
                }
                _prmPatterns.Add(i, new Pattern(PatternType.PRML, delay, Leds.StringPatterns[patt], i, count));
                count++;
            }
            CurrentPrmPattern = 0;
        }

        private void SetupSecPatterns()
        {
            _secPatterns = new Dictionary<int, Pattern>();
            _secPatterns.Add(0, new Pattern(PatternType.SECL, 500, Leds.StringPatterns[22], 0));
            CurrentSecPattern = 0;
            foreach (Extra.Extra e in _extras.SECL.Values)
            {
                switch (e.Id)
                {
                    case 7:
                        e.Pattern = _secPatterns[CurrentSecPattern].StringPattern;
                        break;
                    case 8:
                        e.Pattern = PatternUtils.Reverse(_secPatterns[CurrentSecPattern].StringPattern);
                        break;
                    case 9:
                        e.Pattern = _secPatterns[CurrentSecPattern].StringPattern;
                        break;
                }
                e.Delay = _secPatterns[CurrentSecPattern].Delay;
            }
        }

        private void SetupWrnPatterns()
        {
            _wrnPatterns = new Dictionary<int, Pattern>();
            _wrnPatterns.Add(0, new Pattern(PatternType.SECL, 300, Leds.StringPatterns[2], 0));
            CurrentWrnPattern = 0;
            foreach (Extra.Extra e in _extras.WRNL.Values)
            {
                switch (e.Id)
                {
                    case 5:
                        e.Pattern = _secPatterns[CurrentWrnPattern].StringPattern;
                        break;
                    case 6:
                        e.Pattern = PatternUtils.Reverse(_secPatterns[CurrentWrnPattern].StringPattern);
                        break;
                }
                e.Delay = _wrnPatterns[CurrentWrnPattern].Delay;
            }
        }
    }
}
