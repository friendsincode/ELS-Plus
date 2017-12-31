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
    enum ExtraEnum
    {
        PRML,
        WRNL,
        SECL
    }
    internal struct Extras
    {
        public Dictionary<int, Extra.Extra> PRML;
        internal Dictionary<int, Extra.Extra> WRNL;
        internal Dictionary<int, Extra.Extra> SECL;
    }
    internal class Lights : IManagerEntry
    {
        private Extras _extras = new Extras
        {
            PRML = new Dictionary<int, Extra.Extra>(),
            WRNL = new Dictionary<int, Extra.Extra>(),
            SECL = new Dictionary<int, Extra.Extra>()
            
        };
        private bool enabled = false;
        private Vcfroot _vcfroot;
        private Vehicle _vehicle;
        private LightPatternSettings lightPatternSettings;
        internal Lights(Vehicle vehicle, Vcfroot vcfroot, [Optional]IDictionary<string, object> data)
        {
            _vcfroot = vcfroot;
            _vehicle = vehicle;
            lightPatternSettings = new LightPatternSettings();
            AddAllValidLightExtras();
        }

        private void AddAllValidLightExtras()
        {
            for (int x = 0; x < 12; x++)
            {
                switch (x)
                {
                    case 1:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 1) && bool.Parse(_vcfroot.EOVERRIDE.Extra01.IsElsControlled))
                            {
                                this._extras.PRML.Add(1, new Extra.Extra(_vehicle,1));
                            }
                        }
                        break;
                    case 2:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 2) && bool.Parse(_vcfroot.EOVERRIDE.Extra02.IsElsControlled))
                            {
                                this._extras.PRML.Add(2, new Extra.Extra(_vehicle,2));
                            }
                        }
                        break;
                    case 3:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 3) && bool.Parse(_vcfroot.EOVERRIDE.Extra03.IsElsControlled))
                            {
                                this._extras.PRML.Add(3, new Extra.Extra(_vehicle,3));
                            }
                        }
                        break;
                    case 4:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 4) && bool.Parse(_vcfroot.EOVERRIDE.Extra04.IsElsControlled))
                            {
                                this._extras.SECL.Add(4, new Extra.Extra(_vehicle,4));
                            }
                        }
                        break;
                    case 5:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 5) && bool.Parse(_vcfroot.EOVERRIDE.Extra05.IsElsControlled))
                            {
                                this._extras.SECL.Add(5, new Extra.Extra(_vehicle,5));
                            }
                        }
                        break;
                    case 6:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 6) && bool.Parse(_vcfroot.EOVERRIDE.Extra06.IsElsControlled))
                            {
                                this._extras.SECL.Add(6, new Extra.Extra(_vehicle,6));
                            }
                        }
                        break;
                    case 7:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 7) && bool.Parse(_vcfroot.EOVERRIDE.Extra07.IsElsControlled))
                            {
                                this._extras.SECL.Add(7, new Extra.Extra(_vehicle,7));
                            }
                        }
                        break;
                    case 8:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 8) && bool.Parse(_vcfroot.EOVERRIDE.Extra08.IsElsControlled))
                            {
                                this._extras.SECL.Add(8, new Extra.Extra(_vehicle,8));
                            }
                        }
                        break;
                    case 9:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 9) && bool.Parse(_vcfroot.EOVERRIDE.Extra09.IsElsControlled))
                            {
                                this._extras.SECL.Add(9, new Extra.Extra(_vehicle,9));
                            }
                        }
                        break;
                    case 10:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 10) && bool.Parse(_vcfroot.EOVERRIDE.Extra10.IsElsControlled))
                            {
                                this._extras.SECL.Add(10, new Extra.Extra(_vehicle,10));
                            }
                        }
                        break;
                    case 11:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 11) && bool.Parse(_vcfroot.EOVERRIDE.Extra11.IsElsControlled))
                            {
                                this._extras.SECL.Add(11, new Extra.Extra(_vehicle,11));
                            }
                        }
                        break;
                    case 12:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 12) && bool.Parse(_vcfroot.EOVERRIDE.Extra12.IsElsControlled))
                            {
                                this._extras.SECL.Add(12, new Extra.Extra(_vehicle,12));
                            }
                        }
                        break;
                }
                //add PRML
                //10 steadty burn
                //11 scl scene lights
                //12 takedown
            }
        }


        Vehicle IManagerEntry._vehicle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void CleanUP()
        {
            //throw new NotImplementedException();
        }

        public async void ExternalTicker()
        {
            while (this.enabled)
            {
                LightPattern.RunLightPattern(_vehicle, LightPattern.StringPatterns, null, 100);
            }
            
        }

        public void SirenControlsRemote(string sirenString, bool state)
        {
            //throw new NotImplementedException();
        }

        public void Ticker()
        {

            if (Game.IsControlJustPressed(0, ControlConfiguration.KeyBindings.Sound_Ahorn))
            {
                
            }
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
