using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS
{
    class SirenManager
    {
        /// <summary>
        /// Siren that LocalPlayer can manage
        /// </summary>
        private Siren currentSiren;
        private List<Siren> _sirens;
        public SirenManager()
        {
            _sirens = new List<Siren>();
        }

        public void AddSiren(Vehicle vehicle)
        {
            _sirens.Add(new Siren(vehicle));
        }

        public void SetCurrentSiren(Vehicle vehicle)
        {
            if (!HasEls(vehicle))
            {
                AddSiren(vehicle);
                Debug.WriteLine("added new siren");
            }
            else
            {
                foreach (Siren siren in _sirens)
                {
                    if (siren._vehicle.Handle == vehicle.Handle)
                    {
                        Debug.WriteLine("added existing siren");
                        currentSiren = siren;
                    }
                }
            }
            

        }
        public bool HasEls(Vehicle vehicle)
        {
            var result = false;
            foreach (Siren siren in _sirens)
            {
                if (siren._vehicle.Handle == vehicle.Handle) result = true;
            }
            return result;
        }

        public void runtick()
        {
            currentSiren.ticker();
        }
        public void UpdateSirens(Vehicle vehicle)
        {
            Siren lsiren;
            foreach (Siren siren in _sirens)
            {
                if (siren._vehicle == vehicle)
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
