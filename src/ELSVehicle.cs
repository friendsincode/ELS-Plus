using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace ELS
{
    public class ELSVehicle :PoolObject
    {
        private Siren.Siren _siren;
        private Vehicle _vehicle;
        private Light.Light = new 
        public ELSVehicle(int handle) : base(handle)
        {
            _vehicle = new Vehicle(handle);
            _siren = new Siren.Siren(_vehicle);
            Debug.WriteLine($"created vehicle");
        }

        internal void RunTick()
        {
            _siren.ticker();
            if (Game.IsControlJustPressed(0,Control.Cover))
            {
                RunFullSync();
            }
        }
        public override bool Exists()
        {
            return _vehicle.Exists();
        }

        public override void Delete()
        {
           _vehicle.Delete();
        }

        private void RunFullSync()
        {
            _siren.FullSync();
        }

        internal void SyncData(string dataType, IDictionary<string, object> dataDic)
        {
            _siren.SetFullSync(dataType,dataDic);
        }
        internal void SendSirenCommand(string command,bool state)
        {
            _siren.updateLocalRemoteSiren(command,state);
        }
    }
}