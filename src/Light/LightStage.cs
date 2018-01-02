using ELS.configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    enum ELightStage
    {
        Lstg0=0,
        Lstg1 =1,
        Lstg2=2,
        Lstg3=3
    }
    class LightStage
    {
        ELightStage currentStage;
        internal LightStage(): this(ELightStage.Lstg0)
        {
            
        }
        internal LightStage(ELightStage stage)
        {
            currentStage = stage;
        }
        internal void NextStage()
        {

        }
        internal void PrevousStage()
        {

        }

        internal Lstg GetPresetPrimaryLstg(ELightStage stage, Vcfroot root)
        {
            switch (stage)
            {
                case ELightStage.Lstg1:
                    return root.PRML.PresetPatterns.Lstg1;
                case ELightStage.Lstg2:
                    return root.PRML.PresetPatterns.Lstg2;
                case ELightStage.Lstg3:
                    return root.PRML.PresetPatterns.Lstg3;
                default:
                    return new Lstg();
            }
        }

        internal Lstg GetPresetSecondaryLstg(ELightStage stage, Vcfroot root)
        {
            switch (stage)
            {
                case ELightStage.Lstg1:
                    return root.SECL.PresetPatterns.Lstg1;
                case ELightStage.Lstg2:
                    return root.SECL.PresetPatterns.Lstg2;
                case ELightStage.Lstg3:
                    return root.SECL.PresetPatterns.Lstg3;
                default:
                    return new Lstg();
            }
        }
    }
}
