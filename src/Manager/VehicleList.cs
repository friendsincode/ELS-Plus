using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Manager
{
    class VehicleList : Dictionary<int,ELSVehicle>
    {
        //public new void Add(ELSVehicle veh)
        //{
        //    base.Add(veh);
        //}
        public void Add(int NetworkID)
        {
            var veh = new ELSVehicle(API.NetToVeh(NetworkID));
            Add(NetworkID,veh);
        }
        public bool IsReadOnly => throw new NotImplementedException();
        public void RunTick()
        {

        }
        public void RunExternalTick( ELSVehicle vehicle=null)
        {
            if (vehicle == null) return;
            try
            {
                foreach (var t in Values)
                {
                    if (vehicle == null ||  t.Handle!=vehicle.Handle)
                    t.RunExternalTick();
                }
            }
            catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"VehicleList Error: {e.Message}\n");
            }
        }
        public bool MakeSureItExists(int NetworkID, [Optional]out ELSVehicle vehicle, IDictionary<string, object> data = null)
        {
            if (NetworkID == 0)
            {

               // VehicleManager.makenetworked()
                Utils.DebugWriteLine("ERROR NetwordID equals 0 Can't add vehicle\n");

                CitizenFX.Core.Debug.WriteLine("ERROR Try to add vehicle\nNetwordID equals 0");

                vehicle = null;
                data = null;
                return false;
            }
            else if (!ContainsKey(NetworkID))
            {
                try
                {
                    ELSVehicle veh = null;
                    int handle = API.NetToVeh(NetworkID);
                    if (handle == 0)
                    {
                        Utils.ReleaseWriteLine("handle=0");
                    }
                    else
                    {
                        if (data != null)
                            veh = new ELSVehicle(handle, data);
                        else
                            veh = new ELSVehicle(handle, data);
                    }
                    Add(NetworkID,veh);

                    Utils.DebugWriteLine($"Adding Vehicle");

                    vehicle = veh;
                    return true;
                }
                catch (Exception ex)
                {
#if DEBUG 
                    CitizenFX.Core.Debug.Write($"Exsits Error: {ex.Message} due to {ex.InnerException}");
#endif
                    vehicle = null;
                    return false;
                    throw ex;
                }

            }
            else
            {
                vehicle = this[NetworkID];//Find(poolObject => ((ELSVehicle)poolObject).GetNetworkId() == NetworkID);
                return true;
            }
        }
        public void CleanUP()
        {
            for(int i = 0; i < Count; i++)
            {
                this.ElementAt(i).Value.CleanUP();
            }
        }
        ~VehicleList()
        {
            CleanUP();
            Clear();
        }
    }
}
