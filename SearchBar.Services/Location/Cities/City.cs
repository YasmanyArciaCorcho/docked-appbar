using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Location.Cities
{
    public class City
    {
        public City(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }
        public string Name { get; }
    }
}
