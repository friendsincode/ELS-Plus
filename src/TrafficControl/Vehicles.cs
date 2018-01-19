using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace ELS.TrafficControl
{
    internal partial class Vehicles
    {
        
        internal static async Task MoveTraffic()
        {
            List<Ped> peds = GetNearestPeds(1f);
            foreach (Ped ped in peds)
            {
                if (ped.Handle != Game.PlayerPed.Handle)
                {
                    API.TaskSmartFleePed(ped.Handle, Game.PlayerPed.Handle, 1f, 10, true, false);
                    Screen.ShowNotification("Move Bitch Get out the way");
                }
            }
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

        static List<Ped> GetNearestPeds(float range)
        {
            //API.GetClosestVehicle()
            int handle = 0;
            List<Ped> peds = new List<Ped>();
            do
            {
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
            } while (peds.Count < 6);
            return peds;
        }
    }
}
