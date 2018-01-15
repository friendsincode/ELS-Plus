using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.Light.Patterns
{

    internal enum PatternType
    {
        PRML,
        SECL,
        WRNL
    }

    internal struct Pattern
    {
        internal PatternType Type;
        internal int Delay;
        internal string StringPattern;
        internal int BasePattern;
        internal int PatternNumber;

        internal Pattern(PatternType type, int delay, string patt, int num,int _base = 0)
        {
            Type = type;
            Delay = delay;
            StringPattern = patt;
            PatternNumber = num;
            BasePattern = _base;
        }
    }
}
