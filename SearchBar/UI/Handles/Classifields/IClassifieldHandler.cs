using SearchBar.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Handles.Classifields
{
    public interface IClassifieldHandler
    {
        public void UpdateStateList();

        public void UpdateCitiesList(string stateCode);

        public void UpdateSearchCategory();
    }

    public interface IClassifieldHandler<T> : IClassifieldHandler, IDashboardHandler<T> where T : IDashboard
    {

    }
}
