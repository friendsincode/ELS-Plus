﻿
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.FullSync;
using ELS.Light;

namespace ELS.Gadgets
{
    public struct LadderFSData
    {
        public float Vertical;
        public float Horizontal;
        public bool Raised;
    }

    internal class Ladder : IFullSyncComponent<LadderFSData>
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

        public LadderFSData GetData()
        {
            
            return new LadderFSData() { Horizontal = HorizontalAngle, Raised = RaiseBoardNow, Vertical = VerticalAngle };
        }

        public void SetData(LadderFSData data)
        {
            RaiseBoardNow = data.Raised;
            HorizontalAngle = data.Horizontal;
            VerticalAngle = data.Vertical;
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
            int currId = Game.PlayerPed.CurrentVehicle.GetElsId();
            int vehId = lights._vehicle.GetElsId();
            if (Game.PlayerPed.IsInPoliceVehicle && currId == vehId)
                if (Game.IsControlPressed(0, Control.PhoneUp) && Game.PlayerPed.IsSittingInELSVehicle() && currId == vehId)
                {
                    RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveLadderUp, lights._vehicle, true, Game.Player.ServerId);
                    VerticalAngle = VerticalAngle + 0.029999999f;

                }
            if (Game.IsControlPressed(0, Control.PhoneDown) && Game.PlayerPed.IsSittingInELSVehicle() && currId == vehId)
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
            int currId = Game.PlayerPed.CurrentVehicle.GetElsId();
            int vehId = lights._vehicle.GetElsId();
            if (Game.IsControlPressed(0, Control.PhoneUp) && Game.PlayerPed.IsSittingInELSVehicle() && currId == vehId)
            {
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveLadderLeft, lights._vehicle, true, Game.Player.ServerId);
                HorizontalAngle = HorizontalAngle + 0.029999999f;

            }
            if (Game.IsControlPressed(0, Control.PhoneDown) && Game.PlayerPed.IsSittingInELSVehicle() && currId == vehId)
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
