using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;

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
            foreach (ELSVehicle veh in Entities)
            {
                veh.RunExternalTick();
            }
#if DEBUG
            if (Game.Player.Character.LastVehicle == null) return;
            var bonePos = Game.Player.Character.LastVehicle.Bones["door_dside_f"].Position;
            var pos = Game.Player.Character.GetPositionOffset(bonePos);
            var text = new Text($"X:{pos.X} Y:{pos.Y} Z:{pos.Z} Lenght:{pos.Length()}", new PointF(Screen.Width / 2.0f, 10f), 0.5f);
            text.Alignment = Alignment.Center;
            if (pos.Length() < 1.5) text.Color = Color.FromArgb(255, 0, 0);
            text.Draw();
#endif

            //TODO Chnage how I check for the panic alarm
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
        internal void UpdateRemoteSirens(string command, int netId, int playerId, bool state)
        {
#if !REMOTETEST
            if (Game.Player.ServerId == playerId) return;
#endif
            var vehicle = new Vehicle(Function.Call<int>(Hash.NETWORK_GET_ENTITY_FROM_NETWORK_ID, netId));
            if (!CitizenFX.Core.Native.Function.Call<bool>(Hash.DOES_ENTITY_EXIST, vehicle))
            {
                Screen.ShowNotification("Vehicle does not exist");
                Screen.ShowNotification($"Net Status {Function.Call<bool>(Hash.NETWORK_GET_ENTITY_IS_NETWORKED,vehicle)}");
                return;
            };
            AddIfNotPresint(vehicle);
            ((ELSVehicle)Entities.Find(o => o.Handle == (int)vehicle.Handle)).UpdateRemoteSiren(command, state);


        }

        internal void SetSyncVehicle(string dataType, IDictionary<string, object> dataDic, int playerId)
        {
            var veh = new PlayerList()[playerId].Character.CurrentVehicle;
            ((ELSVehicle)Entities.Find(o => o.Handle == veh.Handle)).SetSyncData(dataType, dataDic);
        }

        void SyncAllVehicles()
        {

        }

        void GetAllVehicles()
        {

        }
        internal void CleanUP()
        {
            foreach(var obj in Entities)
            {
                ((ELSVehicle) obj).CleanUP();
            }
        }
        ~VehicleManager()
        {
            CleanUP();
        }
    }
}