using System;
using CitizenFX.Core;

namespace ELS
{
    public class ELSVehicle :PoolObject
    {
        private Siren.Siren _siren;
        private Vehicle _vehicle;
        public ELSVehicle(int handle) : base(handle)
        {
            _vehicle = new Vehicle(handle);
            _siren = new Siren.Siren(_vehicle);
        }

        internal void RunTick()
        {
            _siren.ticker();
        }
        public override bool Exists()
        {
            return _vehicle.Exists();
        }

        public override void Delete()
        {
           _vehicle.Delete();
        }

        internal void SendSirenCommand(string command,bool state)
        {
            _siren.updateLocalRemoteSiren(command,state);
        }
    }
}