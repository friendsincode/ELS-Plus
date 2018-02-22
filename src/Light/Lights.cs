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
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        internal Extra.Extra SBRN;
        internal Extra.Extra SCL;
        internal Extra.Extra TDL;
        internal Board.ArrowBoard BRD;
    }

    partial class Lights : IManagerEntry
    {
        private Extras _extras = new Extras
        {
            PRML = new Dictionary<int, Extra.Extra>(),
            WRNL = new Dictionary<int, Extra.Extra>(),
            SECL = new Dictionary<int, Extra.Extra>(),
        };

        private Vcfroot _vcfroot;
        private Vehicle _vehicle;
        internal Stage _stage;
       

        internal Lights(Vehicle vehicle, Vcfroot vcfroot, [Optional]IDictionary<string, object> data)
        {
            _vcfroot = vcfroot;
            _vehicle = vehicle;
            AddAllValidLightExtras();
            LightStagesSetup();
            SetupPatternsPrm();
            SetupSecPatterns();
            SetupWrnPatterns();
        }

        private void LightStagesSetup()
        {
            _stage = new Stage(_vcfroot.PRML, _vcfroot.SECL, _vcfroot.WRNL, _vehicle.GetNetworkId(), _vcfroot.INTERFACE.LstgActivationType);
        }

        internal void SyncUi()
        {
            ElsUiPanel.ToggleStages(_stage.CurrentStage);
            if (_vcfroot.INTERFACE.LstgActivationType.ToLower().Equals("euro"))
            {
                ElsUiPanel.SetEuro(true);
            }
            else
            {
                ElsUiPanel.SetEuro(false);
            }
            ElsUiPanel.SetUiDesc(_prefix + CurrentPrmPattern.ToString().PadLeft(3, '0'), ExtraEnum.PRML.ToString());
            ElsUiPanel.SetUiDesc(_prefix + CurrentSecPattern.ToString().PadLeft(3, '0'), ExtraEnum.SECL.ToString());
            ElsUiPanel.SetUiDesc(_prefix + CurrentWrnPattern.ToString().PadLeft(3, '0'), ExtraEnum.WRNL.ToString());
            ElsUiPanel.ToggleUiBtnState(prmLights, "PRML");
            ElsUiPanel.ToggleUiBtnState(secLights, "SECL");
            ElsUiPanel.ToggleUiBtnState(wrnLights, "WRNL");
            ElsUiPanel.ToggleUiBtnState(crsLights, "CRS");
            if (_extras.SCL != null)
            {
                ElsUiPanel.ToggleUiBtnState(_extras.SCL.TurnedOn, "SCL");
            }
            if (_extras.TDL != null)
            {
                ElsUiPanel.ToggleUiBtnState(_extras.TDL.TurnedOn, "TDL");
            }
        }

        private void SetGTASirens(bool state) {
            _vehicle.IsSirenActive = state;
        }

        private void AddAllValidLightExtras()
        {
            for (int x = 1; x < 13; x++)
            {
                switch (x)
                {
                    case 1:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 1) && _vcfroot.EOVERRIDE.Extra01.IsElsControlled)
                            {
                                this._extras.PRML.Add(1, new Extra.Extra(_vehicle, 1, _vcfroot.EOVERRIDE.Extra01, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 2:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 2) && _vcfroot.EOVERRIDE.Extra02.IsElsControlled)
                            {
                                this._extras.PRML.Add(2, new Extra.Extra(_vehicle, 2, _vcfroot.EOVERRIDE.Extra02, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 3:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 3) && _vcfroot.EOVERRIDE.Extra03.IsElsControlled)
                            {
                                this._extras.PRML.Add(3, new Extra.Extra(_vehicle, 3, _vcfroot.EOVERRIDE.Extra03, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 4:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 4) && _vcfroot.EOVERRIDE.Extra04.IsElsControlled)
                            {
                                this._extras.PRML.Add(4, new Extra.Extra(_vehicle, 4, _vcfroot.EOVERRIDE.Extra04, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 5:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 5) && _vcfroot.EOVERRIDE.Extra05.IsElsControlled)
                            {
                                this._extras.WRNL.Add(5, new Extra.Extra(_vehicle, 5, _vcfroot.EOVERRIDE.Extra05, _vcfroot.WRNL.LightingFormat));
                            }
                        }
                        break;
                    case 6:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 6) && _vcfroot.EOVERRIDE.Extra06.IsElsControlled)
                            {
                                this._extras.WRNL.Add(6, new Extra.Extra(_vehicle, 6, _vcfroot.EOVERRIDE.Extra06, _vcfroot.WRNL.LightingFormat));
                            }
                        }
                        break;
                    case 7:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 7) && _vcfroot.EOVERRIDE.Extra07.IsElsControlled)
                            {
                                this._extras.SECL.Add(7, new Extra.Extra(_vehicle, 7, _vcfroot.EOVERRIDE.Extra07, _vcfroot.SECL.LightingFormat));
                            }
                        }
                        break;
                    case 8:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 8) && _vcfroot.EOVERRIDE.Extra08.IsElsControlled)
                            {
                                this._extras.SECL.Add(8, new Extra.Extra(_vehicle, 8, _vcfroot.EOVERRIDE.Extra08, _vcfroot.SECL.LightingFormat));
                            }
                        }
                        break;
                    case 9:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 9) && _vcfroot.EOVERRIDE.Extra09.IsElsControlled)
                            {
                                this._extras.SECL.Add(9, new Extra.Extra(_vehicle, 9, _vcfroot.EOVERRIDE.Extra09, _vcfroot.SECL.LightingFormat));
                            }
                        }
                        break;
                    case 10:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 10) && _vcfroot.EOVERRIDE.Extra10.IsElsControlled)
                            {
                                this._extras.SBRN = new Extra.Extra(_vehicle, 10, _vcfroot.EOVERRIDE.Extra10);
                            }
                        }
                        break;
                    case 11:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 11) && _vcfroot.EOVERRIDE.Extra11.IsElsControlled)
                            {
                                this._extras.SCL = new Extra.Extra(_vehicle, 11, _vcfroot.EOVERRIDE.Extra11);
                            }
                        }
                        break;
                    case 12:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 12) && _vcfroot.EOVERRIDE.Extra12.IsElsControlled)
                            {
                                this._extras.TDL = new Extra.Extra(_vehicle, 12, _vcfroot.EOVERRIDE.Extra12);
                            }
                        }
                        break;
                }
            }
            if (!String.IsNullOrEmpty(_vcfroot.MISC.ArrowboardType))
            {
                switch (_vcfroot.MISC.ArrowboardType)
                {
                    case "bonnet":
                        this._extras.BRD = new Board.ArrowBoard(_vehicle, _vcfroot.MISC);
                        break;
                    case "boot":
                        this._extras.BRD = new Board.ArrowBoard(_vehicle, _vcfroot.MISC);
                        break;
                    case "boot2":
                        this._extras.BRD = new Board.ArrowBoard(_vehicle, _vcfroot.MISC);
                        break;
                    case "boots":
                        this._extras.BRD = new Board.ArrowBoard(_vehicle, _vcfroot.MISC);
                        break;
                    case "off":
                        this._extras.BRD = new Board.ArrowBoard(_vehicle, _vcfroot.MISC);
                        break;
                    default:
                        this._extras.BRD = new Board.ArrowBoard(_vehicle, _vcfroot.MISC);
                        break;
                }
            }
        }


        Vehicle IManagerEntry._vehicle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void CleanUP()
        {
            foreach(Extra.Extra e in _extras.PRML.Values)
            {
                e.CleanUp();
            }
            foreach (Extra.Extra e in _extras.SECL.Values)
            { 
                e.CleanUp();
            }
            foreach (Extra.Extra e in _extras.WRNL.Values)
            {
                e.CleanUp();
            }
            if (_extras.SBRN != null)
            {
                _extras.SBRN.CleanUp();
            }
            if (_extras.TDL != null)
            {
               _extras.TDL.CleanUp();
            }
            if (_extras.SCL != null)
            {
               _extras.SCL.CleanUp();
            }
            
            
        }

        public void LightsControlsRemote()
        {
            CitizenFX.Core.Debug.WriteLine("LightsControlsRemote");
        }


    }
}
