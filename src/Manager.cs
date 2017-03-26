using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using ELS.Siren;

namespace ELS
{
    class Manager
    {
        private IManagerEntry currentEntry;
        private List<IManagerEntry> _list;
        public Manager(IManagerEntry type)
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
            _list = new List<IManagerEntry>();
        }

        private void FileLoader_OnSettingsLoaded(configuration.SettingsType.Type type, string Data)
        {
            switch (type)
            {
                case configuration.SettingsType.Type.GLOBAL:
                    var u = SharpConfig.Configuration.LoadFromString(Data);
                    var t = u["GENERAL"]["MaxActiveVehs"].IntValue;
                    if (_list != null)
                    {
                        _list.Capacity = t;
                        
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
            foreach (var entry in _list)
            {
                entry.CleanUP();
            }
        }

        private void AddEntry(Vehicle vehicle)
        {
            if (ELS.isStopped) return;
            _list.Add(new Siren.Siren(vehicle));
        }


        public void SetEntry(Vehicle vehicle)
        {
            if (!VehicleIsRegisteredLocaly(vehicle))
            {
                AddEntry(vehicle);
#if DEBUG
                Debug.WriteLine("added new siren");
#endif
                SetEntry(vehicle);
            }
            else
            {
                foreach (var entry in _list)
                {
                    if (entry._vehicle.Handle == vehicle.Handle)
                    {
#if DEBUG
                        Debug.WriteLine("added existing siren");
#endif
                        currentEntry = entry;
                    }
                }
            }


        }

        public void Runtick()
        {
            if (((currentEntry == null) || currentEntry._vehicle != new PlayerList()[Game.Player.ServerId].Character.CurrentVehicle))
            {
                SetEntry(new PlayerList()[Game.Player.ServerId].Character.CurrentVehicle);
            }

            currentEntry.ticker();
        }

        private bool VehicleIsRegisteredLocaly(Vehicle vehicle)
        {
            bool vehicleIsRegisteredLocaly = false;
            foreach (var siren in _list)
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
            //Debug.WriteLine($"netId:{NetID.ToString()} localId {ELS.localplayerid.ToString()}");
            if (Game.Player.ServerId == NetID)
            {
                return;
            }
            if (ELS.isStopped) return;
            var y = new PlayerList()[NetID];
            if (!y.Character.IsInVehicle() || !y.Character.IsSittingInVehicle()) return;
            Vehicle vehicle = y.Character.CurrentVehicle;
            if (VehicleIsRegisteredLocaly(vehicle))
            {
                foreach (var siren in _list)
                {
                    if (siren._vehicle == vehicle)
                    {
                        siren.updateLocalRemoteSiren(sirenString, state);
                    }
                }
            }
            else
            {
                AddEntry(vehicle);
                foreach (var siren in _list)
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
