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
using ELS.panel;
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
        private ElsUiPanel _eLSUiPanel;
        private readonly VehicleManager _vehicleManager;
        private configuration.ControlConfiguration _controlConfiguration;
        panel.test _test = new test();

        public ELS()
        {
            bool Loaded = false;
            _controlConfiguration = new configuration.ControlConfiguration();
            _FileLoader = new FileLoader(this);
            _vehicleManager = new VehicleManager();
            _eLSUiPanel = new ElsUiPanel();
            EventHandlers["onClientResourceStart"] += new Action<string>((string obj) =>
                {
                    //TODO rewrite loader so that it 
                    if (obj == Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME))
                    {
                        //await Delay(500);
                        try
                        {
                            //_FileLoader.RunLoader(obj);
                            //TODO: make a load files from all resouces.
                            Screen.ShowNotification($"Welcome {LocalPlayer.Name}\n ELS FiveM\n\n ELS FiveM is Licensed under LGPL 3.0\n\nMore inforomation can be found at http://fivem-scripts.net");
                            SetupConnections();
                            TriggerServerEvent("ELS:VcfSync:Server", Game.Player.ServerId);
                            TriggerServerEvent("ELS:FullSync:Request:All", Game.Player.ServerId);
                            _eLSUiPanel.DisableUI();
                        }
                        catch (Exception e)
                        {
                            TriggerServerEvent($"ONDEBUG", e.ToString());
                            Screen.ShowNotification($"ERROR:{e.Message}");
                            Screen.ShowNotification($"ERROR:{e.StackTrace}");
                            Tick -= Class1_Tick;
                            throw;
                        }
                    }
                    else
                    {
                        try
                        {
                            //_FileLoader.RunLoader(obj);
                        }
                        catch (Exception e)
                        {
                            TriggerServerEvent($"ONDEBUG", e.ToString());
                            Screen.ShowNotification($"ERROR:{e.Message}");
                            Screen.ShowNotification($"ERROR:{e.StackTrace}");

                        }
                    }

                    //_spotLight= new SpotLight();
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
            EventHandlers["onPlayerJoining"] += new Action(() =>
            {

            });
            EventHandlers["ELS:VcfSync:Client"] += new Action<string,string,string>((a,b,c) =>
            {
                VCF.load(SettingsType.Type.VCF, b, c, a);
            });
            EventHandlers["ELS:FullSync:NewSpawnWithData"] += new Action<System.Dynamic.ExpandoObject>((a) =>
            {
                _vehicleManager.SyncAllVehiclesOnFirstSpawn(a);
                Tick -= Class1_Tick;
                Tick += Class1_Tick;
            });
            EventHandlers["ELS:FullSync:NewSpawn"] += new Action(() =>
            {
                Tick -= Class1_Tick;
                Tick += Class1_Tick;
            });
            //Take in data and apply it
            EventHandlers["ELS:NewFullSyncData"] += new Action<IDictionary<string, object>>(_vehicleManager.SetVehicleSyncData);
            RegisterNUICallback("escape", _eLSUiPanel.EscapeUI);
            RegisterNUICallback("togglePrimary", _eLSUiPanel.TooglePrimary);
        }

        public static string CurrentResourceName()
        {
            return Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME);
        }

        #region Callbacks for GUI
        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            CitizenFX.Core.Debug.WriteLine($"Registering NUI EventHandler for {msg}");
            API.RegisterNuiCallbackType(msg); // Remember API calls must be executed on the first tick at the earliest!


            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                Console.WriteLine("TogPri pressed state is " + body);
                callback.Invoke(body, resultCallback);
            });

        }
        #endregion

        private async Task Class1_Tick()
        {
            try
            {
                /* Text text = new Text($"ELS Build dev-v0.0.2.4\nhttp://ELS.ejb1123.tk", new PointF(640f, 10f), 0.5f);
                 text.Alignment = Alignment.Center;
                 text.Centered = true;
                 text.Draw();*/
                //_sirenManager.Runtick();
                //_spotLight.RunTick();
                _vehicleManager.RunTick();
                if (Game.IsControlJustReleased(0, Control.MultiplayerInfo))
                {
                    await Debug.Spawn();
                }

                if (Game.PlayerPed.IsInPoliceVehicle && _eLSUiPanel._enabled == 0)
                {
                    _eLSUiPanel.ShowUI();
                }
                else if (!Game.PlayerPed.IsInPoliceVehicle && _eLSUiPanel._enabled == 1)
                {
                    _eLSUiPanel.DisableUI();
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

