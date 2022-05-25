using SearchBar.UI.Base;
using Services.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Handles.Games
{
    public interface IGamesHandler
    {
        void SetCategories();

        void UpdateGameZone(string category, string gameName);
    }

    public interface IGamesHandler<T> : IGamesHandler, IDashboardHandler<T> where T : IDashboard
    {

    }
}
