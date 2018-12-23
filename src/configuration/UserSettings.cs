using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CitizenFX.Core.Native;
using ELS.NUI;

namespace ELS.configuration
{


    internal class UserSettings
    {

        internal struct ELSUserVehicle
        {
            internal string VehicleName;
            internal string ServerId;
            internal int PrmPatt;
            internal int SecPatt;
            internal int WrnPatt;
            internal string Siren;
        }

        internal enum ElsUi
        {
            NewHotness,
            OldandBusted,
            Whelen
        }

        internal struct UISettings
        {
            internal bool enabled;
            internal ElsUi currentUI;
        }

        static Dictionary<string, ElsUi> ELSUiDic = new Dictionary<string, ElsUi>()
        {
            {"NewHotness", ElsUi.NewHotness },
            {"OldandBusted", ElsUi.OldandBusted },
            {"Whelen", ElsUi.Whelen }
        };

        internal static List<ELSUserVehicle> savedVehicles = new List<ELSUserVehicle>();
        internal static UISettings uiSettings = new UISettings();

        internal static async Task LoadUserSettings()
        {
            string vehs = API.GetResourceKvpString("SavedVehicles");
            string uiSet = API.GetResourceKvpString("ElsUiSettings");
            if (!String.IsNullOrEmpty(vehs))
            {
                savedVehicles = JsonConvert.DeserializeObject<List<ELSUserVehicle>>(vehs);
            }
            if (!String.IsNullOrEmpty(uiSet))
            {
                uiSettings = JsonConvert.DeserializeObject<UISettings>(uiSet);
                ElsUiPanel.ReloadUI();
            }
        }

        internal static async Task SaveVehicles()
        {
            API.SetResourceKvp("SavedVehicles", JsonConvert.SerializeObject(savedVehicles));
        }

        internal static async Task SaveUI()
        {
            API.SetResourceKvp("ElsUiSettings", JsonConvert.SerializeObject(uiSettings));
            ElsUiPanel.ReloadUI();
        }
    }
}
