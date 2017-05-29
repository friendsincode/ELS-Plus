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
    }
}
