using CitizenFX.Core;
using ELS.NUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    partial class Lights
    {
        internal void ControlsTicker()
        {
            ToggleSecLKB();
            ToggleWrnLKB();
        }

        internal void ToggleSecLKB()
        {
            Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.ToggleSecL);
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleSecL))
            {
                ToggleSecLights();
            }
        }

        internal void ToggleSecLights()
        {
            foreach (Extra.Extra ex in _extras.SECL.Values)
            {
                if (ex.IsPatternRunning)
                {
                    ex.IsPatternRunning = false;

                }
                else
                {
                    ex.IsPatternRunning = true;
                    ElsUiPanel.SetUiPatternNumber(0, "SECL");
                    if (ex.Id == 5)
                    {
                        ex.RunPattern(LightPattern.StringPatterns[0]);
                    }
                    else
                    {
                        ex.RunPattern(LightPattern.StringPatterns[1]);
                    }
                }
            }
        }

        internal void ToggleWrnLKB()
        {
            Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.ToggleWrnL);
            if (Game.IsDisabledControlJustPressed(0, configuration.ControlConfiguration.KeyBindings.ToggleWrnL))
            {
                ToggleWrnLights();
            }
        }

        internal void ToggleWrnLights()
        {
            foreach (Extra.Extra ex in _extras.WRNL.Values)
            {
                if (ex.IsPatternRunning)
                {
                    ex.IsPatternRunning = false;

                }
                else
                {
                    ex.IsPatternRunning = true;
                    ElsUiPanel.SetUiPatternNumber(0, "WNRL");
                    switch (ex.Id)
                    {
                        case 7:
                            ex.RunPattern(LightPattern.StringPatterns[39]);
                            break;
                        case 8:
                            ex.RunPattern(LightPattern.StringPatterns[38]);
                            break;
                        case 9:
                            ex.RunPattern(LightPattern.StringPatterns[39]);
                            break;
                    }
                }
            }
        }
    }
}
