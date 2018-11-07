/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using Control = CitizenFX.Core.Control;
using CitizenFX.Core.Native;
using System;
using ELS.Light;
using System.Collections.Generic;
using ELS.Manager;
using ELS.NUI;
using System.Dynamic;
using ELS.configuration;

namespace ELS
{
    public class ELS : BaseScript
    {
        private readonly FileLoader _FileLoader;
        private SpotLight _spotLight;
        private readonly VehicleManager _vehicleManager;
        private ElsConfiguration _controlConfiguration;
        private bool _firstTick = false;
        internal static string ServerId;
        

        public ELS()
        {
            bool Loaded = false;
            ServerId = API.GetConvar("ElsServerId",null);
            _controlConfiguration = new ElsConfiguration();
            _FileLoader = new FileLoader(this);
            _vehicleManager = new VehicleManager();
            EventHandlers["onClientResourceStart"] += new Action<string>((string obj) =>
                {
                    //TODO rewrite loader so that it 
                    if (obj == CurrentResourceName())
                    {
                        //await Delay(500);
                        try
                        {
                            Task us = UserSettings.LoadUserSettings();
                            us.Start();
                            _FileLoader.RunLoader(obj);
                            Screen.ShowNotification($"Welcome {LocalPlayer.Name}\n ELS Plus\n\n ELS Plus is Licensed under LGPL 3.0\n\nMore inforomation can be found at http://els.friendsincode.com");
                            SetupConnections();
                            TriggerServerEvent("ELS:VcfSync:Server", Game.Player.ServerId);
                            TriggerServerEvent("ELS:FullSync:Request:All", Game.Player.ServerId);
                            ElsUiPanel.InitData();
                            ElsUiPanel.DisableUI();
                            SetupExports();
                        }
                        catch (Exception e)
                        {
                            TriggerServerEvent($"ONDEBUG", e.ToString());
                            Screen.ShowNotification($"ERROR:{e.Message}");
                            Screen.ShowNotification($"ERROR:{e.StackTrace}");
                            Tick -= Class1_Tick;
                            //throw;
                        }
                    }
                });

        }
        private void SetupConnections()
        {
            EventHandlers["onClientResourceStop"] += new Action<string>(async (string obj) =>
            {
                if (obj == Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME))
                {
                    _vehicleManager.CleanUP();
                    _FileLoader.UnLoadFilesFromScript(obj);
                }
            });
            //command that siren state has changed.
            //recieves a command
            //EventHandlers["ELS:SirenUpdated"] += new Action<string, int, int, bool>(_vehicleManager.UpdateRemoteSirens);
            EventHandlers["ELS:SpawnCar"] += new Action<string>((veh) =>
            {
                SpawnCar(veh);
            });
            EventHandlers["ELS:VcfSync:Client"] += new Action<List<dynamic>>((vcfs) =>
              {
                  VCF.ParseVcfs(vcfs);
              });
            EventHandlers.Add("ELS:PatternSync:Client", new Action<List<dynamic>>((patterns) =>
            {

                VCF.ParsePatterns(patterns);
            }));
            EventHandlers["ELS:FullSync:NewSpawnWithData"] += new Action<System.Dynamic.ExpandoObject>((a) =>
            {
                //_vehicleManager.SyncAllVehiclesOnFirstSpawn(a);
                Tick -= Class1_Tick;
                Tick += Class1_Tick;
            });
            EventHandlers["ELS:FullSync:NewSpawn"] += new Action(() =>
            {
                Tick -= Class1_Tick;
                Tick += Class1_Tick;
            });

            EventHandlers["ELS:VehicleEntered"] += new Action<int>((veh) =>
            {
                Utils.DebugWriteLine("Vehicle entered");
                Vehicle vehicle = new Vehicle(veh);
                if (vehicle.IsEls() && VehicleManager.vehicleList.ContainsKey(vehicle.GetNetworkId()))
                {
                    Utils.DebugWriteLine("ELS Vehicle entered syncing UI and settings");
                    if (UserSettings.savedVehicles.Count > 1)
                    {
                        UserSettings.ELSUserVehicle usrVeh = UserSettings.savedVehicles.Find(uv => (uv.VehicleName == vehicle.DisplayName && uv.ServerId == ServerId));
                    }
                    
                    
                    VehicleManager.SyncUI(vehicle.GetNetworkId());
                }
            });
            EventHandlers["ELS:VehicleExited"] += new Action<int>((veh) =>
            {
                if (Global.DisableSirenOnExit)
                {
                    Utils.DebugWriteLine("Vehicle exited");
                    Vehicle vehicle = new Vehicle(veh);
                    if (vehicle.IsEls() && VehicleManager.vehicleList.ContainsKey(vehicle.GetNetworkId()))
                    {
                        VehicleManager.vehicleList[vehicle.GetNetworkId()].DisableSiren();
                    }
                }
            });
            //Take in data and apply it
            EventHandlers["ELS:NewFullSyncData"] += new Action<IDictionary<string, object>,int>(_vehicleManager.SetVehicleSyncData);


            API.RegisterCommand("vcfsync", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                TriggerServerEvent("ELS:VcfSync:Server", Game.Player.ServerId);
            }), false);

            API.RegisterCommand("elsui", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                if (arguments.Count != 1) return;
                switch (arguments[0].ToString().ToLower())
                {
                    case "enable":
                        ElsUiPanel.EnableUI();
                        break;
                    case "disable":
                        ElsUiPanel.DisableUI();
                        break;
                    case "show":
                        ElsUiPanel.ShowUI();
                        break;
                    case "whelen":
                        UserSettings.uiSettings.currentUI = UserSettings.ElsUi.Whelen;
                        UserSettings.SaveUI();
                        break;
                    case "new":
                        UserSettings.uiSettings.currentUI = UserSettings.ElsUi.NewHotness;
                        UserSettings.SaveUI();
                        break;
                    case "old":
                        UserSettings.uiSettings.currentUI = UserSettings.ElsUi.OldandBusted;
                        UserSettings.SaveUI();
                        break;
                }
            }), false);

            API.RegisterCommand("elslist", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var listString = "";
                listString += $"Available ELS Vehicles\n" +
                              $"----------------------\n";
                foreach (var entry in VCF.ELSVehicle.Values)
                {
                    if (entry.modelHash.IsValid)
                    {
                        listString += $"{System.IO.Path.GetFileNameWithoutExtension(entry.filename)}\n";
                    }
                }
                CitizenFX.Core.Debug.WriteLine(listString);
            }), false);
        }

        internal async Task<Vehicle> SpawnCar(string veh)
        {
            Model hash = Game.GenerateHash(veh);
            if (String.IsNullOrEmpty(veh))
            {
                Screen.ShowNotification("Vehicle not found please try again");
                return null;
            }
            if (!VCF.ELSVehicle.ContainsKey(hash))
            {
                Screen.ShowNotification("Vehicle not ELS please try again");
                return null;
            }
            if (Game.PlayerPed.IsInVehicle())
            {
                if (Game.PlayerPed.CurrentVehicle.IsEls())
                {
                    if (VehicleManager.vehicleList.ContainsKey(Game.PlayerPed.CurrentVehicle.GetNetworkId()))
                    {
                        VehicleManager.vehicleList[Game.PlayerPed.CurrentVehicle.GetNetworkId()].Delete();
                    }
                    else
                    {
                        Game.PlayerPed.CurrentVehicle.Delete();
                    }

                }
                else
                {
                    Game.PlayerPed.CurrentVehicle.Delete();
                }
            }
            CitizenFX.Core.Debug.WriteLine($"Attempting to spawn: {veh}");
            var polModel = new Model((VehicleHash)hash);
            await polModel.Request(-1);
            Vehicle _veh =  new Vehicle(API.CreateVehicle((uint)polModel.Hash, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y + 5f, Game.PlayerPed.Position.Z, Game.PlayerPed.Heading, true,false));
            VehicleManager.makenetworked(_veh);
            Game.PlayerPed.SetIntoVehicle(_veh, VehicleSeat.Driver);
            polModel.MarkAsNoLongerNeeded();
            return _veh;
        }

        internal async Task<Vehicle> SpawnCar(string veh, dynamic coords)
        {
            Model hash = Game.GenerateHash(veh);
            if (String.IsNullOrEmpty(veh))
            {
                Screen.ShowNotification("Vehicle not found please try again");
                return null;
            }
            if (!VCF.ELSVehicle.ContainsKey(hash))
            {
                Screen.ShowNotification("Vehicle not ELS please try again");
                return null;
            }
            if (Game.PlayerPed.IsInVehicle())
            {
                if (Game.PlayerPed.CurrentVehicle.IsEls())
                {
                    VehicleManager.vehicleList[Game.PlayerPed.CurrentVehicle.GetNetworkId()].Delete();
                }
                else
                {
                    Game.PlayerPed.CurrentVehicle.Delete();
                }
            }
            CitizenFX.Core.Debug.WriteLine($"Attempting to spawn: {veh}");
            var polModel = new Model((VehicleHash)hash);
            await polModel.Request(-1);

            Vehicle _veh = new Vehicle(API.CreateVehicle((uint)Game.GenerateHash(veh), coords.x, coords.y + 5f, coords.z, Game.PlayerPed.Heading, true, false));
            VehicleManager.makenetworked(_veh);
            polModel.MarkAsNoLongerNeeded();
            _veh.PlaceOnGround();
            Game.PlayerPed.SetIntoVehicle(_veh, VehicleSeat.Driver);
            polModel.MarkAsNoLongerNeeded();
            return _veh;
        }

        private void SetupExports()
        {
            Exports.Add("SpawnCar", new Func<string, Task<Vehicle>>((veh) =>
            {
                return SpawnCar(veh);
            }));
            Exports.Add("SpawnCarWithCoords", new Func<string, dynamic, Task<Vehicle>>((veh,coord) =>
            {
                return SpawnCar(veh, coord);
            }));
        }

        public static string CurrentResourceName()
        {
            return Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME);
        }

        #region Callbacks for GUI
        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            CitizenFX.Core.Debug.WriteLine($"Registering NUI EventHandler for {msg}");
            API.RegisterNuiCallbackType(msg);
            // Remember API calls must be executed on the first tick at the earliest!
            //Function.Call(Hash.REGISTER_NUI_CALLBACK_TYPE, msg);
            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                Console.WriteLine("We has event" + body);
                callback.Invoke(body, resultCallback);
            });
            EventHandlers[$"{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                Console.WriteLine("We has event without __cfx_nui" + body);
                callback.Invoke(body, resultCallback);
            });

        }
        #endregion

        private async Task Class1_Tick()
        {
            try
            {
                if (!_firstTick)
                {
                    RegisterNUICallback("escape", ElsUiPanel.EscapeUI);
                    RegisterNUICallback("togglePrimary", ElsUiPanel.TooglePrimary);
                    RegisterNUICallback("keyPress", ElsUiPanel.KeyPress);
                    _firstTick = true;
                }
                _vehicleManager.RunTickAsync();

                if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.IsEls())
                {
                    Game.DisableControlThisFrame(0, Control.FrontendPause);
                    if (Game.IsControlPressed(0, Control.VehicleMoveUpDown) && Game.IsDisabledControlJustReleased(0, Control.FrontendPause))
                    {

                        if (ElsUiPanel._enabled == 0)
                        {
                            ElsUiPanel.ShowUI();
                        }
                        else if (ElsUiPanel._enabled == 1)
                        {
                            ElsUiPanel.DisableUI();
                        }
                    }
                }
                else
                {
                    if (ElsUiPanel._enabled != 0)
                    {
                        ElsUiPanel.DisableUI();
                    }
                }
            }
            catch (Exception ex)
            {
                //TriggerServerEvent($"ONDEBUG", ex.ToString());
                //await Delay(5000);
                Screen.ShowNotification($"ERROR {ex}", true);
                //throw ex;
            }
        }
    }
}

