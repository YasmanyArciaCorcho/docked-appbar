using SearchBar.UI.Controls.Dashboad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SearchBar.UI.Base
{
    public interface IDashboardHandler<T> where T : IDashboard
    {
        T Dashboard { get; set; }
    }
}
