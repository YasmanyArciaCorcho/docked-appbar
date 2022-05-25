using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static string CharEnumerableToString(this IEnumerable<char> characters)
        {
            string srt = "";
            foreach (var character in characters)
            {
                srt += character;
            }
            return srt;
        }
    }
}
