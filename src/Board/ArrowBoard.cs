using CitizenFX.Core.Native;
using ELS.FullSync;
using ELS.Light;

namespace ELS.Board
{
    public struct ArrowBoardFSData
    {
        public bool Raised { get; set; }
    }
    internal class ArrowBoard : IFullSyncComponent<ArrowBoardFSData>
    {
        ILight lights;
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
                if (RaiseBoardNow)
                {
                    // RaiseBoard();
                    API.SetVehicleDoorOpen(lights._vehicle.Handle, BoardDoorIndex, false, false);
                    BoardRaised = true;
                }
                else
                {
                    //LowerBoard();
                    API.SetVehicleDoorShut(lights._vehicle.Handle, BoardDoorIndex, false);
                    BoardRaised = false;
                }
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

        public ArrowBoardFSData GetData()
        {
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("raised", RaiseBoardNow);
            return new ArrowBoardFSData()
            {
                Raised = RaiseBoardNow
            };
        }

        public void SetData(ArrowBoardFSData data)
        {
            BoardRaised = data.Raised;
        }

        internal ArrowBoard(ILight light, configuration.MISC misc)
        {
            lights = light;
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
            Utils.DebugWriteLine($"Added ArrowBoard of {_boardType}");
        }
    }

}
