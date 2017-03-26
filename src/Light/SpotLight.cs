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

        public SpotLight()
        {
            
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

            var off = Game.Player.Character.CurrentVehicle.GetPositionOffset(
                Game.Player.Character.CurrentVehicle.Bones["extra_1"].Position);
            var spotoffset = Game.Player.Character.CurrentVehicle.GetOffsetPosition(off+new Vector3(0,0.05f,0));
            
            Vector3 myPos = Game.Player.Character.CurrentVehicle.Bones["extra_1"].Position;
            float hx = (float)((double)spotoffset.X + 5 * Math.Cos(((double)anglehorizontal + Game.Player.Character.CurrentVehicle.Rotation.Z) * Math.PI / 180.0));
            float hy = (float)((double)spotoffset.Y + 5 * Math.Sin(((double)anglehorizontal + Game.Player.Character.CurrentVehicle.Rotation.Z) * Math.PI / 180.0));
            float vz = (float)((double)spotoffset.Z + 5 * Math.Sin((double)anngleVirtical * Math.PI / 180.0));

            Vector3 destinationCoords = (new Vector3(hx,
           hy, vz));
            
            dirVector = destinationCoords - spotoffset;
            dirVector.Normalize();
            Function.Call(Hash._DRAW_SPOT_LIGHT_WITH_SHADOW, spotoffset.X, spotoffset.Y, spotoffset.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 100.0f, 1f, 0.0f, 13.0f, 1f,100f);

        }

    }
}
