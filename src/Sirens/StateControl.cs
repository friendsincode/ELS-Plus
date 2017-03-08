using System;
using System.Runtime.InteropServices.WindowsRuntime;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace ELS.Sirens
{
    public class StateControl
    {
        private ELS _els;

        public StateControl(ELS els)
        {
            _els = els;
            els.EventHandlerDictionary["sirenStateChanged"]+=new Action<int,string,string,bool>(delegate(int vehid, string netSoundid, string propertyName, bool state)
            {
                Debug.WriteLine(vehid.ToString());
                if(!Function.Call<bool>(Hash.NETWORK_DOES_NETWORK_ID_EXIST,vehid))return;
                var vehId = Function.Call<Entity>(Hash.NETWORK_GET_ENTITY_FROM_NETWORK_ID, vehid);
                if (Function.Call<bool>(Hash.DECOR_EXIST_ON, vehId, propertyName))
                {
                    
                }
                else
                {
                    
                }
                Screen.ShowNotification(netSoundid.ToString()+ propertyName.ToString()+state.ToString());
            });
        }

        public void sendState(Vehicle vehicle,String propertyName,bool state)
        {
            var netId = Function.Call<int>(Hash.GET_NEAREST_PLAYER_TO_ENTITY,(InputArgument)vehicle);
            BaseScript.TriggerServerEvent("sirenStateChanged",netId,propertyName,state);
        }
    }
}