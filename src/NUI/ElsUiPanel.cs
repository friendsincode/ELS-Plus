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
    internal class ElsUiPanel
    {
        public int _enabled = 0;
        public bool _runPattern = false;

        //Enable full ui control and cursor
        public void EnableUI()
        {
            CitizenFX.Core.Debug.WriteLine("Enabling UI");
            API.SendNuiMessage("{\"type\":\"enableui\", \"enable\":true}");
            API.SetNuiFocus(true, true);
            _enabled = 2;
        }


        //Disable the UI and cursor
        public void DisableUI()
        {
            CitizenFX.Core.Debug.WriteLine("Disabling Ui");
            API.SendNuiMessage("{\"type\":\"enableui\", \"enable\":false}");
            API.SetNuiFocus(false, false);
            _enabled = 0;
        }


        //Show only the UI without focus and cursor
        public void ShowUI()
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
        public void SendLightData(bool state, string light, string color)
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
        public async Task RunNuiLightPattern(uint patt, string light, string color)
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
        public async Task RunNuiLightPattern(string patt, string light, string color)
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
                    await ELS.Delay(75);
                }
                SendLightData(false, light, "");
            } while (_runPattern);
        }

        public ElsUiPanel()
        { 
        }

        public CallbackDelegate EscapeUI(IDictionary<string,Object> data, CallbackDelegate cb)
        {
            CitizenFX.Core.Debug.WriteLine("Escape Executed");
            return cb;
        }

        public CallbackDelegate TooglePrimary(IDictionary<string,Object> data, CallbackDelegate cb)
        {
            CitizenFX.Core.Debug.WriteLine("Toggle Primary Executed");
            return cb;
        }


        
    }
}
