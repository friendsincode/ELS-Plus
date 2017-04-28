/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using CitizenFX.Core;
using CitizenFX.Core.Native;
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

        public enum Commands
        {
            MainSiren,
            AirHorn
        }
        public enum MessageTypes
        {
            SirenUpdate,
            SirenAdded,
            SirenRemoved,
            LightUpdate
        }
        public delegate void RemoteMessageRecievedHandler();
        public static event RemoteMessageRecievedHandler RemoteMessageRecieved;
        public static void SendEvent(Commands type,Vehicle vehicle,string siren,bool state)
        {
            var netID = Function.Call<int>(Hash.VEH_TO_NET, vehicle.Handle);
            var ped = vehicle.GetPedOnSeat(VehicleSeat.Driver);
            BaseScript.TriggerServerEvent("ELS",type.ToString(),Game.Player.ServerId,siren,state);
            //BaseScript.TriggerServerEvent("ELS", type, netID);
        }
    }
}
