using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.Sirens;
using Control = CitizenFX.Core.Control;

namespace ELS
{
    public class ELS : BaseScript
    {
        private SirenManager _sirenManager;
        public EventHandlerDictionary EventHandlerDictionary => EventHandlers;
        public ELS()
        {
            _sirenManager = new SirenManager();
            Tick += Class1_Tick;
        }

        private async Task Class1_Tick()
        {

            if (LocalPlayer.WantedLevel > 0)
            {
                CitizenFX.Core.UI.Screen.ShowNotification("wanted level");
                LocalPlayer.WantedLevel = 0;
            }
            if (LocalPlayer.Character.IsGettingIntoAVehicle)
            {
                 _sirenManager.SetCurrentSiren(LocalPlayer.Character.VehicleTryingToEnter);
            }
            if (LocalPlayer.Character.IsInVehicle())
            {
                //_sirenManager.SetCurrentSiren(LocalPlayer.Character.CurrentVehicle);
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

