using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace ELS
{
    public class ELSVehicle : PoolObject
    {
        private Siren.Siren _siren;
        private Vehicle _vehicle;
        public ELSVehicle(int handle) : base(handle)
        {
            _vehicle = new Vehicle(handle);
            _siren = new Siren.Siren(_vehicle);
            CitizenFX.Core.Debug.WriteLine($"created vehicle");
        }
        internal void CleanUP()
        {
            _siren.CleanUP();
        }

        internal void RunTick()
        {
            _siren.Ticker();
            if (Game.IsControlJustPressed(0, Control.Cover))
            {
                RunFullSync();
                CitizenFX.Core.UI.Screen.ShowNotification("FullSync™ ran");
                CitizenFX.Core.Debug.WriteLine("FullSync™ ran");
            }
        }
        internal void RunExternalTick()
        {
            _siren.ExternalTicker();
        }
        internal Vector3 GetBonePosistion()
        {
            return _vehicle.Bones["door_dside_f"].Position;
        }
        public override bool Exists()
        {
            return CitizenFX.Core.Native.Function.Call<bool>(CitizenFX.Core.Native.Hash.DOES_ENTITY_EXIST,_vehicle);
        }

        public override void Delete()
        {
            _vehicle.Delete();
        }

        internal void RunFullSync()
        {
            _siren.RunSendSync();
        }

        internal void SetSyncData(IDictionary<string, object> dataDic)
        {
            _siren.SetData(dataDic);
        }
        internal void UpdateRemoteSiren(string command, bool state)
        {
            _siren.SirenControlsRemote(command, state);
        }
        internal long GetNetworkId()
        {
            return this._vehicle.GetNetworkId();
        }
    }
}