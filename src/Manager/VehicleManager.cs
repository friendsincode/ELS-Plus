using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core;

namespace ELS.Manager
{
    class VehicleManager : Manager
    {
        private ELSVehicle _currentVehicle;
        public VehicleManager() : base()
        {
        }

        internal override void RunTick()
        {
            if (Game.PlayerPed.IsInVehicle()
                && Game.PlayerPed.IsSittingInVehicle()
                && Game.PlayerPed.CurrentVehicle.IsEls()
                && (
                    Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
                    || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed
                )
            )
            {
                this.AddIfNotPresint(Game.PlayerPed.CurrentVehicle);
                _currentVehicle = Entities.Find((o => o.Handle == (int) Game.PlayerPed.CurrentVehicle.Handle)) as ELSVehicle;
                _currentVehicle?.RunTick();
            }
        }

        private new bool  AddIfNotPresint(PoolObject o)
        {
            if (!Entities.Exists(poolObject => poolObject.Handle == o.Handle))
            {
                Entities.Add(new ELSVehicle(o.Handle));

                return false;
            }
            return true;
        }
        internal void UpdateSirens(string command, int netId, bool state)
        {
            if (Game.Player.ServerId == netId) return;
            var vehicle = new PlayerList()[netId].Character.CurrentVehicle;
            if (!vehicle.Exists()) throw new Exception("Vehicle does not exist");
            AddIfNotPresint(vehicle);
           ((ELSVehicle) Entities.Find(o => o.Handle == (int) vehicle.Handle)).SendSirenCommand(command,state);
        }

        internal void SyncVehicle(string dataType, IDictionary<string, object> dataDic, int playerId)
        {
            var veh = new PlayerList()[playerId].Character.CurrentVehicle;
            ((ELSVehicle) Entities.Find(o => o.Handle == veh.Handle)).SyncData(dataType, dataDic);
        }

        void SyncAllVehicles()
        {
            
        }

        void GetAllVehicles()
        {
            
        }
    }
}