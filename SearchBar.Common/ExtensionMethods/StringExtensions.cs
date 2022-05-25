using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string UpperCaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            s = s.ToLower();
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string RemplaceWhiteSpaceToPercent(this string origin)
        {
            return origin.Replace(" ", "%20");
        }

        public static string RemplaceUrlWhiteSpace(this string url)
        {
            return url.Replace(' ', '+');
        }

        public static bool ContainsHTTPPreffix(this string url)
        {
            return url.StartsWith("http");
        }

        public static string ShortString(this string origin, int size)
        {
            if (origin.Length > size)
                return origin.Substring(0, size);
            return origin;
        }
    }
}
