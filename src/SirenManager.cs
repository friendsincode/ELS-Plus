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
using CitizenFX.Core;
using ELS.configuration;

namespace ELS
{
    class SirenManager
    {
        /// <summary>
        /// Siren that LocalPlayer can manage
        /// </summary>
        Siren.Siren _currentSiren;

        private readonly List<Siren.Siren> _sirens;
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
            _currentSiren.FullSync();
            //_sirens.ForEach((siren) =>
            //{
            //    siren.FullSync();
            //});
        }
        internal void FullSync(string dataType, IDictionary<string, object> dataDic, int playerId)
        {
            // RunGC();
            var randVehicle = new PlayerList()[playerId].Character.CurrentVehicle;
            Debug.WriteLine("FullSyncRecieved");
            _sirens.Find(siren => siren._vehicle.Handle == randVehicle.Handle).SetFullSync(dataType, dataDic);
        }
        void FileLoader_OnSettingsLoaded(SettingsType.Type type, string data)
        {
            switch (type)
            {
                case SettingsType.Type.GLOBAL:
                    var u = SharpConfig.Configuration.LoadFromString(data);
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
            if (ELS.IsStopped) return;

            _sirens.Add(new Siren.Siren(vehicle));
        }


        void SetCurrentSiren(Vehicle vehicle)
        {
            AddVehicleIfNotRegistered(vehicle);
            _currentSiren = _sirens.Find(siren => siren._vehicle.Handle == vehicle.Handle);
        }

        internal void Runtick()
        {
            var localPlayer = Game.Player;
            if (localPlayer.Character.IsInVehicle()
                && localPlayer.Character.IsSittingInVehicle()
                && localPlayer.Character.CurrentVehicle.IsEls()
                && (
                    localPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == localPlayer.Character
                    || localPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == localPlayer.Character
                    )
                )
            {
                if (((_currentSiren == null) || _currentSiren._vehicle != Game.Player.Character.CurrentVehicle))
                {
                    SetCurrentSiren(Game.Player.Character.CurrentVehicle);
                }
                _currentSiren.ticker();
            }
        }

        bool VehicleIsRegisteredLocaly(Vehicle vehicle)
        {
            return _sirens.Exists(siren => siren._vehicle.Handle == vehicle.Handle);
        }

        internal void UpdateSirens(string command, int netId, bool state)
        {
            if (Game.Player.ServerId == netId) return;

#if DEBUG
            Debug.WriteLine($"netId:{netId} localId:{Game.Player.ServerId}");
#endif
            if (ELS.IsStopped) return;
            Vehicle vehicle = new PlayerList()[netId].Character.CurrentVehicle;
            if (!vehicle.Exists()) throw new Exception("Vehicle does not exist");
            AddVehicleIfNotRegistered(vehicle);
            _sirens.Find(siren => siren._vehicle.Handle == vehicle.Handle).updateLocalRemoteSiren(command, state);
        }

        private void AddVehicleIfNotRegistered(Vehicle vehicle)
        {
            if (!VehicleIsRegisteredLocaly(vehicle))
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
        }
    }
}
