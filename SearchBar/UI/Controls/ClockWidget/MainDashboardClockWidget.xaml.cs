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
using System.Windows.Threading;

namespace SearchBar.UI.Controls.ClockWidgets
{
    /// <summary>
    /// Interaction logic for MainDashboardClockWidget.xaml
    /// </summary>
    public partial class MainDashboardClockWidget : UserControl
    {
        public MainDashboardClockWidget()
        {
            InitializeComponent();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            Hour.Content = DateTime.Now.Hour;
            Mins.Content = DateTime.Now.ToString("mm tt");
        }
    }
}
