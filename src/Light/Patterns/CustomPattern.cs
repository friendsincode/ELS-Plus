using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light.Patterns
{
    public struct CustomPattern
    {
        internal CustomPattern(int prm, int sec, int ta, Dictionary<int,string> dict) 
        {
            PrmDelay = prm;
            SecDelay = sec;
            TADelay = ta;
            PatternData = dict;
        }
        internal Dictionary<int,string> PatternData { get; set; }
        internal int PrmDelay { get; set; }
        internal int SecDelay { get; set; }
        internal int TADelay { get; set; }

    }
}
