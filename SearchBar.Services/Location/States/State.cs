using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Location.States
{
    public class State
    {
        public State(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            if (obj is State other)
            {
                if (!Code.Equals(other.Code))
                    return false;

                if (!Name.Equals(other.Name))
                    return false;

                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
