using CitizenFX.Core;
using ELS.configuration;

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
