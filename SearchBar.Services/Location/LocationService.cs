using Common;
using Common.Logger;
using Common.Settings.UserIp;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Services.Location
{
    public class LocationService : ILocationService
    {
        const string retriveLocationFromExternalIpAddressUrl = "http://ip-api.com/json/{0}";

        private readonly string _userIp = "";

        public LocationService(IUserExternalIpResolver userExternalIpResolver)
        {
            _userIp = userExternalIpResolver.UserIp;
        }

        public ILocation GetUserLocation()
        {
            string query = string.Format(retriveLocationFromExternalIpAddressUrl, _userIp);
            string queryResult = HTTPRequestHelper.DoQuery(query);

            try
            {
                LocationModelAPI locationApiModel = JsonConvert.DeserializeObject<LocationModelAPI>(queryResult);

                return new Location()
                {
                    City = locationApiModel.city,
                    Country = locationApiModel.country,
                    Latitude = locationApiModel.lat,
                    Longitude = locationApiModel.lon,
                };
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return null;
            }
        }
    }
}
