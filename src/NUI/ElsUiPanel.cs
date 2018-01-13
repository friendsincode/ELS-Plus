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

        /// <summary>
        /// Run a given pattern for lights on NUI
        /// </summary>
        /// <param name="patt">Dec representation of binary pattern</param>
        /// <param name="light">Corresponding light on NUI display</param>
        /// <param name="color">Color of light</param>
        /// <returns></returns>        
        internal static async Task RunNuiLightPattern(uint patt, string light, string color)
        {
            string patt2 = Convert.ToString(patt, 2);
            char[] binary = patt2.ToCharArray();
            do
            {
                foreach (char c in binary)
                {
                    if (c.Equals('0'))
                    {
                        SendLightData(false, light, "");
                    }
                    else
                    {
                        SendLightData(true, light, color);
                    }
                    await ELS.Delay(75);
                }
                SendLightData(false, light, "");
            } while (_runPattern);
        }

        /// <summary>
        /// Run a given pattern for lights on NUI
        /// </summary>
        /// <param name="patt">Binary String of light pattern</param>
        /// <param name="light">Corresponding light on NUI Display</param>
        /// <param name="color">Color of light</param>
        /// <returns></returns>
        internal static async Task RunNuiLightPattern(string patt, string light, string color)
        {
            //string patt2 = Convert.ToString(patt, 2);
            char[] binary = patt.ToCharArray();
            do
            {
                foreach (char c in binary)
                {
                    if (c.Equals('0'))
                    {
                        SendLightData(false, light, "");
                    }
                    else
                    {
                        SendLightData(true, light, color);
                    }
                    await ELS.Delay(150);
                }
                SendLightData(false, light, "");
            } while (_runPattern);
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
