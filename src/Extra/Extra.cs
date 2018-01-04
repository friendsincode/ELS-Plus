using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.NUI;

namespace ELS.Extra
{

    internal class Extra
    {

        Vector3 _posistion;
        Entity _vehicle;
        int _Id;
        configuration.Extra _extraInfo;
        private bool _state;
        private bool _pattRunning;
        string _pattern;

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
            }

        }

        
        internal int Delay { get; set; }
        internal bool state
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
                return API.IsVehicleExtraTurnedOn(_vehicle.Handle, _Id);
            }
        }
        internal Extra(Entity entity, int id, configuration.Extra ex, bool state = false)
        {
            _state = state;
            _vehicle = entity;
            _Id = id;
            _extraInfo = ex;
            Delay = 50;
            if (!API.DoesExtraExist(entity.Handle, id))
            {
                CitizenFX.Core.Debug.WriteLine($"Extra id: {id} does not exsist");
            }
            SetFalse();
            if (_Id == 7 || _Id == 8 || _Id == 9)
            {
                Delay = 100;
            }
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"Registered extra_{_Id} successfully");    
#endif
        }
        internal void SetState(bool state)
        {
            if (_state == state) return;
            this.state = state;
        }
        private void SetTrue()
        {
            API.SetVehicleExtra(_vehicle.Handle, _Id, false);
        }
        private void SetFalse()
        {
            API.SetVehicleExtra(_vehicle.Handle, _Id, true);

        }

        internal async void RunPattern(string patt)
        {
            while (IsPatternRunning)
            {
                foreach (char c in patt.ToCharArray())
                {
                    if (!IsPatternRunning)
                    {
                        break;
                    }
                    if (c.Equals('0'))
                    {
                        ElsUiPanel.SendLightData(false, $"#extra{_Id}", "");
                        SetFalse();
                    }
                    else
                    {
                        ElsUiPanel.SendLightData(true, $"#extra{_Id}", _extraInfo.Color);
                        SetTrue();
                    }
                    await ELS.Delay(Delay);
                }
            }
            CleanUp();

        }

        internal void CleanUp()
        {
            SetFalse();
            ElsUiPanel.SendLightData(false, $"#extra{_Id}", "");
        }
    }
}
