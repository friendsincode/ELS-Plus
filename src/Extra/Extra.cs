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
        internal bool IsPatternRunning
        {
            get { return _pattRunning; }
            set
            {
                _pattRunning = value;
            }

        }
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
            if (!API.DoesExtraExist(entity.Handle, id))
            {
                CitizenFX.Core.Debug.WriteLine($"Extra id: {id} does not exsist");
            }
        }
        internal void SetState(bool state)
        {
            if (_state == state) return;
            this.state = state;
        }
        private void SetTrue()
        {
            if (!state)
            {
                API.SetVehicleExtra(_vehicle.Handle, _Id, true);
            }
            else
            {
                API.SetVehicleExtra(_vehicle.Handle, _Id, true);
            }
        }
        private void SetFalse()
        {
            if (state)
            {
                API.SetVehicleExtra(_vehicle.Handle, _Id, false);
            }
            else
            {
                API.SetVehicleExtra(_vehicle.Handle, _Id, false);
            }
        }

        internal async void RunPattern(string patt, int delay)
        {
            if (!IsPatternRunning)
            {
                return;
            }
            foreach (char c in patt.ToCharArray())
            {
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
                await ELS.Delay(delay);
            }
        }

        internal void CleanUp()
        {
            SetFalse();
            ElsUiPanel.SendLightData(false, $"#extra{_Id}", "");
        }
    }
}
