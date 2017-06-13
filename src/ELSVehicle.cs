using CitizenFX.Core;

namespace ELS
{
    public class ELSVehicle :PoolObject
    {
        public ELSVehicle(int handle) : base(handle)
        {
            
        }

        internal void RunTick()
        {
            
        }
        public override bool Exists()
        {
            throw new System.NotImplementedException();
        }

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}