using Common.Settings.Chromium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ChromiumSettings
{
    public interface IChromiumSettingsService
    {
        string this[string index]
        {
            get;
            set;
        }

        BrowserSettings BrowserSettings
        { get; set; }

        MasterPreferences MasterPreferences
        { get; set; }

        string ChromiumProductName
        { get; set; }

        string ProductVersion
        { get; set; }

        bool FirstRun { get; }

        string GetSearchUrl();

        bool SendImpression(string impressionEvent);

        string GetShortcutPackageUrl(string packageName);

        string GetNTPUrl();

        string GetImplementationId();

        string GetDomain();

        string GetSystemDefaultBrowser();
    }
}
