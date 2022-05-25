using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
   public static class IConvertibleExtensions
    {
        public static string GetDescription<T>(this IConvertible e)
        {
            if (!(e is Enum)) return null; // could also return string.Empty
            var type = e.GetType();
            var values = System.Enum.GetValues(type);

            return (from int val in values
                    where val == e.ToInt32(CultureInfo.InvariantCulture)
                    select type.GetMember(type.GetEnumName(val))
                    into memInfo
                    select memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault()).OfType<DescriptionAttribute>()
                .Select(descriptionAttribute => descriptionAttribute.Description)
                .FirstOrDefault();
        }
    }
}
