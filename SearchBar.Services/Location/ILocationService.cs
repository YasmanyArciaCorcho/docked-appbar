using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Location
{
   public interface ILocationService
    {
       ILocation GetUserLocation();
    }
}
