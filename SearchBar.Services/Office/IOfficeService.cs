using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Office
{
    public interface IOfficeService
    {
        void StartOfficeApp(OfficeLaunchApp appToStart);
    }
}
