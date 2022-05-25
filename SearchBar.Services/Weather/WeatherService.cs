using Common;
using Common.ExtensionMethods;
using Common.Logger;
using Newtonsoft.Json;
using Services.Location;
using System;
using System.Collections.Generic;

namespace Services.Weather
{
    public class WeatherService : IWeatherService
    {
        const string _apiKey = "e227543966ce3ce124a59a27c4806ddf";
        const string _weatherAPIUrl = "https://api.openweathermap.org/data/2.5/forecast/daily?appid={0}&lat={1}&lon={2}";
        const string _weatherAPIIconUrl = "http://openweathermap.org/img/wn/{0}@2x.png";

        public ILocationService LocationService { get; set; }

        public List<IWeather> WeeklyWeather { get; set; }

        public WeatherService(ILocationService locationService)
        {
            LocationService = locationService;
        }

        public IWeather GetTodayWeather()
        {
            if (WeeklyWeather == null)
                UpdateForecast();

            if (WeeklyWeather != null && WeeklyWeather.Count > 0)
                return WeeklyWeather[0];
            return null;
        }

        public List<IWeather> WeeklyForecast()
        {
            if (WeeklyWeather == null)
                UpdateForecast();
         
            return WeeklyWeather;
        }

        private void UpdateForecast()
        {
            try
            {
                ILocation userLocation = LocationService.GetUserLocation();
                string query = string.Format(_weatherAPIUrl, _apiKey, userLocation.Latitude, userLocation.Longitude);

                string queryResult = HTTPRequestHelper.DoQuery(query);

                WeatherModelAPI weatherForescast = JsonConvert.DeserializeObject<WeatherModelAPI>(queryResult);

                if (weatherForescast != null)
                {
                    WeeklyWeather = new List<IWeather>();

                    StaticLogger.Logger.Info("Weather service - created weather information.");

                    DateTime currentDate = DateTime.Today;
                    string dayInformation = currentDate.DayOfWeek.ToString().Substring(0, 3) + ", " + currentDate.Day + $"/{currentDate.Month}";

                    foreach (var forecast in weatherForescast.list)
                    {
                        WeeklyWeather.Add(new Weather()
                        {
                            DayInformation = dayInformation,
                            Location = userLocation,
                            Description = forecast.weather[0].description.UpperCaseFirst(),
                            Humidity = Math.Round((double)forecast.humidity, 2),
                            IconPath = string.Format(_weatherAPIIconUrl, forecast.weather[0].icon),
                            Pressure = Math.Round((double)forecast.pressure, 2),
                            Sunrise = forecast.sunrise,
                            Sunset = forecast.sunset,
                            Temperature = Math.Round((forecast.temp.day - 273.15) * 9 / 5 + 32, 2)
                        });

                        currentDate = currentDate.AddDays(1);
                        dayInformation = currentDate.DayOfWeek.ToString().Substring(0, 3) + ", " + currentDate.Day + $"/{currentDate.Month}";
                    }
                }
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
        }
    }
}
