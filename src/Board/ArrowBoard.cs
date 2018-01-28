
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.FullSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Board
{
    internal class ArrowBoard : IFullSyncComponent
    {
        Entity _vehicle;
        configuration.MISC _misc;
        string _boardType;
        private bool _hasBoard;
        private bool _raise;
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

        internal int BoardDoorIndex
        {
            get; set;
        }

        internal bool BoardRaised
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
            BoardRaised = (bool.Parse(data["raised"].ToString()));
        }

        internal ArrowBoard(Entity entity, configuration.MISC misc)
        {
            _vehicle = entity;
            _misc = misc;
            _boardType = _misc.ArrowboardType.ToLower();
            RaiseBoardNow = false;
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

        internal void BoardTicker()
        {
#if DEBUG
            //CitizenFX.Core.Debug.WriteLine($"Raise board now: {RaiseBoardNow}");
#endif
            if (RaiseBoardNow)
            {
                RaiseBoard();
            }
            else if (!RaiseBoardNow)
            {
                LowerBoard();
            }
        }
        

        internal void RaiseBoard()
        {
            if (BoardRaised)
            {
                return;
            }
            var _angle = API.GetVehicleDoorAngleRatio(_vehicle.Handle, BoardDoorIndex);
            if (!API.IsVehicleDoorFullyOpen(_vehicle.Handle,BoardDoorIndex) || _angle <= 0.80000001f)
            {
                API.SetVehicleDoorControl(_vehicle.Handle, BoardDoorIndex, _speed, _angle + 0.029999999f);
                if (_boardType.Equals("boots"))
                {
                    API.SetVehicleDoorControl(_vehicle.Handle, 6, _speed, _angle);
                }
            }
            if (_angle > .95)
            {
                BoardRaised = true;
            }
        }

        internal void LowerBoard()
        {
            if (!BoardRaised)
            {
                return;
            }
            var _angle = API.GetVehicleDoorAngleRatio(_vehicle.Handle, BoardDoorIndex);
            if (API.IsVehicleDoorFullyOpen(_vehicle.Handle, BoardDoorIndex) || _angle >= 0.00000001f)
            {
                API.SetVehicleDoorControl(_vehicle.Handle, BoardDoorIndex, _speed, _angle - 0.029999999f);
                if (_boardType.Equals("boots"))
                {
                    API.SetVehicleDoorControl(_vehicle.Handle, 6, _speed, _angle);
                }
            }
            if (_angle == 0)
            {
                BoardRaised = false;
            }
        }
    }
}
