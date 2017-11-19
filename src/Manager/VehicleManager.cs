using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

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

                _currentVehicle = Entities.Find((o => o.Handle == (int)Game.PlayerPed.CurrentVehicle.Handle)) as ELSVehicle;
                Screen.ShowNotification("adding vehicle");
                AddIfNotPresint(Game.PlayerPed.CurrentVehicle);

                _currentVehicle?.RunTick();
            }
        }

        private new bool AddIfNotPresint(PoolObject o)
        {
            if (!Entities.Exists(poolObject => poolObject.Handle == o.Handle))
            {
                Entities.Add(new ELSVehicle(o.Handle));

                return false;
            }
            return true;
        }
        internal void UpdateSirens(string command, int netId, int playerId, bool state)
        {
#if !REMOTETEST
            if (Game.Player.ServerId == playerId) return;
#endif
            //var vehicle = new PlayerList()[netId].Character.CurrentVehicle;
            var vehicle = new Vehicle(Function.Call<int>(Hash.NETWORK_GET_ENTITY_FROM_NETWORK_ID, netId));
            if (!CitizenFX.Core.Native.Function.Call<bool>(Hash.DOES_ENTITY_EXIST, vehicle))
            {
                Screen.ShowNotification("Vehicle does not exist");
                return;
            };
            AddIfNotPresint(vehicle);
            ((ELSVehicle)Entities.Find(o => o.Handle == (int)vehicle.Handle)).UpdateSiren(command, state);


        }

        internal void SyncVehicle(string dataType, IDictionary<string, object> dataDic, int playerId)
        {
            var veh = new PlayerList()[playerId].Character.CurrentVehicle;
            ((ELSVehicle)Entities.Find(o => o.Handle == veh.Handle)).SyncData(dataType, dataDic);
        }

        void SyncAllVehicles()
        {

        }

        void GetAllVehicles()
        {

        }
    }
}