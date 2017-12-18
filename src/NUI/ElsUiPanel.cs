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
    internal class ElsUiPanel : BaseScript
    {
        public int _enabled = 0;


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

        //Send UI Light Data to control light functions
        public void SendLightData(bool state, string light, string color)
        { 
            CitizenFX.Core.Debug.WriteLine("Sending Light Data");
            API.SendNuiMessage("{\"type\":\"lightControl\", \"state\":" + state.ToString().ToLower() + ", \"light\": \"" + light + "\", \"color\":\"" + color + "\" }");
        }

        //Convert Dec number to Binary Pattern and execute on specified light in UI 
        public async Task RunNuiLightPattern(uint patt, string light, string color) 
        {            
            char[] binary = Convert.ToString(patt, 2).ToCharArray();
            foreach(char c in binary)
            {
                if (c.Equals('0'))
                {
                    SendLightData(false, light, "");
                }
                else
                {
                    SendLightData(true, light, color);
                }
                await Delay(100);
            }
            SendLightData(false, light, "");
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


        #region Callbacks for GUI
        public void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            CitizenFX.Core.Debug.WriteLine("Registering NUI EventHandlers");
            API.RegisterNuiCallbackType(msg); // Remember API calls must be executed on the first tick at the earliest!
            

            EventHandlers[$"__cfx_nui:togglePrimary"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                Console.WriteLine("TogPri pressed state is " + body);
                callback.Invoke(body, resultCallback);
            });

            EventHandlers[$"__cfx_nui:escape"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                //Console.WriteLine("TogPri pressed state is " + body);
                DisableUI();
                _enabled = 0;
                callback.Invoke(body, resultCallback);
            });

        }
        #endregion
    }
}
