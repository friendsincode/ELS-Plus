using CitizenFX.Core;

namespace ELS
{
    public static class Extentions
    {
        public static bool IsEls(this Vehicle vehicle)
        {
            return configuration.VCF.ELSVehicle.Exists(obj => obj.FileName == vehicle.DisplayName);
        }

        public static void CleanUp(this PoolObject poolObject)
        {
            if (!poolObject.Exists()) poolObject.CleanUp();
        }

    }

    public interface CleanUP :
    {

    }
}
