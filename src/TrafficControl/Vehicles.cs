using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELS.TrafficControl
{
    internal partial class Vehicles
    {
        
        internal static async Task MoveTraffic()
        {
            int veh = API.GetClosestVehicle(Game.PlayerPed.CurrentVehicle.Position.X, Game.PlayerPed.CurrentVehicle.Position.Y, Game.PlayerPed.CurrentVehicle.Position.Z, 50f, 0, 70);
            API.TaskVehicleTempAction(Ped.FromHandle(API.GetPedInVehicleSeat(veh,-1)).Handle,veh,32, 1000);
            Screen.ShowNotification("Move Bitch Get out the way");

        }

        static bool doesEntityExistsAndIsNotNull(Entity entity)
        {
            return (entity != null && API.DoesEntityExist(entity.Handle));
        }

        static float getDistanceBetweenEntities(Entity entity1, Entity entity2)
        {
            Vector3 entity1Coords = API.GetEntityCoords(entity1.Handle, true);
            Vector3 entity2Coords = API.GetEntityCoords(entity2.Handle, true);
            return API.GetDistanceBetweenCoords(entity1Coords.X, entity1Coords.Y, entity1Coords.Z, entity2Coords.X, entity2Coords.Y, entity2Coords.Z, true);
        }

        static List<Ped> GetNearestVehicles(float range)
        {
            
            int handle = 0;
            List<Ped> peds = new List<Ped>();
            //API.isentityin
                ELS.Delay(10);
                if (handle == 0)
                {
                    API.FindFirstPed(ref handle);
                } else
                {
                    API.FindNextPed(handle,ref handle);
                }
                Ped ped = (Ped)Ped.FromHandle(handle);
                if (getDistanceBetweenEntities(Game.PlayerPed, ped) <= range && ped.IsInVehicle())
                {
                    peds.Add(ped);
                }
            return peds;
        }
    }
}
