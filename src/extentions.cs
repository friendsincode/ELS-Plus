using CitizenFX.Core;

namespace ELS
{
    public static class Extentions
    {
        public static bool IsEls(this Vehicle vehicle)
        {
            return configuration.VCF.ELSVehicle.Exists(obj => obj.FileName == vehicle.DisplayName);
        }
        
    }
}
