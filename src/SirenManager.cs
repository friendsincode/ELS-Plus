using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Sirens;

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
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
            _sirens = new List<Siren>();
        }

        private void FileLoader_OnSettingsLoaded(configuration.SettingsType.Type type, string Data)
        {
            switch (type)
            {
                case configuration.SettingsType.Type.GLOBAL:
                    var u = SharpConfig.Configuration.LoadFromString(Data);
                    var t = u["GENERAL"]["MaxActiveVehs"].IntValue;
                    if (_sirens != null)
                    {
                        _sirens.Capacity = t;
                        Debug.WriteLine($"INI Value:{t}\n" +
                            $"varible capicity: {_sirens.Capacity}");
                    }
                    break;
                case configuration.SettingsType.Type.LIGHTING:
                    break;
            }

        }

        private void AddSiren(Vehicle vehicle)
        {
            _sirens.Add(new Siren(vehicle));
        }

        public void SetCurrentSiren(Vehicle vehicle)
        {
            if (!vehicleIsRegisteredLocaly(vehicle))
            {
                AddSiren(vehicle);
#if DEBUG
                Debug.WriteLine("added new siren");
#endif
                SetCurrentSiren(vehicle);
            }
            else
            {
                foreach (Siren siren in _sirens)
                {
                    if (siren._vehicle.Handle == vehicle.Handle)
                    {
#if DEBUG
                        Debug.WriteLine("added existing siren");
#endif
                        currentSiren = siren;
                    }
                }
            }


        }

        public void runtick()
        {
            currentSiren.ticker();
        }
        private bool vehicleIsRegisteredLocaly(Vehicle vehicle)
        {
            bool vehicleIsRegisteredLocaly = false;
            foreach (Siren siren in _sirens)
            {
                if (siren._vehicle == vehicle)
                {
                    vehicleIsRegisteredLocaly = true;
                }
            }
            return vehicleIsRegisteredLocaly;
        }
        public void UpdateSirens(int NetID)
        {
            var y = new PlayerList()[NetID];
           
            Vehicle vehicle = y.Character.CurrentVehicle;
            if (vehicleIsRegisteredLocaly(vehicle))
            {
                foreach (Siren siren in _sirens)
                {
                    if (siren._vehicle == vehicle)
                    {
                        siren.updateLocalRemoteSiren();
                    }
                }
            }
            else
            {
                AddSiren(vehicle);
            }
        } 
    }

}
