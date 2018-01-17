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

namespace ELS.Light
{
    class SpotLight
    {
        private float anglehorizontal = 90f;
        private float anngleVirtical = 0f;

        private Vector3 dirVector;
        private int _id;

        public SpotLight(int id)
        {
            _id = id;
        }

        public void RunTick()
        {

            if (Game.IsControlPressed(0, Control.PhoneLeft))
            {
                anglehorizontal++;
            }
            if (Game.IsControlPressed(0, Control.PhoneRight))
            {
                anglehorizontal--;
            }
            if (Game.IsControlPressed(0, Control.PhoneUp))
            {
                anngleVirtical++;
            }
            if (Game.IsControlPressed(0, Control.PhoneDown))
            {
                anngleVirtical--;
            }
            
            //var spotoffset = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(-0.9f, 1.15f, 0.5f));

            var off = Game.PlayerPed.CurrentVehicle.GetPositionOffset(Game.PlayerPed.CurrentVehicle.Bones[$"extra_{_id}"].Position);
            var spotoffset = Game.PlayerPed.CurrentVehicle.GetOffsetPosition(off+new Vector3(0,0.05f,0));
            
            Vector3 myPos = Game.PlayerPed.CurrentVehicle.Bones[$"extra_{_id}"].Position;
            float hx = (float)((double)spotoffset.X + 5 * Math.Cos(((double)anglehorizontal + Game.PlayerPed.CurrentVehicle.Rotation.Z) * Math.PI / 180.0));
            float hy = (float)((double)spotoffset.Y + 5 * Math.Sin(((double)anglehorizontal + Game.PlayerPed.CurrentVehicle.Rotation.Z) * Math.PI / 180.0));
            float vz = (float)((double)spotoffset.Z + 5 * Math.Sin((double)anngleVirtical * Math.PI / 180.0));

            Vector3 destinationCoords = (new Vector3(hx,
           hy, vz));
            
            dirVector = destinationCoords - spotoffset;
            dirVector.Normalize();
            Function.Call(Hash._DRAW_SPOT_LIGHT_WITH_SHADOW, spotoffset.X, spotoffset.Y, spotoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 100.0f, 1f, 0.0f, 13.0f, 1f,100f);
        }

    }
}
