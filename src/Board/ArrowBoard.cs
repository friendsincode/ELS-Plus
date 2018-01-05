using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Board
{
    internal class ArrowBoard
    {
        Entity _vehicle;
        configuration.MISC _misc;
        string _boardType;
        private bool _hasBoard;
        int _speed = 2;
        public bool HasBoard
        {
            get
            {
                return _hasBoard;
            }
            private set
            {
                _hasBoard = value;
            }
        }

        internal bool AnimateBoard
        {
            get; set;
        }

        internal int BoardDoorIndex
        {
            get; set;
        }

        internal bool BoardRaised
        {
            get; set;
        }

        internal ArrowBoard(Entity entity, configuration.MISC misc)
        {
            _vehicle = entity;
            _misc = misc;
            _boardType = _misc.ArrowboardType.ToLower();
            AnimateBoard = false;
            BoardRaised = false;
            switch (_boardType)
            {
                case "bonnet":
                    BoardDoorIndex = 4;
                    HasBoard = true;
                    break;
                case "boot":
                    BoardDoorIndex = 5;
                    HasBoard = true;
                    break;
                case "boot2":
                    BoardDoorIndex = 6;
                    HasBoard = true;
                    break;
                case "boots":
                    BoardDoorIndex = 5;
                    HasBoard = true;
                    break;
                case "off":
                    BoardDoorIndex = -1;
                    HasBoard = false;
                    break;
                default:
                    HasBoard = false;
                    break;
            }
            CitizenFX.Core.Debug.WriteLine($"Added ArrowBoard of {_boardType}");
        }

        

        internal void RaiseBoard()
        {
            if (BoardRaised)
            {
                return;
            }
            var _angle = API.GetVehicleDoorAngleRatio(_vehicle.Handle, BoardDoorIndex);
            if (!API.IsVehicleDoorFullyOpen(_vehicle.Handle,BoardDoorIndex))
            {
                API.SetVehicleDoorControl(_vehicle.Handle, BoardDoorIndex, _speed, _angle);
                if (_boardType.Equals("boots"))
                {
                    API.SetVehicleDoorControl(_vehicle.Handle, 6, _speed, _angle);
                }
            }
            if (API.IsVehicleDoorFullyOpen(_vehicle.Handle, BoardDoorIndex))
            {
                BoardRaised = true;
            }
            
        }

        internal void LowerBoard()
        {
            BoardRaised = false;
        }
    }
}
