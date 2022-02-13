using System;

namespace ELS.Light.Patterns
{
    internal static class PatternUtils
    {
        internal static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
