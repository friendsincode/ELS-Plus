using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

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

        public static Int64 GetNetWorkId(this Entity entity)
        {
            return Function.Call<Int64>(Hash.NETWORK_GET_NETWORK_ID_FROM_ENTITY, entity.Handle);
        }
    }
}
