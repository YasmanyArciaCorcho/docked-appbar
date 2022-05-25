using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchBar.UI.Builders.Dashboard
{
    public interface IDashboardControl
    {
        string DashboardName { get; }
 
        string ImagePath { get; }

        UserControl Build(WebBarViewModel webBarView);
    }
}
