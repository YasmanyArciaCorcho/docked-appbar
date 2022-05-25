using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchBar.UI.Base
{
    public interface IDashboardControlParent
    {
        void ToggleDashboard(UserControl dashboard);
       
        void SetDashboard(UserControl dashboard);
        
        void ClearDashboard();

        WebBarViewModel WebBarViewModel
        { get; }
    }
}
