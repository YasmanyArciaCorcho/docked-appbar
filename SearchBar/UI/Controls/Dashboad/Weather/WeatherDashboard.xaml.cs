using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.WebBar;
using Services.Weather;
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

namespace SearchBar.UI.Controls.Dashboad.Weather
{
    /// <summary>
    /// Interaction logic for WeatherDashboard.xaml
    /// </summary>
    public partial class WeatherDashboard : UserControl
    {
        public static string DashboardName = "Weather";
        public static string ImagePath = "weather_logo";

        public WeatherDashboard(WebBarViewModel webBarViewModel, IWeatherService weatherService, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            MainWeather.DataContext = weatherService.GetTodayWeather();

            List<IWeather> weatherForecast = weatherService.WeeklyForecast();

            List<DailyWeatherWidget> dailyWeather = new List<DailyWeatherWidget>()
            {
                Next1,
                Next2,
                Next3,
                Next4,
                Next5,
                Next6
            };

            if (weatherForecast != null && weatherForecast.Count > 0)
            {
                Today.DataContext = weatherForecast[0];

                for (int i = 1; i < weatherForecast.Count; i++)
                {
                    dailyWeather[i - 1].DataContext = weatherForecast[i];
                }
            }
            else
            {
                HideDailyWeatherControls(dailyWeather);
            }

            WeatgerRadar.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.accuweather.com/en/us/national/weather-radar"); };
            StormTracker.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.accuweather.com/en/hurricane"); };
            Snowfall.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.accuweather.com/en/us/winter-weather"); };
            Climate.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://climate-charts.com/World-Climate-Maps.html#temperature"); };
            GlobalWarming.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://climate-charts.com/World-Climate-Maps.html#warming"); };
            WindSpeed.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://climate-charts.com/World-Climate-Maps.html#wind-speed"); };
        }

        public void HideDailyWeatherControls(List<DailyWeatherWidget> dailyWeather)
        {
            Today.Visibility = Visibility.Collapsed;

            for (int i = 0; i < dailyWeather.Count; i++)
            {
                dailyWeather[i].Visibility = Visibility.Collapsed;
            }
        }


        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "weather_{0}";
            imageSourceBuilder.SetImageSource(WeatgerRadar.Image, string.Format(imageNamespace, "radar"));
            imageSourceBuilder.SetImageSource(StormTracker.Image, string.Format(imageNamespace, "stormtracker"));
            imageSourceBuilder.SetImageSource(Snowfall.Image, string.Format(imageNamespace, "snowflake"));
            imageSourceBuilder.SetImageSource(Climate.Image, string.Format(imageNamespace, "climate"));
            imageSourceBuilder.SetImageSource(GlobalWarming.Image, string.Format(imageNamespace, "globalwarming"));
            imageSourceBuilder.SetImageSource(WindSpeed.Image, string.Format(imageNamespace, "windspeed"));
        }
    }
}
