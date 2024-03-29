﻿/*
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
using ELS.configuration;
using ELS.FullSync;
using System;


namespace ELS.Light
{

    public struct SpotLightFSData
    {
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
        public bool TurnedOn { get; set; }
    }

    class SpotLight : IFullSyncComponent<SpotLightFSData>
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

        public SpotLightFSData GetData()
        {
            return new SpotLightFSData() { Vertical = angleVertical, Horizontal = anglehorizontal, TurnedOn = TurnedOn };
        }

        public void SetData(SpotLightFSData data)
        {
            anglehorizontal = data.Horizontal;
            angleVertical = data.Vertical;
            TurnedOn = data.TurnedOn;
            Utils.DebugWriteLine($"Got spotlight data for {lights._vehicle.GetElsId()} set horizontal {anglehorizontal} and vertical {angleVertical}");

        }

        long update = Game.GameTime;
        public void RunControlTick()
        {
            
            if (Game.IsControlPressed(0, Control.PhoneLeft) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetElsId() == lights._vehicle.GetElsId())
            {
                anglehorizontal++;
                
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightLeft, lights._vehicle, true, Game.Player.ServerId);
            }
            if (Game.IsControlPressed(0, Control.PhoneRight) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetElsId() == lights._vehicle.GetElsId())
            {
                anglehorizontal--;
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightRight, lights._vehicle, true, Game.Player.ServerId);
            }
            if (Game.IsControlPressed(0, Control.PhoneUp) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetElsId() == lights._vehicle.GetElsId())
            {
                angleVertical++;
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightUp, lights._vehicle, true, Game.Player.ServerId);
            }
            if (Game.IsControlPressed(0, Control.PhoneDown) && Game.PlayerPed.IsSittingInELSVehicle() && Game.PlayerPed.CurrentVehicle.GetElsId() == lights._vehicle.GetElsId())
            {
                angleVertical--;
                //RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlightDown, lights._vehicle, true, Game.Player.ServerId);
            }
            if (Game.GameTime >= update + 1000)
            {
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MoveSpotlight, lights._vehicle, true, Game.Player.ServerId);
                update = Game.GameTime;
            }
        }


        public void RunTick()
        {
            Utils.DebugWriteLine($"Spotlight veh handle of {lights._vehicle.Handle}");
            

            //var spotoffset = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(-0.9f, 1.15f, 0.5f));

            var off = lights._vehicle.GetPositionOffset(lights._vehicle.Bones[$"window_lf"].Position);

            var spotoffset = lights._vehicle.GetOffsetPosition(off + new Vector3(-.25f, 1f, 0.1f));

            //Vector3 myPos = Game.PlayerPed.CurrentVehicle.Bones[$"extra_{_id}"].Position;
            float hx = (float)((double)spotoffset.X + 5 * Math.Cos(((double)anglehorizontal + lights._vehicle.Rotation.Z) * Math.PI / 180.0));
            float hy = (float)((double)spotoffset.Y + 5 * Math.Sin(((double)anglehorizontal + lights._vehicle.Rotation.Z) * Math.PI / 180.0));
            float vz = (float)((double)spotoffset.Z + 5 * Math.Sin((double)angleVertical * Math.PI / 180.0));

            Vector3 destinationCoords = (new Vector3(hx, hy, vz));
            dirVector = destinationCoords - spotoffset;
            dirVector.Normalize();
            //Function.Call(Hash.DRAW_SPOT_LIGHT, spotoffset.X, spotoffset.Y, spotoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 100.0f, 1f, 0.0f, 13.0f, 1f,100f);
            API.DrawSpotLightWithShadow(spotoffset.X, spotoffset.Y, spotoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, Global.TkdnRng, Global.TkdnInt, 1f, 18f, 1f, 0);
        }

    }
}
