using SearchBar.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Handles.TopSites
{
    public interface ITopSitesHandler<T> where T : IDashboard
    {
        void Initialize(T dashboard);
    }
}
