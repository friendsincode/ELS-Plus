using CitizenFX.Core;
using ELS.configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    interface ILight
    {
        Vehicle _vehicle { get; set; }
        Scene scene { get; set; }
        SpotLight spotLight { get; set; }
        Vcfroot _vcfroot { get; set; }
    }
}
