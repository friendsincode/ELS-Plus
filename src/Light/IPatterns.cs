using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    internal interface IPatterns
    {
        int CurrentPrmPattern { get; set; }
        int CurrentSecPattern { get; set; }
        int CurrentWrnPattern { get; set; }
    }
}
