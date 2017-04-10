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
using Debug = System.Diagnostics.Debug;

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
                        _FileLoader.RunLoadeer(obj);
                    }
                    catch (Exception e)
                    {
                        TriggerServerEvent($"ONDEBUG", e.ToString());
                        Screen.ShowNotification($"ERROR:{e}");
                        Tick -= Class1_Tick;
                        throw;
                    }

                    //_spotLight= new SpotLight();
                    if (obj == "ELS-for-FiveM") Tick += Class1_Tick;
                });
            EventHandlers["ELS:SirenUpdated"] += new Action<int, string, bool>(_sirenManager.UpdateSirens);

            EventHandlers["onPlayerJoining"] += new Action(() =>
              {

              });
        }

        private async Task Class1_Tick()
        {
            try
            {


               /* Text text = new Text($"ELS Build dev-v0.0.2.4\nhttp://ELS.ejb1123.tk", new PointF(640f, 10f), 0.5f);
                text.Alignment = Alignment.Center;
                text.Centered = true;
                text.Draw();*/

                if (LocalPlayer.Character.IsInVehicle() && LocalPlayer.Character.IsSittingInVehicle() &&
                    LocalPlayer.Character.CurrentVehicle.HasSiren &&
                    LocalPlayer.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == LocalPlayer.Character)
                {
                    //Screen.ShowNotification(LocalPlayer.Character.CurrentVehicle.DisplayName);
                    //Screen.ShowNotification(Function.Call<ulong>(Hash.GET_HASH_KEY,LocalPlayer.Character.CurrentVehicle.DisplayName).ToString());
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

