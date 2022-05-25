using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.String
{
   public interface ICmdStringRead
    {
        bool HasKey(string parameterName);

        string GetValue(string paramenterName);
    }
}
