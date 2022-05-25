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
    /// Interaction logic for ClockWidget.xaml
    /// </summary>
    public partial class WeatherClockWidget : UserControl
    {
        public WeatherClockWidget()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void InitializeClock()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += RefreshClock;
            timer.Start();
        }

        private void RefreshClock(object sender, EventArgs e)
        {
            string currentTime = DateTime.Now.ToShortTimeString();
            currentTime = currentTime.Insert(currentTime.Length - 3, $":{DateTime.Now.Second} ");
            Time.Content = currentTime;
            Date.Content = DateTime.Now.Date.ToShortDateString();
        }
    }
}
