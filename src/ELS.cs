using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.Siren;
using Control = CitizenFX.Core.Control;
using CitizenFX.Core.Native;
using System;
using System.Drawing;
using System.Reflection;
using System.Security.Permissions;
using ELS.configuration;

namespace ELS
{
    public class ELS : BaseScript
    {
        public static int localplayerid;
        public static bool isStopped;
        private SirenManager _sirenManager;
        private FileLoader _FileLoader;
        private configuration.ControlConfiguration controlConfiguration;

        public ELS()
        {
            localplayerid = LocalPlayer.ServerId;
            controlConfiguration = new configuration.ControlConfiguration();
            _FileLoader = new FileLoader(this);
            _sirenManager = new SirenManager();
            EventHandlers["onClientResourceStart"] += new Action<string>(
                (string obj) =>
                {
                    _FileLoader.RunLoadeer(obj);
                    Tick += Class1_Tick;
                });
            EventHandlers["ELS:SirenUpdated"] += new Action<int,string,bool>(_sirenManager.UpdateSirens);
            EventHandlers["onClientResourceStop"] += new Action(() => {
                Debug.WriteLine("Stop");
                isStopped = true;
                _sirenManager.CleanUp();
            });
            EventHandlers["onPlayerJoining"] += new Action(() =>
              {
                  localplayerid = LocalPlayer.ServerId;
              });
        }

        private Task Class1_Tick()
        {
            Text text = new Text($"ELS Build v0.0.2.3\nhttp://ELS.ejb1123.tk", new PointF(640f, 10f), 0.5f);
            text.Alignment = Alignment.Center;
            text.Centered = true;
            text.Draw();
            
            if (LocalPlayer.Character.IsInVehicle() && LocalPlayer.Character.IsSittingInVehicle() && LocalPlayer.Character.CurrentVehicle.HasSiren && LocalPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver)==LocalPlayer.Character)
            {
                //Vector3 myPos = Game.Player.Character.CurrentVehicle.Bones["extra_1"].Position;
                //Vector3 destinationCoords = Game.Player.Character.GetOffsetPosition(new Vector3(0,
                //    20, 0));
                //Vector3 dirVector = destinationCoords - myPos;
                //dirVector.Normalize();
                //Function.Call(Hash._DRAW_SPOT_LIGHT_WITH_SHADOW, myPos.X, myPos.Y, myPos.Z, dirVector.X, dirVector.Y, dirVector.Z, 255, 255, 255, 100.0f, 1f, 0.0f, 13.0f, 1f,10f);
               _sirenManager.Runtick();
            }
            return null;
        }
    }
}

