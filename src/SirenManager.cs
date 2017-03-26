using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.Siren;

namespace ELS
{
    class SirenManager
    {
        /// <summary>
        /// Siren that LocalPlayer can manage
        /// </summary>
        private Siren.Siren currentSiren;
        private List<Siren.Siren> _sirens;
        public SirenManager()
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
            _sirens = new List<Siren.Siren>();
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
                    }
                    break;
                case configuration.SettingsType.Type.LIGHTING:
                    break;
            }

        }

        internal void CleanUp()
        {
#if DEBUG
            Debug.WriteLine("Running CleanUp()");
#endif
            foreach (var siren in _sirens)
            {
                siren.CleanUP();
            }
        }

        private void AddSiren(Vehicle vehicle)
        {
            if (ELS.isStopped) return;
#if DEBUG
            foreach (var VARIABLE in vehicle.Bones)
            {
                Debug.WriteLine("Bones\n" + VARIABLE.ToString());
            }
#endif
            
            _sirens.Add(new Siren.Siren(vehicle));
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
                foreach (Siren.Siren siren in _sirens)
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

        public void Runtick()
        {
            if ((( currentSiren == null) || currentSiren._vehicle !=  new PlayerList()[Game.Player.ServerId].Character.CurrentVehicle))
            {
                SetCurrentSiren( new PlayerList()[Game.Player.ServerId].Character.CurrentVehicle);
            }

            currentSiren.ticker();
        }

        private bool vehicleIsRegisteredLocaly(Vehicle vehicle)
        {
            bool vehicleIsRegisteredLocaly = false;
            foreach (Siren.Siren siren in _sirens)
            {
                if (siren._vehicle.Handle == vehicle.Handle)
                {
                    vehicleIsRegisteredLocaly = true;
                }
            }
            return vehicleIsRegisteredLocaly;
        }

        public void UpdateSirens(int NetID, string sirenString, bool state)
        {
            if (Game.Player.ServerId == NetID)
            {
                return;
            }
#if DEBUG
            Debug.WriteLine($"netId:{NetID.ToString()} localId {Game.Player.ServerId.ToString()}");
#endif
            if (ELS.isStopped) return;
            var y = new PlayerList()[NetID];
            if (!y.Character.IsInVehicle() || !y.Character.IsSittingInVehicle()) return;
            Vehicle vehicle = y.Character.CurrentVehicle;
            if (vehicleIsRegisteredLocaly(vehicle))
            {
                foreach (Siren.Siren siren in _sirens)
                {
                    if (siren._vehicle == vehicle)
                    {
                        siren.updateLocalRemoteSiren(sirenString, state);
                    }
                }
            }
            else
            {
                AddSiren(vehicle);
                foreach (Siren.Siren siren in _sirens)
                {
                    if (siren._vehicle == vehicle)
                    {
                        siren.updateLocalRemoteSiren(sirenString, state);
                    }
                }
            }
        }
    }

}
