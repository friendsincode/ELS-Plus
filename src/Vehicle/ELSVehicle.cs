using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using ELS.configuration;

namespace ELS
{
    public class ELSVehicle : PoolObject, FullSync.IFullSyncComponent
    {
        private Siren.Siren _siren;
        private Light.Lights _light;
        private Vehicle _vehicle;
        private Vcfroot _vcf;
        public ELSVehicle(int handle) : base(handle)
        {
            _vehicle = new Vehicle(handle);
            ModelLoaded();

            if (_vehicle.DisplayName == "CARNOTFOUND" || _vehicle.GetNetworkId()==0) {
                throw new Exception("Vehicle creation failure.");
            }
            else if( VCF.ELSVehicle.Exists(item => item.modelHash == _vehicle.Model))
            {
                _vcf = VCF.ELSVehicle.Find(item => item.modelHash == _vehicle.Model).root;
            }
 
                //_vehicle.SetExistOnAllMachines(true);
#if DEBUG
                CitizenFX.Core.Debug.WriteLine(CitizenFX.Core.Native.API.IsEntityAMissionEntity(_vehicle.Handle).ToString());

                CitizenFX.Core.Debug.WriteLine($"registering netid:{_vehicle.GetNetworkId()}\n" +
                    $"Does entity belong to this script: {CitizenFX.Core.Native.API.DoesEntityBelongToThisScript(_vehicle.Handle, false)}");

#endif
                _siren = new Siren.Siren(_vehicle,_vcf);
                _light = new Light.Lights(_vehicle, _vcf);
           
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"created vehicle");
#endif
        }
        public ELSVehicle(int handle, [Optional]IDictionary<string, object> data) : base(handle)
        {
            _vehicle = new Vehicle(handle);
            ModelLoaded();

            if (_vehicle.DisplayName == "CARNOTFOUND" || _vehicle.GetNetworkId() == 0)
            {
                throw new Exception("Vehicle creation failure.");
            }
            else if (VCF.ELSVehicle.Exists(item => item.modelHash == _vehicle.Model))
            {
                _vcf = VCF.ELSVehicle.Find(item => item.modelHash == _vehicle.Model).root;
            }
            
                _siren = new Siren.Siren(_vehicle, _vcf, (IDictionary<string, object>)data["siren"]);
                _light = new Light.Lights(_vehicle, _vcf, (IDictionary<string, object>)data["light"]);
            
                //_vehicle.SetExistOnAllMachines(true);
#if DEBUG
                CitizenFX.Core.Debug.WriteLine(CitizenFX.Core.Native.API.IsEntityAMissionEntity(_vehicle.Handle).ToString());

                CitizenFX.Core.Debug.WriteLine($"registering netid:{_vehicle.GetNetworkId()}\n" +
                    $"Does entity belong to this script:{CitizenFX.Core.Native.API.DoesEntityBelongToThisScript(_vehicle.Handle, false)}");

#endif

#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"created vehicle");
#endif
        }
        private async void  ModelLoaded()
        {
            while (_vehicle.DisplayName == "CARNOTFOUND")
            {
                await CitizenFX.Core.BaseScript.Delay(0);
            }
        }
        internal void CleanUP()
        {
            _siren.CleanUP();
            _light.CleanUP();
            CitizenFX.Core.Debug.WriteLine("running vehicle deconstructor");
            CitizenFX.Core.Native.API.NetworkUnregisterNetworkedEntity(_vehicle.Handle);
            //CitizenFX.Core.Native.API.NetworkSetMissionFinished();
            //_vehicle.MarkAsNoLongerNeeded();
        }

        internal void RunTick()
        {
            _siren.Ticker();
            _light.Ticker();
        }
        internal void RunExternalTick()
        {
            _siren.ExternalTicker();
            _light.ExternalTicker();
        }
        internal Vector3 GetBonePosistion()
        {
            return _vehicle.Bones["door_dside_f"].Position;
        }

        public override bool Exists()
        {
            return CitizenFX.Core.Native.Function.Call<bool>(CitizenFX.Core.Native.Hash.DOES_ENTITY_EXIST, _vehicle);
        }

        public override void Delete()
        {
            _vehicle.Delete();
        }

        internal void UpdateRemoteSiren(string command, bool state)
        {
            _siren.SirenControlsRemote(command, state);
        }

        internal void UpdateRemoteLights()
        {
            _light.LightsControlsRemote();
        }

        internal int GetNetworkId()
        {
            return _vehicle.GetNetworkId();
        }
        /// <summary>
        /// Proxies sync data to the lighting and siren sub components
        /// </summary>
        /// <param name="dataDic"></param>
        public void SetData(IDictionary<string, object> data)
        {
            _siren.SetData((IDictionary<string, object>)data["siren"]);
            _light.SetData((IDictionary<string, object>)data["light"]);

        }

        public Dictionary<string, object> GetData()
        {
            Dictionary<string, object> vehDic = new Dictionary<string, object>
            {
                {"siren",_siren.GetData() },
                {"light",_light.GetData() },
                {"NetworkID",_vehicle.GetNetworkId() }
            };
            return vehDic;
        }
    }
}