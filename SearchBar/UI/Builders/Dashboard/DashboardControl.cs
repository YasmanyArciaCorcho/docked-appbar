using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SearchBar.UI.WebBar;

namespace SearchBar.UI.Builders.Dashboard
{
    public delegate UserControl DashboardControlBuilderFunc(WebBarViewModel view);

    public class DashboardControl : IDashboardControl
    {
        public string DashboardName
        { get; }

        public string ImagePath 
        { get; }

        private readonly DashboardControlBuilderFunc _builder;

        public DashboardControl(string dashboarName, string imagePath, DashboardControlBuilderFunc builder)
        {
            DashboardName = dashboarName;
            ImagePath = imagePath;
            _builder = builder;
        }

        public UserControl Build(WebBarViewModel webBarView)
        {
            if (_builder != null)
                return _builder(webBarView);
            return null;
        }
    }
}
