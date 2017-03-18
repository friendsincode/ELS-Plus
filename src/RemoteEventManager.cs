using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.Siren.Tones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS
{
    class RemoteEventManager
    {
        public RemoteEventManager()
        {
             
        }
        public enum MessageTypes
        {
            SirenUpdate,
            SirenAdded,
            SirenRemoved
        }
        public delegate void RemoteMessageRecievedHandler();
        public static event RemoteMessageRecievedHandler RemoteMessageRecieved;
        public static void SendEvent(MessageTypes type,Vehicle vehicle,string siren,bool state)
        {
            var netID = Function.Call<int>(Hash.VEH_TO_NET, vehicle.Handle);
            var ped = vehicle.GetPedOnSeat(VehicleSeat.Driver);
            BaseScript.TriggerServerEvent("ELS",type,Game.Player.ServerId,siren,state);
            //BaseScript.TriggerServerEvent("ELS", type, netID);
        }
    }
}
