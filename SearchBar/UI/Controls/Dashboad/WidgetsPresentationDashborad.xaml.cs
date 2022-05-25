using SearchBar.UI.Base;
using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Controls.ShortCut;
using SearchBar.UI.Handles.Widgets;
using SearchBar.UI.WebBar;
using Services.ShortcutPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBar.UI.Controls.Dashboad
{
    /// <summary>
    /// Interaction logic for ApplicationsDashborad.xaml
    /// </summary>
    public partial class WidgetsPresentationDashborad : UserControl, IDashboard
    {
        public WebBarViewModel WebBarViewModel { get; set; }
        readonly IWidgetsPresentationHandler<WidgetsPresentationDashborad> _widgetsHandle;

        public WidgetsPresentationDashborad(IWidgetsPresentationHandler<WidgetsPresentationDashborad> widgetsHandle, WebBarViewModel webBarViewModel)
        {
            InitializeComponent();

            _widgetsHandle = widgetsHandle;

            _widgetsHandle.Dashboard = this;
            WebBarViewModel = webBarViewModel;

            _widgetsHandle.Initialize();
        }
    }
}
