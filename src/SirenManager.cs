using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS
{
    class SirenManager
    {
        private Siren currentSiren;
        private List<Siren> _sirens;
        public SirenManager()
        {
            _sirens= new List<Siren>();
        }

        public void AddSiren(Vehicle vehicle)
        {
            foreach (var siren in _sirens)
            {
                if (siren._vehicle == vehicle)
                {
                    return;
                }
            }
            _sirens.Add(new Siren(vehicle));
        }

        public void SetCurrentSiren(Vehicle vehicle)
        {
            foreach (Siren siren in _sirens)
            {
                if (siren._vehicle == vehicle)
                {
                    currentSiren = siren;
                }
            }
        }
        public bool HasEls(Vehicle vehicle)
        {
            var result = false;
            foreach (Siren siren in _sirens)
            {
                if (siren._vehicle == vehicle) result = true;
            }
            return result;
        }

        public void runtick()
        {
            
        }
        public void UpdateSirens(Vehicle vehicle)
        {
            Siren lsiren;
            foreach (Siren siren in _sirens)
            {
                if (siren._vehicle==vehicle)
                {
                    lsiren = siren;
                    break;
                }
            }
        }
        public Entity Nettoint(int netid)
        {
            return Function.Call<Entity>(Hash.NETWORK_GET_ENTITY_FROM_NETWORK_ID, netid);
        }
    }
}
