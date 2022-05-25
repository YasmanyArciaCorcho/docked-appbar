using Services.Location.Cities;
using Services.Location.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classifields
{
  public  interface IClassifieldsService
    {
        IEnumerable<State> GetStates();

        IEnumerable<City> GetCities(string stateCode);

        IEnumerable<ClassifieldsCategory> GetSearchCategory();
    }
}
