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
            throw new NotImplementedException();
        }

        public void SetData(IDictionary<string, object> data)
        {
            throw new NotImplementedException();
        }
    }
}
