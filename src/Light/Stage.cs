using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELS.configuration;
using ELS.NUI;
using CitizenFX.Core;

namespace ELS.Light
{
    internal class Stage
    {
        internal configuration.Lights PRML;
        internal configuration.Lights SECL;
        internal configuration.Lights WRNL;
        private int vehicleId;
        private string ActivationType = "manual";
        private int stage = 0;

        internal Stage(configuration.Lights prml, configuration.Lights secl, configuration.Lights wrnl, int veh, string acttype)
        {
            PRML = prml;
            SECL = secl;
            WRNL = wrnl;
            CurrentStage = 0;
            vehicleId = veh;
            ActivationType = acttype.ToLower();
            Utils.DebugWriteLine($"Light Stage activation type is {ActivationType}");
        }

        internal int CurrentStage
        {
            get { return stage; }
            private set
            {
                stage = value;
                if (Game.PlayerPed.IsSittingInELSVehicle() && vehicleId == Game.PlayerPed.CurrentVehicle.GetNetworkId())
                {
                    ElsUiPanel.ToggleStages(CurrentStage);
                }
            }
        }

        internal async Task NextStage(bool inverse)
        {
            if (inverse)
            {
                switch (ActivationType)
                {
                    case "manual":
                        InvertStage();
                        break;
                    case "invert":
                        ManualStage();
                        break;
                    case "auto":
                        InvertStage();
                        break;
                    case "euro":
                        InvertStage();
                        break;
                    default:
                        ManualStage();
                        break;
                }
            }
            else
            {
                switch (ActivationType)
                {
                    case "manual":
                        ManualStage();
                        break;
                    case "invert":
                        InvertStage();
                        break;
                    case "auto":
                        AutoEuroStage();
                        break;
                    case "euro":
                        AutoEuroStage();
                        break;
                    default:
                        ManualStage();
                        break;
                }
            }
        }

        internal void ManualStage()
        {
            if (CurrentStage.Equals(3))
            {
                SetStage(0);
                return;
            }
            SetStage(CurrentStage + 1);
        }

        internal void AutoEuroStage()
        {
            if (CurrentStage.Equals(0))
            {
                SetStage(3);
                return;
            }
            if (CurrentStage.Equals(3))
            {
                SetStage(0);
                return;
            }

        }

        internal void InvertStage()
        {
            if (CurrentStage.Equals(0))
            {
                SetStage(3);
                return;
            }
            SetStage(CurrentStage - 1);
        }

        internal void SetStage(int stage)
        {
            CurrentStage = stage;
        }

        internal int[] GetStage2Extras()
        {
            int[] arr = { 1, 4 };
            if (!String.IsNullOrEmpty(PRML.ExtrasActiveAtLstg2))
            {
                switch (PRML.ExtrasActiveAtLstg2)
                {
                    case "1and4":
                        arr[0] = 1;
                        arr[1] = 4;
                        break;
                    case "2and3":
                        arr[0] = 2;
                        arr[1] = 3;
                        break;
                    case "1and3":
                        arr[0] = 1;
                        arr[1] = 3;
                        break;
                    case "2and4":
                        arr[0] = 2;
                        arr[1] = 4;
                        break;
                    case "1and2":
                        arr[0] = 1;
                        arr[1] = 2;
                        break;
                    case "3and4":
                        arr[0] = 3;
                        arr[1] = 4;
                        break;
                }

            }
            return arr;
        }
    }
}
