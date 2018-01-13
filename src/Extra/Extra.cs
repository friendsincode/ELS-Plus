using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.NUI;
using ELS.Light.Patterns;

namespace ELS.Extra
{

    internal enum LightType
    {
        PRML,
        SECL,
        WRNL,
        SCL,
        SBRN,
        TDL
    }

    internal partial class Extra
    {

        Vector3 _posistion;
        Entity _vehicle;
        int _Id;
        configuration.Extra _extraInfo;
        private bool _state;
        private bool _pattRunning;
        private int _pattnum;
        private LightType LightType { get; set; }
        private string _pattern;
        private string _pattType;

        internal string Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
            }
        }

        internal string PatternType
        {
            get { return _pattType;  }
            set
            {
                _pattType = value;
            }
        }

        internal int PatternNum
        {
            get { return _pattnum; }
            set
            {
                _pattnum = value;
                Pattern = Leds.StringPatterns[PatternNum];
                //ElsUiPanel.SetUiPatternNumber(PatternNum, LightType.ToString());
            }
        }

        internal int Id
        {
            get
            {
                return _Id;
            }
        }
        internal bool IsPatternRunning
        {
            get { return _pattRunning; }
            set
            {
                _pattRunning = value;
                if (!IsPatternRunning)
                {
                    CleanUp();
                }
            }

        }

        internal Vector3 GetBone()
        {
            if (_Id == 10)
            {
                return ((Vehicle)_vehicle).Bones[$"extra_ten"].Position;
            }
            return ((Vehicle)_vehicle).Bones[$"extra_{_Id}"].Position;
        }

        internal int Delay { get; set; }
        internal bool State
        {
            private set
            {
                _state = value;
                if (value)
                {
                    SetTrue();
                }
                else
                {
                    SetFalse();
                }
            }
            get
            {
                return _state;
            }
        }

        internal Extra(Entity entity, int id, configuration.Extra ex, string format = "", bool state = false)
        {
            _vehicle = entity;
            _Id = id;
            _extraInfo = ex;
            CleanUp();
            SetInfo();
            PatternType = format;
            if (!API.DoesExtraExist(entity.Handle, id))
            {
                CitizenFX.Core.Debug.WriteLine($"Extra id: {id} does not exsist");
            }
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"Registered extra_{_Id} successfully");
#endif
        }

        internal void SetState(bool state)
        {
            this.State = state;
        }

        private void SetTrue()
        {
            API.SetVehicleExtra(_vehicle.Handle, _Id, false);
            ElsUiPanel.SendLightData(true, $"#extra{_Id}", _extraInfo.Color);
        }

        private void SetFalse()
        {
            API.SetVehicleExtra(_vehicle.Handle, _Id, true);
            ElsUiPanel.SendLightData(false, $"#extra{_Id}", "");
        }

        int count = 0;
        int flashrate;
        int allowflash = 1;  
        internal async void ExtraTicker()
        {
            flashrate = (int)Game.FPS / ((int)Game.FPS * 60 / Delay);
#if DEBUG
            //CitizenFX.Core.Debug.WriteLine($"Flash rate: {flashrate}");
            //CitizenFX.Core.Debug.WriteLine($"Allow Flash: {allowflash}");
#endif
            if (Game.FPS < 25 || flashrate == allowflash)
            {
                allowflash = 1;
                if (IsPatternRunning)
                {
                    if (!IsPatternRunning)
                    {
                        CleanUp();
                        return;
                    }
                    if (Pattern[count].Equals('0'))
                    {
                        SetState(false);
                        if (!IsPatternRunning)
                        {
                            CleanUp();
                            return;
                        }
                    }
                    else
                    {
                        SetState(true);
                        if (!IsPatternRunning)
                        {
                            CleanUp();
                            return;
                        }
                        //DrawLight();
                    }
                    count++;
                    if (count > Pattern.Length - 1)
                    {
                        count = 0;
                    }
                    if (!IsPatternRunning)
                    {
                        CleanUp();
                        return;
                    }
                }
            }
            else if (allowflash > flashrate)
            {
                allowflash = 1;
            }
            else
            {
                allowflash++;
            }
        }

        internal async void RunPattern()
        {
            ElsUiPanel.SetUiPatternNumber(PatternNum, LightType.ToString());
            while (IsPatternRunning)
            {
                foreach (char c in Pattern.ToCharArray())
                {
                    if (!IsPatternRunning)
                    {
                        CleanUp();
                        break;
                    }
                    if (c.Equals('0'))
                    { 
                        SetFalse();
                    }
                    else
                    {
                        DrawLight();
                        SetTrue();
                    }
                    await ELS.Delay(Delay);
                }
            }
            CleanUp();
        }

        private Vector3 dirVector;
        private float anglehorizontal = 0f;
        private float anngleVirtical = 0f;

        internal void DrawLight()
        {
            var off = _vehicle.GetPositionOffset(GetBone());
            var extraoffset = _vehicle.GetOffsetPosition(off + new Vector3(0, 0.05f, 0));

            
            float hx = (float)((double)extraoffset.X + 5 * Math.Cos(((double)anglehorizontal + Game.Player.Character.CurrentVehicle.Rotation.Z) * Math.PI / 180.0));
            float hy = (float)((double)extraoffset.Y + 5 * Math.Sin(((double)anglehorizontal + Game.Player.Character.CurrentVehicle.Rotation.Z) * Math.PI / 180.0));
            float vz = (float)((double)extraoffset.Z + 5 * Math.Sin((double)anngleVirtical * Math.PI / 180.0));

            Vector3 destinationCoords = (new Vector3(hx,
           hy, vz));

            dirVector = destinationCoords - extraoffset;
            dirVector.Normalize();
            API.DrawSpotLightWithShadow(extraoffset.X, extraoffset.Y, extraoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, Color['r'], Color['g'], Color['b'], 100.0f, 1f, 0.0f, 13.0f, 1f, 100f);
            API.DrawLightWithRangeAndShadow(GetBone().X + float.Parse(_extraInfo.OffsetX), GetBone().Y + float.Parse(_extraInfo.OffsetY), GetBone().Z + float.Parse(_extraInfo.OffsetZ), Color['r'], Color['g'], Color['b'], 5f, 1f, .5f);
        }

        internal Dictionary<char, int> Color;        

        internal void SetInfo()
        {
            switch (_Id)
            {
                case 1:
                    LightType = LightType.PRML;
                    Delay = 400;
                    PatternNum = 0;
                    break;
                case 2:
                    LightType = LightType.PRML;
                    Delay = 400;
                    PatternNum = 0;
                    break;
                case 3:
                    LightType = LightType.PRML;
                    Delay = 400;
                    PatternNum = 0;
                    break;
                case 4:
                    LightType = LightType.PRML;
                    Delay = 400;
                    PatternNum = 0;
                    break;
                case 5:
                    LightType = LightType.WRNL;
                    Delay = 400;
                    PatternNum = 0;
                    break;
                case 6:
                    LightType = LightType.WRNL;
                    Delay = 400;
                    PatternNum = 0;
                    break;
                case 7:
                    LightType = LightType.SECL;
                    PatternNum = 0;
                    Delay = 200;
                    break;
                case 8:
                    LightType = LightType.SECL;
                    PatternNum = 0;
                    Delay = 200;
                    break;
                case 9:
                    LightType = LightType.SECL;
                    PatternNum = 0;
                    Delay = 200;
                    break;
                case 10:
                    LightType = LightType.SBRN;
                    break;
                case 11:
                    LightType = LightType.SCL;
                    break;
                case 12:
                    LightType = LightType.TDL;
                    break;
            }


            string hex = "0xFFFFFF";
            switch (_extraInfo.Color)
            {
                case "red":
                    hex = "0xFF0300";
                    break;
                case "blue":
                    hex = "0x000AFF";
                    break;
                case "amber":
                    hex = "0xFF7E00";
                    break;
                case "white":
                    break;
            }
            int r = Convert.ToInt32(hex.Substring(2, 2), 16);
            int g = Convert.ToInt32(hex.Substring(4, 2), 16);
            int b = Convert.ToInt32(hex.Substring(6, 2), 16);
            Color =  new Dictionary<char, int>
            {
                { 'r', r },
                { 'g', g },
                { 'b', b },
            };
        }

        internal void CleanUp()
        {
            SetState(false);
        }
    }
}
