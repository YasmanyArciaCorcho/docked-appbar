using Common.Settings.UserIp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings.Chromium.AWSElastic
{
    public class AWSElasticChromiumUrlFixed : IChromiumUrlFixed
    {
        const string _yahooSearchUrl = "https://api.{0}/search/yhs/?userid={1}&iid={2}&ap={3}&uc={4}&source={5}";
        readonly string _impressionUrl = "https://api.{0}/log/imp/e/{1}/?ip={ip-value}&user_id={2}&source={3}&traffic_source={4}&referrer={5}&useragent={6}&subid={7}&subid2={8}&implementation_id={9}&page={10}&offer_id={11}&pitch_id={12}&tid={13}";

        public AWSElasticChromiumUrlFixed(IUserExternalIpResolver userExternalIpResolver)
        {
            _impressionUrl = _impressionUrl.Replace("{ip-value}", userExternalIpResolver.UserIp);
        }

        public void SendImpression(string impressionEvent, string domain, string userId, string source, string adProvider,
            string referrer, string userAgent, string userClass, string subId2, string implementationId,
            string page, string awAccountNumber, string aswConversionId, string trackingId)
        {
            Task.Run(async () =>
            {
                await HTTPRequestHelper.DoAsyncQuery(string.Format(_impressionUrl, domain, impressionEvent, userId, source, adProvider, referrer, userAgent, userClass, subId2, implementationId, page, awAccountNumber, aswConversionId, trackingId));
            });

        }

        public string GetSearchUrl(string domain, string userId, string iid, string adProvider, string uc, string source)
        {
            string searchUrl = HTTPRequestHelper.DoQuery(string.Format(_yahooSearchUrl, domain, userId, iid, adProvider, uc, source)/*, new List<KeyValuePair<string, string>>() { AWSElasticSettings.AWSElasticApiKey, AWSElasticSettings.AWSElasticName }*/);
            searchUrl = searchUrl.Replace("searchTerms", "0");
            return searchUrl;
        }
    }
}