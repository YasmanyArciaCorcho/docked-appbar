using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Location.States
{
    public interface IStateProvider
    {
        State[] GetStates();
    }
}
