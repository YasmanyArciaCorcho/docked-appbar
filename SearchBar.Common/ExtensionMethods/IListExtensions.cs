using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class IListExtensions
    {
        public static string JoinList(this IList<string> list, string startString, string separator, string endString)
        {
            string result = startString;

            if (list.Count > 0)
                result += $"'{list[0]}'";

            for (int i = 1; i < list.Count; i++)
            {
                result += separator + $"'{list[i]}'";
            }

            return result + endString;
        }
    }
}
