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

namespace ELS
{
    public class ELS : BaseScript
    {
        private readonly FileLoader _FileLoader;
        private SpotLight _spotLight;
        private readonly VehicleManager _vehicleManager;
        private configuration.ControlConfiguration _controlConfiguration;
        panel.test _test = new test();

        public ELS()
        {
            _controlConfiguration = new configuration.ControlConfiguration();
            _FileLoader = new FileLoader(this);
            _vehicleManager = new VehicleManager();
            EventHandlers["onClientResourceStart"] += new Action<string>(async (string obj) =>
                {
                    if (obj == Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME))
                    {
                        await Delay(500);
                        try
                        {
                            _FileLoader.RunLoader(obj);
                            //TODO: make a load files from all resouces.
                            Screen.ShowNotification($"Welcome {LocalPlayer.Name}\n ELS FiveM\n\n ELS FiveM is Licensed under LGPL 3.0\n\nMore inforomation can be found at http://fivem-scripts.net");
                            EventHandlers["ELS:NewFullSyncData"] += new Action<string, IDictionary<string, object>, int>(_vehicleManager.SetSyncVehicle);
                            Tick += Class1_Tick;
                        }
                        catch (Exception e)
                        {
                            TriggerServerEvent($"ONDEBUG", e.ToString());
                            Screen.ShowNotification($"ERROR:{e.Message}");
                            Tick -= Class1_Tick;
                            throw;
                        }
                    }
                    else
                    {
                        _FileLoader.RunLoader(obj);
                    }

                    //_spotLight= new SpotLight();
                });
            EventHandlers["onClientResourceStop"] += new Action<string>(async (string obj) =>
            {
                if (obj == Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME))
                {
                    _vehicleManager.CleanUP();
                    _FileLoader.UnLoadFilesFromScript(obj);
                }
            });

            EventHandlers["ELS:SirenUpdated"] += new Action<string, int, int, bool>(_vehicleManager.UpdateRemoteSirens);

            EventHandlers["onPlayerJoining"] += new Action(() =>
            {

            });
        }
        ~ELS()
        {
            CitizenFX.Core.Debug.WriteLine("ELS dcstor ran");
        }
        public static string CurrentResourceName()
        {
            return Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME);
        }

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

                    // _sirenManager.FullSync();
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

