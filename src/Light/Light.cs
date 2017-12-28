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
using ELS.configuration;
using ELS.Siren;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    class Light : IManagerEntry
    {
        private int _extraId;
        private Vehicle _vehicle;
        internal Light(Vehicle vehicle, Vcfroot vcfroot,[Optional]IDictionary<string, object> data)
        {
            _vehicle = vehicle;
        }

        Vehicle IManagerEntry._vehicle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void CleanUP()
        {
            //throw new NotImplementedException();
        }

        public void ExternalTicker()
        {
            //throw new NotImplementedException();
        }

        public void SirenControlsRemote(string sirenString, bool state)
        {
            //throw new NotImplementedException();
        }

        public void Ticker()
        {
            //throw new NotImplementedException();
        }

        internal object GetData()
        {
            return null;
            //throw new NotImplementedException();
        }

        internal void SetData(IDictionary<string, object> dictionary)
        {
            //throw new NotImplementedException();
        }
    }
}
