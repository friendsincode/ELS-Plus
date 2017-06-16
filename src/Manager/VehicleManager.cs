using System;
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
                AddIfNotPresint(new ELSVehicle(Game.PlayerPed.CurrentVehicle.Handle));
                _currentVehicle = Entities.Find((o => o.Handle == (int) Game.PlayerPed.CurrentVehicle.Handle)) as ELSVehicle;
                _currentVehicle?.RunTick();
            }
        }

        public void UpdateSirens(string command, int netId, bool state)
        {
            if (Game.Player.ServerId == netId) return;
            var vehicle = new PlayerList()[netId].Character.CurrentVehicle;
            if (!vehicle.Exists()) throw new Exception("Vehicle does not exist");
            AddIfNotPresint(new ELSVehicle(vehicle.Handle));
           ((ELSVehicle) Entities.Find(o => o.Handle == (int) vehicle.Handle)).SendSirenCommand(command,state);
        }
    }
}