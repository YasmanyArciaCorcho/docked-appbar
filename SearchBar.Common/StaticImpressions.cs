using Common.Logger;
using Common.ChromiumSettings;
using System;

namespace Common.Impressions
{
    public static class StaticImpressions
    {
        public static IChromiumSettingsService SettingsService { get; set; }

        public static void SendImpression(string impressionEvent)
        {
            try
            {
                if (SettingsService != null)
                    SettingsService.SendImpression(impressionEvent);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
        }
    }
}