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
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ELS_Server
{
    internal class Configuration
    {

        internal static bool ElsCarAdminOnly
        {
            get; private set;
        }

        internal static List<string> ElsVehicleGroups
        {
            get; set;
        }

        static Configuration()
        {
            var data = API.LoadResourceFile(API.GetCurrentResourceName(), "ELS.ini");
            if (isValidData(data))
            {
                SharpConfig.Configuration u = SharpConfig.Configuration.LoadFromString(data);
                ElsCarAdminOnly = u["ADMIN"]["ElsCarAdminOnly"].BoolValue;
                ElsVehicleGroups = new List<string>();
                foreach(string s in u["ADMIN"]["Groups"].StringValueArray)
                {
                    ElsVehicleGroups.Add(s);
                }
            }
        }



        internal static bool isValidData(string data)
        {
            return SharpConfig.Configuration.LoadFromString(data).Contains("ADMIN", "VcfContainerFolder");
        }


    }
}