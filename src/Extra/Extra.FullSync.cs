using ELS.FullSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Extra
{
    internal partial class Extra : IFullSyncComponent
    {

        public Dictionary<string, object> GetData()
        {
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
           
            dic.Add("patternrunning", IsPatternRunning);
            dic.Add("on", TurnedOn);
            if (spotLight != null)
            {
                dic.Add("spotlight", spotLight.GetData());
            }
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"Got data for {_Id} setting data");
#endif
            //PatternNum = int.Parse(data["pattern"].ToString());
            IsPatternRunning = bool.Parse(data["patternrunning"].ToString());
            TurnedOn = bool.Parse(data["on"].ToString());
            if (spotLight != null)
            {
                spotLight.SetData((IDictionary<string, object>)data["spotlight"]);
            }
        }

    }
}
