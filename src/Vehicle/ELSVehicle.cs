using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using ELS.configuration;
using CitizenFX.Core.Native;
using ELS.Manager;
using System.Threading.Tasks;

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

            if (_vehicle.DisplayName == "CARNOTFOUND") {
                throw new Exception("Vehicle creation failure.");
            }
            else if (_vehicle.GetNetworkId() == 0)
            {
                throw new Exception("NetworkId is 0");
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

            if (_vehicle.DisplayName == "CARNOTFOUND")
            {
                throw new Exception("Vehicle not found");
            } 
            else if (_vehicle.GetNetworkId() == 0)
            {
                throw new Exception("NetworkId is 0");
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
            if (!_vehicle.IsEngineRunning)
            {
#if DEBUG
                CitizenFX.Core.Debug.WriteLine("Engine not running starting it");
#endif
                _vehicle.IsEngineRunning = true;
            }
            if (lastDeleteTime == 0)
            {
                lastDeleteTime = Game.GameTime;
            }
            if (Game.GameTime - lastDeleteTime >= 180000)
            {
                DeleteStale();
                lastDeleteTime = Game.GameTime;
            }
        }

        int lastDeleteTime = 0;
        private async Task DeleteStale()
        {
            PlayerList list = new PlayerList();
#if DEBUG
            CitizenFX.Core.Debug.WriteLine("Running Delete");
#endif
            foreach (Player p in list)
            {
                if (_vehicle.GetNetworkId() != p.Character.CurrentVehicle.GetNetworkId())
                {
#if DEBUG
                    CitizenFX.Core.Debug.WriteLine($"Deleting {_vehicle.GetNetworkId()}");
#endif
                    Delete();
                }
            }
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
            try
            {
                VehicleManager.vehicleList.RemoveAt(VehicleManager.vehicleList.IndexOf(VehicleManager.vehicleList.Find(v => v.GetNetworkId() == _vehicle.GetNetworkId())));
                _light.CleanUP();
                _siren.CleanUP();
                _vehicle.SetExistOnAllMachines(false);
                API.SetEntityAsMissionEntity(_vehicle.Handle, false, false);
                _vehicle.MarkAsNoLongerNeeded();
                _vehicle.Delete();
            } catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"Delete error: {e.Message}");
            }
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