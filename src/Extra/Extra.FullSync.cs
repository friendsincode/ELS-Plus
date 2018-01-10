using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Extra
{
    internal partial class Extra
    {

        public Dictionary<string, object> GetData()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pattern", PatternNum);
            dic.Add("patternrunning", IsPatternRunning);
            dic.Add("state", state);
            return dic;
        }

        public void SetData(IDictionary<string, object> data)
        {
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"Got data for {_Id} setting data");
            CitizenFX.Core.Debug.WriteLine($"pattern: {data["pattern"]} running: {data["patternrunning"]} state: {data["state"]}");
#endif
            PatternNum = int.Parse(data["pattern"].ToString());
            IsPatternRunning = bool.Parse(data["patternrunning"].ToString());
            SetState(bool.Parse(data["state"].ToString()));

        }

    }
}
