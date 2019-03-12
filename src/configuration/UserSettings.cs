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
        internal static UISettings uiSettings = new UISettings()
        {
            currentUI = ElsUi.NewHotness,
            enabled = false
        };

        internal static async void LoadUserSettings()
        {
            string vehs = API.GetResourceKvpString("SavedVehicles");
            string uiSet = API.GetResourceKvpString("ElsUiSettings");
            Utils.DebugWriteLine($"got ui settings as: {uiSet}");
            Utils.DebugWriteLine($"got vehicle settings as: {vehs}");
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

        internal static async void SaveVehicles(ELSUserVehicle veh)
        {
            int usrVeh = UserSettings.savedVehicles.FindIndex(uveh => uveh.VehicleName == veh.VehicleName && veh.ServerId == uveh.ServerId);
            if (usrVeh != -1)
            {
                savedVehicles[usrVeh] = veh;
            }
            else
            {
                savedVehicles.Add(veh);
            }
            API.SetResourceKvp("SavedVehicles", JsonConvert.SerializeObject(savedVehicles).ToString());
            Utils.DebugWriteLine($"Vehicle Settings Saved: {API.GetResourceKvpString("SavedVehicles")}");
        }

        internal static async void SaveUI(bool reload)
        {
            API.SetResourceKvp("ElsUiSettings", JsonConvert.SerializeObject(uiSettings).ToString());
            Utils.DebugWriteLine($"Ui Settings Saved: {API.GetResourceKvpString("ElsUiSettings")}");
            if (reload)
            {
                ElsUiPanel.ReloadUI();
            }
        }
    }
}
