using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS.Extra
{
    internal class Extra
    {

        Vector3 _posistion;
        Entity _vehicle;
        int _Id;
        private bool _state;
        internal bool state { private set {
                _state=value;
                if (value)
                {
                    SetTrue();
                }
                else
                {
                    SetFalse();
                }
            } get {
                return API.IsVehicleExtraTurnedOn(_vehicle.Handle, _Id);
            } }
        internal Extra(Entity entity,int id, bool state = false)
        {
            _state = state;
            _vehicle = entity;
            _Id = id;
            if (!API.DoesExtraExist(id, entity.Handle))
            {
                CitizenFX.Core.Debug.WriteLine($"Extra id: {id} does not exsist");
            }
        }
        internal void SetState(bool state)
        {
            if (_state == state) return;
            this.state = state;
        }
        private void SetTrue() {
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
                API.SetVehicleExtra(_vehicle.Handle, _Id, true);
            }
            else
            {
                API.SetVehicleExtra(_vehicle.Handle, _Id, true);
            }
        }
        internal void CleanUp() {

        }
    }
}
