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

        internal ArrowBoard(Entity entity, configuration.MISC misc)
        {
            _vehicle = entity;
            _misc = misc;
            _boardType = _misc.ArrowboardType.ToLower();
            switch (_boardType)
            {
                case "bonnet":
                    HasBoard = true;
                    break;
                case "boot":
                    HasBoard = true;
                    break;
                case "boot2":
                    HasBoard = true;
                    break;
                case "boots":
                    HasBoard = true;
                    break;
                case "off":
                    HasBoard = false;
                    break;
                default:
                    HasBoard = false;
                    break;
            }
        }

        

        internal void RaiseBoard()
        {
            if (HasBoard)
            {
                Vehicle vehicle = (Vehicle)_vehicle;
                foreach (VehicleDoor door in vehicle.Doors)
                {
                    if (door.Index.Equals(_boardType))
                    {
                        door.Close();
                    }
                }
            }
            else
            {
                CitizenFX.Core.Debug.WriteLine("Vehicle does not have an arrowboard");
            }
        }

        internal void LowerBoard()
        {
            if (HasBoard)
            {
                Vehicle vehicle = (Vehicle)_vehicle;
                foreach(VehicleDoor door in vehicle.Doors)
                {
                    if(door.Index.Equals(_boardType))
                    {
                        door.Close();
                    }
                }
            }
            else
            {
                CitizenFX.Core.Debug.WriteLine("Vehicle does not have an arrowboard");
            }
        }
    }
}
