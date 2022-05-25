using System;

namespace Common.Settings.Chromium
{
    public class AppSettings
    {
        public BrowserSettings BrowserSettings { get; set; }

        public string PreInstallUirl { get; set; }

        public string ChromiumProductName { get; set; }

        public string ProductVersion { get; set; }

        public bool FirstRun { get; set; }

        public string TrackingId { get; set; }

        public string AwAccountNumber { get; set; }

        public string AwConversionId { get; set; }

        public string SearchUrl { get; set; }

        public AppSettings()
        {
            BrowserSettings = new BrowserSettings();
        }
    }
}
