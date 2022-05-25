using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((dateTime - epoch).TotalSeconds);
        }
    }
}
