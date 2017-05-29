using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS
{
    public static class Extentions
    {
        public static bool IsELS(this Vehicle vehicle)
        {
            return configuration.VCF.ELSVehicle.Exists(obj => obj.FileName == vehicle.DisplayName);
        }
        
    }
}
