using CitizenFX.Core;

namespace ELS.Manager
{
    class VehicleManager : Manager
    {
        public VehicleManager() : base()
        {
        }

        internal override void RunTick()
        {
            if (Game.PlayerPed.CurrentVehicle.IsEls())
            {
                AddIfNotPresint(new ELSVehicle(Game.PlayerPed.CurrentVehicle.Handle));
                
            }
        }

    }
}