using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Location;

namespace Services.Weather
{
    public class Weather : IWeather
    {
        public ILocation Location 
        {get;set;}
        public string Description
        {get;set;}
        public string IconPath
        {get;set;}
        public double Sunrise
        {get;set;}
        public double Sunset 
        {get;set;}
        public double Pressure
        {get;set;}
        public double Humidity
        {get;set;}
        public double Temperature 
        {get;set;}
        public string DayInformation { get; set; }
    }
}
