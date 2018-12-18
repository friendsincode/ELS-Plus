
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.FullSync;
using ELS.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Gadgets
{
    internal class Ladder : IFullSyncComponent
    {
        ILight lights;
        string _boardType;
        private bool _hasLadder;
        private bool _raise;
        int _speed = 2;

        public bool HasLadder
        {
            get
            {
                return _hasLadder;
            }
            private set
            {
                _hasLadder = value;
            }
        }

        internal bool RaiseBoardNow
        {
            get
            {
                return _raise;
            }
            set
            {
                _raise = value;
            }
        }

        internal int LadderVerticalIndex
        {
            get; set;
        }

        internal int LadderHorizontalIndex
        {
            get; set;
        }
        
        internal float VerticalAngle { get; set; }

        internal float HorizontalAngle { get; set; }

        internal bool LadderRaised
        {
            get; set;
        }

        internal bool LadderRotated
        {
            get; set;
        }

        public Dictionary<string, object> GetData()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("raised", RaiseBoardNow);
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
           
        }

        internal Ladder(ILight light, configuration.MISC misc)
        {
            lights = light;
            
            RaiseBoardNow = false;
            LadderRaised = false;
            switch (misc.LadderControl.HorizontalControl)
            {
                case "bonnet":
                    LadderHorizontalIndex = 4;
                    break;
                case "boot":
                    LadderHorizontalIndex = 5;
                    break;
            }
            switch (misc.LadderControl.VerticalControl)
            {
                case "bonnet":
                    LadderVerticalIndex = 4;
                    break;
                case "boot":
                    LadderVerticalIndex = 5;
                    break;
            }
            Utils.DebugWriteLine($"Added Ladder");
        }

        internal void LadderTicker()
        {
            RaiseLowerLadder();
            RotateLadder();
        }
        

        internal void RaiseLowerLadder()
        {
            VerticalAngle = API.GetVehicleDoorAngleRatio(lights._vehicle.Handle, LadderVerticalIndex);
            if (Game.IsControlPressed(0, Control.PhoneUp) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetNetworkId() == lights._vehicle.GetNetworkId())
            {
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveLadderUp, lights._vehicle, true, Game.Player.ServerId);
                VerticalAngle = VerticalAngle + 0.029999999f;

            }
            if (Game.IsControlPressed(0, Control.PhoneDown) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetNetworkId() == lights._vehicle.GetNetworkId())
            {

                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveLadderDown, lights._vehicle, true, Game.Player.ServerId);
                VerticalAngle = VerticalAngle - 0.029999999f;
            }
            if (API.IsVehicleDoorFullyOpen(lights._vehicle.Handle, LadderVerticalIndex) || VerticalAngle <= 0.80000001f)
            {
                API.SetVehicleDoorControl(lights._vehicle.Handle, LadderVerticalIndex, _speed, VerticalAngle);
            }
            if (VerticalAngle > .25)
            {
                LadderRaised = true;
            }
        }

        internal void RotateLadder()
        {
            VerticalAngle = API.GetVehicleDoorAngleRatio(lights._vehicle.Handle, LadderVerticalIndex);
            if (Game.IsControlPressed(0, Control.PhoneUp) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetNetworkId() == lights._vehicle.GetNetworkId())
            {
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveLadderLeft, lights._vehicle, true, Game.Player.ServerId);
                HorizontalAngle = HorizontalAngle + 0.029999999f;

            }
            if (Game.IsControlPressed(0, Control.PhoneDown) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetNetworkId() == lights._vehicle.GetNetworkId())
            {

                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveLadderRight, lights._vehicle, true, Game.Player.ServerId);
                HorizontalAngle = HorizontalAngle - 0.019999999f;
            }
            if (API.IsVehicleDoorFullyOpen(lights._vehicle.Handle, LadderHorizontalIndex) || VerticalAngle <= 0.80000001f)
            {
                API.SetVehicleDoorControl(lights._vehicle.Handle, LadderHorizontalIndex, _speed, VerticalAngle);
            }
            if (HorizontalAngle > .25 || HorizontalAngle < -.25)
            {
                LadderRotated = true;
            }
        }
    }
}
