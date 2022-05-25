using Services.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Weather
{
    public interface IWeatherService
    {
        ILocationService LocationService { get; set; }

        IWeather GetTodayWeather();

        List<IWeather> WeeklyForecast();
    }
}
