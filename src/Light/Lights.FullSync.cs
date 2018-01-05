using ELS.FullSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light
{
    partial class Lights : IFullSyncComponent
    {
        public Dictionary<string, object> GetData()
        {

            var dic = new Dictionary<string, object>
            {
                {"_extras", _extras }
            };
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
            if (data.ContainsKey("_extras"))
            {
               // SetExtrasData();
            }
            else if (data.TryGetValue("_enabled", out object res))
            {
              //  _enabled = (bool)res
            }
        }
    }
}
