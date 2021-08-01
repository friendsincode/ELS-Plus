/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System.Collections.Generic;

namespace ELS.configuration
{
    internal class Global
    {
        internal static bool EnabeTrafficControl { get; set; }
        internal static int PrimDelay { get; set; }
        internal static float EnvLightRng { get; set; }
        internal static float DeleteInterval { get; set; }
        internal static float EnvLightInt { get; set; }
        internal static float TkdnInt { get; set; }
        internal static float TkdnRng { get; set; }
        internal static float DayLtBrightness { get; set; }
        internal static float NightLtBrightness { get; set; }
        internal static float SoundVolume { get; set; }
        internal static bool AllowController { get; set; }
        internal static bool DisableSirenOnExit { get; set; }
        internal static bool ResetTakedownSpotlight { get; set; }
        internal static bool BtnClicksBtwnHrnTones { get; set; }
        internal static bool BtnClicksBtwnSrnTones { get; set; }
        internal static bool BtnClicksIndicators { get; set; }
        internal static List<string> RegisterdSoundBanks { get; set; }

        public Global()
        {
            
        }
    }
}
