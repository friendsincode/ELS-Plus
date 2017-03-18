using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.Siren;
using Control = CitizenFX.Core.Control;
using CitizenFX.Core.Native;
using System;
using System.Security.Permissions;

namespace ELS
{
    public class ELS : BaseScript
    {
        static PlayerList _playerList;
        public static int localplayerid;
        public static bool isStopped;
        private SirenManager _sirenManager;
        private FileLoader _FileLoader;
        private configuration.ControlConfiguration controlConfiguration;
       // [SecurityPermission(SecurityAction.Demand,Flags =SecurityPermissionFlag.ControlAppDomain)]
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
                    handleExistingVehicle();
                });
            EventHandlers["ELS:SirenUpdated"] += new Action<int,string,bool>(_sirenManager.UpdateSirens);
            EventHandlers["onClientResourceStop"] += new Action(() => {
                Debug.WriteLine("Stop");
                isStopped = true;
                _sirenManager.CleanUp();
            });
            EventHandlers["onPlayerJoining"] += new Action(() =>
              {
                  _playerList = Players;
                  localplayerid = LocalPlayer.ServerId;
              });
            Tick += Class1_Tick;
        }

        private void handleExistingVehicle()
        {
            if (LocalPlayer.Character.IsInVehicle())
            {
                _sirenManager.SetCurrentSiren(LocalPlayer.Character.CurrentVehicle);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task Class1_Tick()
        {
            if (LocalPlayer.Character.IsGettingIntoAVehicle)
            {
                if (LocalPlayer.Character.VehicleTryingToEnter != null)
                {
                    _sirenManager.SetCurrentSiren(LocalPlayer.Character.VehicleTryingToEnter);
                }
            }
            if (LocalPlayer.Character.IsInVehicle())
            {
                Screen.ShowNotification(Function.Call<int>(Hash.NETWORK_GET_NETWORK_ID_FROM_ENTITY, LocalPlayer.Character.CurrentVehicle.Handle).ToString());
                _sirenManager.runtick();
            }
        }
    }
}

