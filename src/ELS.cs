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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.Siren;
using Control = CitizenFX.Core.Control;
using CitizenFX.Core.Native;
using System;
using System.Drawing;
using System.Reflection;
using System.Security.Permissions;
using ELS.configuration;
using ELS.Light;
using ELS.panel;

namespace ELS
{
    public class ELS : BaseScript
    {
        // public static int localplayerid;
        public static bool isStopped;
        private SirenManager _sirenManager;
        private FileLoader _FileLoader;
        private SpotLight _spotLight;
        private configuration.ControlConfiguration controlConfiguration;
        panel.test test = new test();

        public ELS()
        {
            controlConfiguration = new configuration.ControlConfiguration();
            _FileLoader = new FileLoader(this);
            _sirenManager = new SirenManager();

            EventHandlers["onClientResourceStart"] += new Action<string>(
                (string obj) =>
                {
                    try
                    {
                        if (obj != CurrentResourceName())
                        {
                            _FileLoader.RunLoadeer(obj);
                        }
                        else if (obj == CurrentResourceName())
                        {
                            _FileLoader.RunLoadeer(obj);
                            Tick += Class1_Tick;
                        }
                    }
                    catch (Exception e)
                    {
                        TriggerServerEvent($"ONDEBUG", e.ToString());
                        Screen.ShowNotification($"ERROR:{e}");
                        Tick -= Class1_Tick;
                        throw;
                    }

                    //_spotLight= new SpotLight();
                });
            EventHandlers["ELS:SirenUpdated"] += new Action<int, string, bool>(_sirenManager.UpdateSirens);

            EventHandlers["onPlayerJoining"] += new Action(() =>
              {

              });
        }

        public static string CurrentResourceName()
        {
            return Function.Call<string>(Hash.GET_CURRENT_RESOURCE_NAME);
        }

        private async Task Class1_Tick()
        {
            try
            {
                test.draw();
                /* Text text = new Text($"ELS Build dev-v0.0.2.4\nhttp://ELS.ejb1123.tk", new PointF(640f, 10f), 0.5f);
                 text.Alignment = Alignment.Center;
                 text.Centered = true;
                 text.Draw();*/

                if (LocalPlayer.Character.IsInVehicle() && LocalPlayer.Character.IsSittingInVehicle() &&
                    VCF.isELSVechicle(LocalPlayer.Character.CurrentVehicle.DisplayName) &&
                    LocalPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == LocalPlayer.Character)
                {
                    _sirenManager.Runtick();
                    //_spotLight.RunTick();
                }

            }
            catch (Exception ex)
            {
                TriggerServerEvent($"ONDEBUG", ex.ToString());
                await Delay(5000);
                Screen.ShowNotification($"ERROR {ex}", true);
            }
        }
    }
}

