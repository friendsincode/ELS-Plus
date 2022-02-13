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
        internal Gadgets.Ladder LDR;
    }

    partial class Lights : IManagerEntry, ILight
    {
        private Extras _extras = new Extras
        {
            PRML = new Dictionary<int, Extra.Extra>(),
            WRNL = new Dictionary<int, Extra.Extra>(),
            SECL = new Dictionary<int, Extra.Extra>(),
        };

        public Vcfroot _vcfroot { get; set; }
        public Vehicle _vehicle { get; set; }
        internal Stage _stage;
        public Scene scene { get; set; }
        public SpotLight spotLight { get; set; }


        internal Lights(Vehicle vehicle, Vcfroot vcfroot, [Optional] LightFSData data)
        {
            _vcfroot = vcfroot;
            _vehicle = vehicle;
            AddAllValidLightExtras();
            LightStagesSetup();
            SetupPatternsPrm();
            SetupSecPatterns();
            SetupWrnPatterns();
            if (!data.Equals(null))
            {
                SetData(data);
            }
        }

        private void LightStagesSetup()
        {
            _stage = new Stage(_vcfroot.PRML, _vcfroot.SECL, _vcfroot.WRNL, _vehicle.GetElsId(), _vcfroot.INTERFACE.LstgActivationType);
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
            if (scene != null)
            {
                ElsUiPanel.ToggleUiBtnState(scene.TurnedOn, "SCL");
            }
            if (spotLight != null)
            {
                ElsUiPanel.ToggleUiBtnState(spotLight.TurnedOn, "TDL");
            }
        }

        internal void SetGTASirens(bool state)
        {
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
                                this._extras.PRML.Add(1, new Extra.Extra(this, 1, _vcfroot.EOVERRIDE.Extra01, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 2:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 2) && _vcfroot.EOVERRIDE.Extra02.IsElsControlled)
                            {
                                this._extras.PRML.Add(2, new Extra.Extra(this, 2, _vcfroot.EOVERRIDE.Extra02, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 3:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 3) && _vcfroot.EOVERRIDE.Extra03.IsElsControlled)
                            {
                                this._extras.PRML.Add(3, new Extra.Extra(this, 3, _vcfroot.EOVERRIDE.Extra03, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 4:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 4) && _vcfroot.EOVERRIDE.Extra04.IsElsControlled)
                            {
                                this._extras.PRML.Add(4, new Extra.Extra(this, 4, _vcfroot.EOVERRIDE.Extra04, _vcfroot.PRML.LightingFormat));
                            }
                        }
                        break;
                    case 5:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 5) && _vcfroot.EOVERRIDE.Extra05.IsElsControlled)
                            {
                                this._extras.WRNL.Add(5, new Extra.Extra(this, 5, _vcfroot.EOVERRIDE.Extra05, _vcfroot.WRNL.LightingFormat));
                            }
                        }
                        break;
                    case 6:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 6) && _vcfroot.EOVERRIDE.Extra06.IsElsControlled)
                            {
                                this._extras.WRNL.Add(6, new Extra.Extra(this, 6, _vcfroot.EOVERRIDE.Extra06, _vcfroot.WRNL.LightingFormat));
                            }
                        }
                        break;
                    case 7:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 7) && _vcfroot.EOVERRIDE.Extra07.IsElsControlled)
                            {
                                this._extras.SECL.Add(7, new Extra.Extra(this, 7, _vcfroot.EOVERRIDE.Extra07, _vcfroot.SECL.LightingFormat));
                            }
                        }
                        break;
                    case 8:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 8) && _vcfroot.EOVERRIDE.Extra08.IsElsControlled)
                            {
                                this._extras.SECL.Add(8, new Extra.Extra(this, 8, _vcfroot.EOVERRIDE.Extra08, _vcfroot.SECL.LightingFormat));
                            }
                        }
                        break;
                    case 9:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 9) && _vcfroot.EOVERRIDE.Extra09.IsElsControlled)
                            {
                                this._extras.SECL.Add(9, new Extra.Extra(this, 9, _vcfroot.EOVERRIDE.Extra09, _vcfroot.SECL.LightingFormat));
                            }
                        }
                        break;
                    case 10:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 10) && _vcfroot.EOVERRIDE.Extra10.IsElsControlled)
                            {
                                this._extras.SBRN = new Extra.Extra(this, 10, _vcfroot.EOVERRIDE.Extra10);
                            }
                        }
                        break;
                    case 11:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 11) && _vcfroot.EOVERRIDE.Extra11.IsElsControlled && _vcfroot.MISC.Takedowns.AllowUse)
                            {
                                this._extras.TDL = new Extra.Extra(this, 11, _vcfroot.EOVERRIDE.Extra11);
                            }
                            else if (_vcfroot.MISC.Takedowns.AllowUse)
                            {
                                spotLight = new SpotLight(this);
                            }
                        }
                        break;
                    case 12:
                        {
                            if (API.DoesExtraExist(_vehicle.Handle, 12) && _vcfroot.EOVERRIDE.Extra12.IsElsControlled && _vcfroot.MISC.SceneLights.AllowUse)
                            {
                                this._extras.SCL = new Extra.Extra(this, 12, _vcfroot.EOVERRIDE.Extra12);
                            }
                            else if (_vcfroot.MISC.SceneLights.AllowUse)
                            {
                                scene = new Scene(this);
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
                        this._extras.BRD = new Board.ArrowBoard(this, _vcfroot.MISC);
                        break;
                    case "boot":
                        this._extras.BRD = new Board.ArrowBoard(this, _vcfroot.MISC);
                        break;
                    case "boot2":
                        this._extras.BRD = new Board.ArrowBoard(this, _vcfroot.MISC);
                        break;
                    case "boots":
                        this._extras.BRD = new Board.ArrowBoard(this, _vcfroot.MISC);
                        break;
                    case "off":
                        this._extras.BRD = new Board.ArrowBoard(this, _vcfroot.MISC);
                        break;
                    default:
                        this._extras.BRD = new Board.ArrowBoard(this, _vcfroot.MISC);
                        break;
                }
            }
            if (_vcfroot.MISC.HasLadderControl)
            {
                this._extras.LDR = new Gadgets.Ladder(this, _vcfroot.MISC);
            }
        }


        Vehicle IManagerEntry._vehicle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void CleanUP()
        {
            foreach (Extra.Extra e in _extras.PRML.Values)
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
            Utils.DebugWriteLine("LightsControlsRemote");
        }


    }
}
