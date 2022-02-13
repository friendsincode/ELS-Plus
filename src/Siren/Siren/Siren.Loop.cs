using CitizenFX.Core;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        public void Ticker()
        {
            
                Game.DisableControlThisFrame(0, configuration.ElsConfiguration.KBBindings.Sound_Ahorn);
                AirHornControlsKB();
                ManualTone1ControlsKB();
                ManualTone2ControlsKB();
                ManualTone3ControlsKB();
                ManualTone4ControlsKB();
                ManualSoundControlsKB();
                MainSirenToggleControlsKB();
                DualSirenControlsKB();
        }
        public void ExternalTicker()
        {
            PanicAlarmControlsKB();
        }
    }
}
