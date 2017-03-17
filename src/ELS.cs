using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.Sirens;
using Control = CitizenFX.Core.Control;
using CitizenFX.Core.Native;
using System;

namespace ELS
{
    public class ELS : BaseScript
    {
        private SirenManager _sirenManager;
        private FileLoader _FileLoader;
        private configuration.ControlConfiguration controlConfiguration;
        public ELS()
        {
            controlConfiguration = new configuration.ControlConfiguration();
            _FileLoader = new FileLoader(this);
            _sirenManager = new SirenManager();
            EventHandlers["onClientResourceStart"] += new Action<string>(_FileLoader.RunLoadeer);
            EventHandlers["ELS:SirenUpdated"] += new Action<int>(_sirenManager.UpdateSirens);
            Tick += Class1_Tick;
        }

        private async Task Class1_Tick()
        {

            if (LocalPlayer.WantedLevel > 0)
            {
                Screen.ShowNotification("wanted level");
                LocalPlayer.WantedLevel = 0;
            }
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
            //if (Game.IsControlJustReleased(0, Control.NextCamera))
            //{
            //    Game.DisableControlThisFrame(0, Control.NextCamera);
            //    sirenState.ToggleBlackOut();
            //}
        }
    }
}

