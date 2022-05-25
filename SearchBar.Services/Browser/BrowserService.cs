using System;
using System.Diagnostics;
using System.IO;
using Common.Logger;
using Common.Settings.Chromium;
using Common.ChromiumSettings;
using Common.ExtensionMethods;

namespace Services.Browser
{
    public class BrowserService : IBrowserService
    {
        public IBrowser DefaultBrowser
        { get; set; }

        readonly string _urlToDoSearch = "";
        readonly string _ntpUrl = "";
        readonly string _defaultBrowserPath = "";

        public void Open(string url)
        {
            Open(DefaultBrowser, String.Format(_urlToDoSearch, url));
        }

        public BrowserService(IChromiumSettingsService settingService)
        {
            DefaultBrowser = GetBrowser(settingService.BrowserSettings.DefaultPath, settingService.BrowserSettings.DefaultPath);
            _urlToDoSearch = settingService.GetSearchUrl();
            _ntpUrl = settingService.GetNTPUrl();
            _defaultBrowserPath = settingService.GetSystemDefaultBrowser();
        }

        public void Open(IBrowser browser, string url)
        {
            string browserPath;
            if (browser != null && !String.IsNullOrEmpty(browser.Path) && File.Exists(browser.Path))
            {
                browserPath = browser.Path;
            }
            else
            {
                browserPath = _defaultBrowserPath;
            }

            if (string.IsNullOrEmpty(url))
                url = _ntpUrl;

            url = url.RemplaceUrlWhiteSpace();

            ProcessStartInfo startInfo = new ProcessStartInfo(browserPath)
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                Arguments = url
            };

            StaticLogger.Logger.Info($"BrowserService - request to the browser.");
            Process.Start(startInfo);
        }

        private IBrowser GetBrowser(string name, string path) => new Browser(name, path);
    }
}
