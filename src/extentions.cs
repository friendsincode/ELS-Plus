using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS
{
    public static class Extentions
    {
        public static bool IsEls(this Vehicle vehicle)
        {
            return configuration.VCF.ELSVehicle.Exists(obj => obj.Item2.FileName == vehicle.DisplayName);
        }

        public static void CleanUp(this PoolObject poolObject)
        {
            if (!poolObject.Exists()) poolObject.CleanUp();
        }

        public static Int64 GetNetworkId(this Entity entity)
        {
            return Function.Call<Int64>(Hash.VEH_TO_NET, entity.Handle);
        }

        public static void RegisterAsNetworked(this Entity entity)
        {
            Function.Call((Hash)0x06FAACD625D80CAA, entity.Handle);
        }

        public static void SetExistOnAllMachines(this Entity entity, bool b)
        {
            Function.Call(Hash.SET_NETWORK_ID_EXISTS_ON_ALL_MACHINES, entity.GetNetworkId(), b);
        }
    }
}
