/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
        Siren.Siren currentSiren;
        List<Siren.Siren> _sirens;
        public SirenManager()
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
            _sirens = new List<Siren.Siren>();
        }
        private void RunGC()
        {
            _sirens.RemoveAll(obj => !obj._vehicle.Exists());
        }
        internal void FullSync()
        {
            RunGC();
            _sirens.ForEach((siren) =>
            {
                siren.FullSync();
            });
        }
        void FileLoader_OnSettingsLoaded(SettingsType.Type type, string Data)
        {
            switch (type)
            {
                case SettingsType.Type.GLOBAL:
                    var u = SharpConfig.Configuration.LoadFromString(Data);
                    var t = u["GENERAL"]["MaxActiveVehs"].IntValue;
                    if (_sirens != null)
                    {
                        _sirens.Capacity = t;
                    }
                    break;
                case SettingsType.Type.LIGHTING:
                    break;
            }

        }

        internal void CleanUp()
        {
#if DEBUG
            Debug.WriteLine("Running CleanUp()");
#endif
            _sirens.ForEach(siren => siren.CleanUP());
        }

        void AddSiren(Vehicle vehicle)
        {
            if (ELS.isStopped) return;

            _sirens.Add(new Siren.Siren(vehicle));
        }


        void SetCurrentSiren(Vehicle vehicle)
        {
            if (!vehicleIsRegisteredLocaly(vehicle))
            {
                AddSiren(vehicle);
#if DEBUG
                Debug.WriteLine("added new siren");
#endif
            }
            else
            {
#if DEBUG
                Debug.WriteLine("added existing siren");
#endif
            }
            currentSiren = _sirens.Find(siren => siren._vehicle.Handle == vehicle.Handle);

        }

        internal void Runtick()
        {
            var LocalPlayer = Game.Player;
            if (LocalPlayer.Character.IsInVehicle()
                && LocalPlayer.Character.IsSittingInVehicle()
                && VCF.isELSVechicle(LocalPlayer.Character.CurrentVehicle.DisplayName)
                && (
                    LocalPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == LocalPlayer.Character
                    || LocalPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == LocalPlayer.Character
                    )
                )
            {
                if (((currentSiren == null) || currentSiren._vehicle != Game.Player.Character.CurrentVehicle))
                {
                    SetCurrentSiren(Game.Player.Character.CurrentVehicle);
                }
                currentSiren.ticker();
            }
        }

        bool vehicleIsRegisteredLocaly(Vehicle vehicle)
        {
            return _sirens.Exists(siren => siren._vehicle.Handle == vehicle.Handle);
        }

        internal void UpdateSirens(string command, int NetID, bool state)
        {
            if (Game.Player.ServerId == NetID) return;
#if DEBUG
            Debug.WriteLine($"netId:{NetID.ToString()} localId {Game.Player.ServerId.ToString()}");
#endif
            if (ELS.isStopped) return;
            var y = new PlayerList()[NetID];
            Vehicle vehicle = y.Character.CurrentVehicle;
            if (vehicle.Exists()) throw new Exception("Vehicle does not exist");
            if (!vehicleIsRegisteredLocaly(vehicle))
            {
                AddSiren(vehicle);
            }
            _sirens.Find(siren => siren._vehicle.Handle == vehicle.Handle).updateLocalRemoteSiren(command, state);
        }

    }
}
