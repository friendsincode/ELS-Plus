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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;
using ELS.FullSync;
using ELS.configuration;

namespace ELS.Light
{
    class SpotLight : IFullSyncComponent
    {
        private float anglehorizontal = 90f;
        private float angleVertical = 0f;

        private Vector3 dirVector;
        private int _id;
        private int _veh;

        public SpotLight(int id, int veh)
        {
            _id = id;
            _veh = veh;
        }

        internal void SpotLightReset()
        {
            anglehorizontal = 90f;
            angleVertical = 0f;
        }

        public Dictionary<string, object> GetData()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("horizontal", anglehorizontal);
            dic.Add("vertical", angleVertical);
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
            anglehorizontal = float.Parse(data["horizontal"].ToString());
            angleVertical = float.Parse(data["vertical"].ToString());
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"Got spotlight data for {_id} set horizontal {anglehorizontal} and vertical {angleVertical}");
#endif
        }

        public void RunTick()
        {
            Vehicle veh = new Vehicle(API.NetworkGetEntityFromNetworkId(_veh));
            if (Game.IsControlPressed(0, Control.PhoneLeft))
            {
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightLeft, veh, true, Game.Player.ServerId);
                anglehorizontal++;
                
            }
            if (Game.IsControlPressed(0, Control.PhoneRight)) 
            {
                
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightRight, veh, true, Game.Player.ServerId);
                anglehorizontal--;
            }
            if (Game.IsControlPressed(0, Control.PhoneUp))
            {
                
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightUp, veh, true, Game.Player.ServerId);
                angleVertical++;
            }
            if (Game.IsControlPressed(0, Control.PhoneDown))   
            {
                
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightDown, veh, true, Game.Player.ServerId);
                angleVertical--;
            }

            //var spotoffset = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(-0.9f, 1.15f, 0.5f));
            
            var off = veh.GetPositionOffset(veh.Bones[$"extra_{_id}"].Position);
            var spotoffset = veh.GetOffsetPosition(off+new Vector3(0,0.05f,0));
            
            //Vector3 myPos = Game.PlayerPed.CurrentVehicle.Bones[$"extra_{_id}"].Position;
            float hx = (float)((double)spotoffset.X + 5 * Math.Cos(((double)anglehorizontal + veh.Rotation.Z) * Math.PI / 180.0));
            float hy = (float)((double)spotoffset.Y + 5 * Math.Sin(((double)anglehorizontal + veh.Rotation.Z) * Math.PI / 180.0));
            float vz = (float)((double)spotoffset.Z + 5 * Math.Sin((double)angleVertical * Math.PI / 180.0));

            Vector3 destinationCoords = (new Vector3(hx, hy, vz));
            
            dirVector = destinationCoords - spotoffset;
            dirVector.Normalize();
            //Function.Call(Hash.DRAW_SPOT_LIGHT, spotoffset.X, spotoffset.Y, spotoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 100.0f, 1f, 0.0f, 13.0f, 1f,100f);
            API.DrawSpotLightWithShadow(spotoffset.X,spotoffset.Y,spotoffset.Z, dirVector.X,dirVector.Y,dirVector.Z, 255,255,255,Global.TkdnRng,Global.TkdnInt,1f,13f,1f,0);
        }

    }
}
