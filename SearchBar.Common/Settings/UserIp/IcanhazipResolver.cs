using Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings.UserIp
{
    public class IcanhazipResolver : IUserExternalIpResolver
    {
        const string retriveExternalIpAddresURL = "http://icanhazip.com";

        private string _userExternalIp;
        public string UserIp
        {
            get
            {
                if (string.IsNullOrEmpty(_userExternalIp))
                {
                    try
                    {
                        using var webClient = new WebClient();
                        _userExternalIp = webClient.DownloadString(retriveExternalIpAddresURL);
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Error(e);
                    }
                }

                return _userExternalIp;
            }
        }
    }
}
