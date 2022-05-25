using System;

namespace Configurations.Settings
{
    public class AppSettings
    {
        public BrowserSettings BrowserSettings { get; set; }

        public string PreInstallUirl { get; set; }

        public string ChromiumProductName { get; set; }

        public string ProductVersion { get; set; }

        public bool FirstRun { get; set; }

        public AppSettings()
        {
            BrowserSettings = new BrowserSettings();
        }
    }
}
