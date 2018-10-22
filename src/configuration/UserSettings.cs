using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CitizenFX.Core.Native;

namespace ELS.configuration
{


    internal class UserSettings
    {

        internal struct ELSUserVehicle
        {
            int PrmPatt;
            int SecPatt;
            int WrnPatt;
            string Siren;
        }

        internal enum ElsUi
        {
            NewHotness,
            OldandBusted,
            Whelen
        }

        static Dictionary<string, ElsUi> ELSUiDic = new Dictionary<string, ElsUi>()
        {
            {"NewHotness", ElsUi.NewHotness },
            {"OldandBusted", ElsUi.OldandBusted },
            {"Whelen", ElsUi.Whelen }
        };

        internal static Dictionary<string, ELSUserVehicle> savedVehicles = new Dictionary<string, ELSUserVehicle>();
        internal static ElsUi currentUI = ElsUi.NewHotness;

        internal static async Task LoadUserSettings()
        {
           savedVehicles = JsonConvert.DeserializeObject<Dictionary<string,ELSUserVehicle>>(API.GetResourceKvpString("SavedVehicles"));
            currentUI = ELSUiDic[API.GetResourceKvpString("ELSUI")];            
        }

        internal static async Task SaveVehicles()
        {
            API.SetResourceKvp("SavedVehicles", JsonConvert.SerializeObject(savedVehicles));
        }

        internal static async Task SaveUI()
        {
            API.SetResourceKvp("ELSUI", nameof(currentUI));
        }
    }
}
