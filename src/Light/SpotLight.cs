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

        private ILight lights;
        private bool _on;
        internal bool TurnedOn
        {
            get { return _on; }
            set
            {
                _on = value;
                if (!value && Global.ResetTakedownSpotlight)
                {
                    SpotLightReset();
                }
            }
        }
        public SpotLight(ILight light)
        {
            lights = light;
           
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
            dic.Add("TurnedOn", TurnedOn);
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
            anglehorizontal = float.Parse(data["horizontal"].ToString());
            angleVertical = float.Parse(data["vertical"].ToString());
            TurnedOn = bool.Parse(data["TurnedOn"].ToString());
            Utils.DebugWriteLine($"Got spotlight data for {lights._vehicle.Plate()} set horizontal {anglehorizontal} and vertical {angleVertical}");

        }

        public void RunTick()
        {
           
            Utils.DebugWriteLine($"Spotlight veh handle of {lights._vehicle.Handle}");
            if (Game.IsControlPressed(0, Control.PhoneLeft) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.Plate() == lights._vehicle.Plate())
            {
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightLeft, lights._vehicle, true, Game.Player.ServerId);
                anglehorizontal++;
                
            }
            if (Game.IsControlPressed(0, Control.PhoneRight) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.Plate() == lights._vehicle.Plate()) 
            {
                
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightRight, lights._vehicle, true, Game.Player.ServerId);
                anglehorizontal--;
            }
            if (Game.IsControlPressed(0, Control.PhoneUp) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.Plate() == lights._vehicle.Plate())
            {
                
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightUp, lights._vehicle, true, Game.Player.ServerId);
                angleVertical++;
            }
            if (Game.IsControlPressed(0, Control.PhoneDown) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.Plate() == lights._vehicle.Plate())   
            {
                
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightDown, lights._vehicle, true, Game.Player.ServerId);
                angleVertical--;
            }

            //var spotoffset = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(-0.9f, 1.15f, 0.5f));
            
            var off = lights._vehicle.GetPositionOffset(lights._vehicle.Bones[$"window_lf"].Position);

            var spotoffset = lights._vehicle.GetOffsetPosition(off+new Vector3(-.25f,1f,0.1f));
            
            //Vector3 myPos = Game.PlayerPed.CurrentVehicle.Bones[$"extra_{_id}"].Position;
            float hx = (float)((double)spotoffset.X + 5 * Math.Cos(((double)anglehorizontal + lights._vehicle.Rotation.Z) * Math.PI / 180.0));
            float hy = (float)((double)spotoffset.Y + 5 * Math.Sin(((double)anglehorizontal + lights._vehicle.Rotation.Z) * Math.PI / 180.0));
            float vz = (float)((double)spotoffset.Z + 5 * Math.Sin((double)angleVertical * Math.PI / 180.0));

            Vector3 destinationCoords = (new Vector3(hx, hy, vz));
            
            dirVector = destinationCoords - spotoffset;
            dirVector.Normalize();
            //Function.Call(Hash.DRAW_SPOT_LIGHT, spotoffset.X, spotoffset.Y, spotoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 100.0f, 1f, 0.0f, 13.0f, 1f,100f);
            API.DrawSpotLightWithShadow(spotoffset.X,spotoffset.Y,spotoffset.Z, dirVector.X,dirVector.Y,dirVector.Z, 255,255,255,Global.TkdnRng,Global.TkdnInt,1f,18f,1f,0);
        }

    }
}
