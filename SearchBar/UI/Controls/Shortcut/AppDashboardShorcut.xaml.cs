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

namespace SearchBar.UI.Controls.Shortcut
{
    /// <summary>
    /// Interaction logic for AppDashboardShorcut.xaml
    /// </summary>
    public partial class AppDashboardShorcut : UserControl
    {
        private bool _isPinned;
        public bool IsPinned
        {
            get 
            { return _isPinned; }

            set 
            {
                _isPinned = value;
                if (_isPinned)
                {
                    Ellipse.Visibility = Visibility.Visible;
                }
                else 
                {
                    Ellipse.Visibility = Visibility.Collapsed;
                }
            }
        }

        public AppDashboardShorcut()
        {
            InitializeComponent();
        }
    }
}
