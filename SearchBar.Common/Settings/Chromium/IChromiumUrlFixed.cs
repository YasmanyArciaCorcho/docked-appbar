using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings.Chromium
{
    public interface IChromiumUrlFixed
    {
        string GetSearchUrl(string domain, string userId, string iid, string adProvider, string uc, string source);

        void SendImpression(string impressionEvent, string domain, string userId, string source, string adProvider, string referrer, string userAgent, string userClass, string subId2, string implementationId, string page, string awAccountNumber, string awConversionId, string trackingId);
    }
}
