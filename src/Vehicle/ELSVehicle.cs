using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.Light;
using ELS.Manager;
using ELS.Siren;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace ELS
{
    public struct ELSVehicleFSData
    {
        public LightFSData Light { get; set; }
        public SirenFSData Siren { get; set; }
        public int Id { get; set; }
    }


    public class ELSVehicle : PoolObject, FullSync.IFullSyncComponent<ELSVehicleFSData>
    {
        private Siren.Siren _siren;
        private Light.Lights _light;
        private Vehicle _vehicle;
        private Vcfroot _vcf;
        int lastdrivetime;
        int cachedElsID;


        public ELSVehicle(int handle, [Optional] ELSVehicleFSData data) : base(handle)
        {

            _vehicle = new Vehicle(handle);
            if (!_vehicle.Exists())
            {
                return;
            }
            ModelLoaded();
            lastdrivetime = Game.GameTime;
            API.SetVehRadioStation(_vehicle.Handle, "OFF");
            API.SetVehicleRadioEnabled(_vehicle.Handle, false);

            if (_vehicle.DisplayName == "CARNOTFOUND")
            {
                throw new Exception("ELSVehicle.cs: Vehicle not found");
            }
            else if (VCF.ELSVehicle.ContainsKey(_vehicle.Model))
            {
                _vcf = VCF.ELSVehicle[_vehicle.Model].root;
            }
            if (API.DecorExistOn(_vehicle.Handle, "elsplus_id"))
            {
                cachedElsID = _vehicle.GetElsId();
            }
            else
            {
                Utils.ReleaseWriteLine("ELSVehicle.cs: Vehicle is being created without a els id attempting to set now");
                if (data.Id > 0)
                {
                    cachedElsID = data.Id;
                    API.DecorSetInt(_vehicle.Handle, "elsplus_id", cachedElsID);
                }
            }
            try
            {
                Function.Call((Hash)0x5f3a3574, _vehicle.Handle, true);
            }
            catch (Exception e)
            {
                Utils.ReleaseWriteLine("ELSVehicle.cs: Repair Fix is not enabled on this client");
            }
            Utils.DebugWriteLine($"ELS Vehicle Data: {JsonConvert.SerializeObject(data)}");
            if (data.Id.Equals(null))
            {
                _light = new Light.Lights(_vehicle, _vcf);
                _siren = new Siren.Siren(_vehicle, _vcf, _light);
            }
            else
            {
                _light = new Light.Lights(_vehicle, _vcf, data.Light);
                _siren = new Siren.Siren(_vehicle, _vcf,  _light, data.Siren);
            }
            
            _light.SetGTASirens(false);


            Utils.DebugWriteLine(API.IsEntityAMissionEntity(_vehicle.Handle).ToString());
            Utils.DebugWriteLine($"ELSVehicle.cs:registering \n" +
                $"Does entity belong to this script:{API.DoesEntityBelongToThisScript(_vehicle.Handle, false)}");
            Utils.DebugWriteLine($"ELSVehicle.cs:created vehicle");
            FullSync.FullSyncManager.SendDataBroadcast(JsonConvert.SerializeObject(GetData()), Game.Player.ServerId);
        }

        private async void ModelLoaded()
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
            Utils.DebugWriteLine("ELSVehicle.cs:running vehicle cleanup");
        }

        internal Vehicle GetVehicle { get { return _vehicle; } }

        internal void RunControlTick()
        {
            //if (_vehicle.IsDead)
            //{
            //    //VehicleManager.vehicleList.Remove(plate);
            //    ELS.TriggerServerEvent("ELS:FullSync:RemoveStale", plate);
            //    return;
            //}
            _siren.Ticker();
            _light.ControlTicker();
        }

        internal void RunTick()
        {
            if (_vehicle.IsDead)
            {
                VehicleManager.vehicleList.Remove(cachedElsID);
                ELS.TriggerServerEvent("ELS:FullSync:RemoveStale", cachedElsID);
                return;
            }
            _light.Ticker();

            if (_siren._mainSiren._enable && _light._stage.CurrentStage != 3)
            {
                _siren._mainSiren.SetEnable(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MainSiren, _vehicle, true, Game.Player.ServerId);
            }
        }

        internal void RunExternalTick()
        {
            if (_vehicle.IsDead)
            {
                VehicleManager.vehicleList.Remove(cachedElsID);
                ELS.TriggerServerEvent("ELS:FullSync:RemoveStale", cachedElsID);
                return;
            }
            _siren.ExternalTicker();
        }

        internal Vector3 GetBonePosistion()
        {
            return _vehicle.Bones["door_dside_f"].Position;
        }

        internal void SyncUi()
        {
            _light.SyncUi();
            _siren.SyncUi();
        }

        internal int GetStage() => _light._stage.CurrentStage;

        public override bool Exists()
        {
            return Function.Call<bool>(Hash.DOES_ENTITY_EXIST, _vehicle);
        }

        public void DisableSiren()
        {
            if (_siren._mainSiren._enable)
            {
                _siren._mainSiren.SetEnable(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.MainSiren, _vehicle, true, Game.Player.ServerId);
            }
            if (_siren.dual_siren)
            {
                _siren._tones.tone1.SetState(false);
                _siren._tones.tone2.SetState(false);
                _siren._tones.tone3.SetState(false);
                _siren._tones.tone4.SetState(false);
                RemoteEventManager.SendEvent(RemoteEventManager.Commands.DualSiren, _vehicle, true, Game.Player.ServerId);
            }
        }

        public override void Delete()
        {
            try
            {

                _light.CleanUP();
                _siren.CleanUP();
                _vehicle.SetExistOnAllMachines(false);
                ELS.TriggerServerEvent("ELS:FullSync:RemoveStale", _vehicle.GetElsId());
                API.SetEntityAsMissionEntity(_vehicle.Handle, true, true);
                //VehicleManager.vehicleList.Remove(_vehicle.Plate());
                _vehicle.Delete();
            }
            catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"ELSVehicle.cs:Delete error: {e.Message}");
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

        /// <summary>
        /// Proxies sync data to the lighting and siren sub components
        /// </summary>
        /// <param name="dataDic"></param>
        public void SetData(ELSVehicleFSData data)
        {
            int id = _vehicle.GetElsId();
            if (!data.Siren.Equals(null) && cachedElsID == data.Id && data.Id == id)
            {
                Utils.DebugWriteLine($"ELSVehicle.cs: Got siren data for vehicle {_vehicle.Handle} with cached id of {cachedElsID} and decor {id} with dict id {data.Id}");
                _siren.SetData(data.Siren);
            }
            if (!data.Light.Equals(null) && cachedElsID == data.Id && data.Id == id)
            {
                Utils.DebugWriteLine($"ELSVehicle.cs: Got light data for vehicle {_vehicle.Handle} with cached id of {cachedElsID} and decor {id} with dict id {data.Id}");
                _light.SetData(data.Light);
            }
        }

        public ELSVehicleFSData GetData()
        {
            return new ELSVehicleFSData()
            {
                Siren = _siren.GetData(),
                Light = _light.GetData(),
                Id = _vehicle.GetElsId()
            };
        }

        internal void SetSaveSettings(UserSettings.ELSUserVehicle veh)
        {
            _light.CurrentPrmPattern = veh.PrmPatt;
            _light.CurrentSecPattern = veh.SecPatt;
            _light.CurrentWrnPattern = veh.WrnPatt;
            int id = _vehicle.GetElsId();
            VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangePrmPatt, id, Game.Player.ServerId);
            VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangeSecPatt, id, Game.Player.ServerId);
            VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangeWrnPatt, id, Game.Player.ServerId);
            switch (veh.Siren)
            {
                case "WL":
                    _siren._mainSiren.setMainTone(0);
                    break;
                case "YP":
                    _siren._mainSiren.setMainTone(1);
                    break;
                case "A1":
                    _siren._mainSiren.setMainTone(2);
                    break;
                case "A2":
                    _siren._mainSiren.setMainTone(3);
                    break;
            }
            VehicleManager.SyncRequestReply(RemoteEventManager.Commands.MainSiren, id, Game.Player.ServerId);
        }

        internal void GetSaveSettings()
        {
            UserSettings.ELSUserVehicle veh = new UserSettings.ELSUserVehicle()
            {
                ServerId = ELS.ServerId,
                PrmPatt = _light.CurrentPrmPattern,
                SecPatt = _light.CurrentSecPattern,
                WrnPatt = _light.CurrentWrnPattern,
                Siren = _siren._mainSiren.MainTones[_siren._mainSiren.currentTone].Type,
                Model = _vehicle.Model.Hash
            };
            ELS.userSettings.SaveVehicles(veh);
        }

        internal void SetOutofVeh()
        {
            int id = _vehicle.GetElsId();
            if (_vcf.PRML.ForcedPatterns.OutOfVeh.Enabled)
            {
                _light.CurrentPrmPattern = _vcf.PRML.ForcedPatterns.OutOfVeh.IntPattern;
                VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangePrmPatt, id, Game.Player.ServerId);
            }
            if (_vcf.SECL.ForcedPatterns.OutOfVeh.Enabled)
            {
                _light.CurrentSecPattern = _vcf.SECL.ForcedPatterns.OutOfVeh.IntPattern;
                VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangeSecPatt, id, Game.Player.ServerId);
            }
            if (_vcf.WRNL.ForcedPatterns.OutOfVeh.Enabled)
            {
                _light.CurrentWrnPattern = _vcf.WRNL.ForcedPatterns.OutOfVeh.IntPattern;
                VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangeWrnPatt, id, Game.Player.ServerId);
            }
        }

        internal void SetInofVeh()
        {
            int id = _vehicle.GetElsId();
            if (_vcf.PRML.ForcedPatterns.OutOfVeh.Enabled)
            {
                _light.CurrentPrmPattern = _light._oldprm;
                VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangePrmPatt, id, Game.Player.ServerId);
            }
            if (_vcf.SECL.ForcedPatterns.OutOfVeh.Enabled)
            {
                _light.CurrentSecPattern = _light._oldsec;
                VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangeSecPatt, id, Game.Player.ServerId);
            }
            if (_vcf.WRNL.ForcedPatterns.OutOfVeh.Enabled)
            {
                _light.CurrentWrnPattern = _light._oldwrn;
                VehicleManager.SyncRequestReply(RemoteEventManager.Commands.ChangeWrnPatt, id, Game.Player.ServerId);
            }
        }
    }
}