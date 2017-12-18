using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.NUI
{
    public class ELSVehicleUIState
    {

        public bool PrimarySiren
        {
            get;
            set;
        }

        public bool SecondarySiren
        {
            get;
            set;
        }

        public bool WarningSiren
        {
            get;
            set;
        }

        public bool TakedownLights
        {
            get;
            set;
        }

        public bool CRSLights
        {
            get;
            set;
        }

        public int MainSirenNumber
        {
            get;
            set;
        }

        public int MainSirenPattern
        {
            get;
            set;
        }

        public string MainSirenMode
        {
            get;
            set;
        }

        public ELSVehicleUIState(bool priS = false,bool secS = false, bool wrnS = false, bool tkdL = false, bool crsL = false, int mainSNum = 0, int mainSirenPattern = 255, string mainSirenMode = "SCAN") {
            PrimarySiren = priS;
            SecondarySiren = secS;
            WarningSiren = wrnS;
            TakedownLights = tkdL;
            CRSLights = crsL;
            MainSirenNumber = mainSNum;
            MainSirenPattern = mainSirenPattern;
            MainSirenMode = mainSirenMode;
        }
    }
}
