using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Dynamic;
using CitizenFX.Core.Native;
using System.Threading;
using System.IO;

namespace ELS.NUI
{
    internal static class ElsUiPanel
    {
        internal static int _enabled { get; set; }
        internal static bool _runPattern { get; set; }

        //Sent current resource name to the ui
        internal static void InitData()
        {
            string name = API.GetCurrentResourceName();
            CitizenFX.Core.Debug.WriteLine($"Sending Current resouce name {name}");
            API.SendNuiMessage($"{{\"type\":\"initdata\", \"name\":\"{name}\"}}");
        }

        //Enable full ui control and cursor
        internal static void EnableUI()
        {
            CitizenFX.Core.Debug.WriteLine("Enabling UI");
            API.SendNuiMessage("{\"type\":\"enableui\", \"enable\":true}");
            API.SetNuiFocus(true, true);
            _enabled = 2;
        }


        //Disable the UI and cursor
        internal static void DisableUI()
        {
            CitizenFX.Core.Debug.WriteLine("Disabling Ui");
            API.SendNuiMessage("{\"type\":\"enableui\", \"enable\":false}");
            API.SetNuiFocus(false, false);
            _enabled = 0;
        }


        //Show only the UI without focus and cursor
        internal static void ShowUI()
        {
            CitizenFX.Core.Debug.WriteLine("Showing Ui");
            API.SendNuiMessage("{\"type\":\"enableui\", \"enable\":true}");
            API.SetNuiFocus(false, false);
            _enabled = 1;
        }

        /// <summary>
        /// Send lighting data to display 
        /// </summary>
        /// <param name="state">True or false if light is on</param>
        /// <param name="light">Corresponding light on NUI display</param>
        /// <param name="color">Color of light</param>
        internal static void SendLightData(bool state, string light, string color)
        {
            //CitizenFX.Core.Debug.WriteLine("Sending Light Data");
            API.SendNuiMessage("{\"type\":\"lightControl\", \"state\":" + state.ToString().ToLower() + ", \"light\": \"" + light + "\", \"color\":\"" + color + "\" }");
        }

        internal static void SetUiPatternNumber(int patt, string lighttype)
        {
            API.SendNuiMessage($"{{\"type\":\"setpatternnumber\", \"lighttype\":\"{lighttype}\", \"pattern\":\"{patt.ToString().PadLeft(4,'0')}\" }}");
        }

        internal static void ToggleUiBtnState(bool state, string which)
        {
            CitizenFX.Core.Debug.WriteLine($"Setting {which} to {state}");
            API.SendNuiMessage($"{{\"type\":\"togglestate\", \"which\":\"{which}\", \"state\":{state.ToString().ToLower()} }}");
        }

        static ElsUiPanel()
        {
            _enabled = 0;
            _runPattern = false;
        }

        internal static CallbackDelegate EscapeUI(IDictionary<string,Object> data, CallbackDelegate cb)
        {
            CitizenFX.Core.Debug.WriteLine("Escape Executed");
            ShowUI();
            return cb;
        }

        internal static CallbackDelegate TooglePrimary(IDictionary<string,Object> data, CallbackDelegate cb)
        {
            CitizenFX.Core.Debug.WriteLine("Toggle Primary Executed");
            return cb;
        }

        internal static CallbackDelegate KeyPress(IDictionary<string, Object> data, CallbackDelegate cb)
        {
            CitizenFX.Core.Debug.WriteLine("J key pressed");
            return cb;
        }



    }
}
