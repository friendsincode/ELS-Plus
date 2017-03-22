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
            Text text = new Text($"ELS Build v0.0.2.2\nhttp://ELS.ejb1123.tk", new PointF(640f, 10f), 0.5f);
            text.Alignment = Alignment.Center;
            text.Centered = true;
            text.Draw();

            if (LocalPlayer.Character.IsInVehicle() && LocalPlayer.Character.IsSittingInVehicle() && LocalPlayer.Character.CurrentVehicle.HasSiren && LocalPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver)==LocalPlayer.Character)
            {
                //Game.PlayerPed.CurrentVehicle.IsSirenActive = true;
                Function.Call(Hash.SET_HORN_ENABLED,LocalPlayer.Character.CurrentVehicle,false);
               // Function.Call(Hash.USE_SIREN_AS_HORN,LocalPlayer.Character.CurrentVehicle,false);
               _sirenManager.Runtick();
            }
            return null;
        }
    }
}

