using Common;
using Common.ExtensionMethods;
using Common.Logger;
using Common.Settings.Chromium;
using Common.String;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Common.ChromiumSettings
{
    public class ChromiumSettingsService : IChromiumSettingsService
    {
        public BrowserSettings BrowserSettings
        { get; set; }
        public MasterPreferences MasterPreferences
        { get; set; }
        public Dictionary<string, string> MasterPreferenceJsonConfig
        { get; set; }
        public string ProductVersion
        { get; set; }
        public string ChromiumProductName
        { get; set; }
        public bool FirstRun
        { get; private set; }
        public string this[string index]
        {
            get
            {
                return MasterPreferenceJsonConfig[index];
            }
            set
            {
                MasterPreferenceJsonConfig[index] = value;
            }
        }

        private readonly IChromiumUrlFixed _chromiumUrlFixed;
        private string _searchUrl = "";
        private string _shortcutPackageUrl = "";
        private string _ntpUrl = "";
        private string _trackingId = "";

        #region AWSElastic

        private static string AwAccountNumber
        { get; set; }
        private static string AwConversionId
        { get; set; }

        #endregion

        public ChromiumSettingsService(IChromiumUrlFixed chromiumUrlFixed, IAppSettingsInitializer appSettingsInitializer)
        {
            AppSettings settings = appSettingsInitializer.Initialize(out bool firstRun);

            _chromiumUrlFixed = chromiumUrlFixed;
            BrowserSettings = settings.BrowserSettings;
            ProductVersion = settings.ProductVersion;
            FirstRun = firstRun;

            ChromiumProductName = string.IsNullOrEmpty(settings.ChromiumProductName) ? "" : settings.ChromiumProductName;

            if (string.IsNullOrEmpty(BrowserSettings.DefaultPath))
                BrowserSettings.DefaultPath = GetBrowserPath(settings.ChromiumProductName);

            InitializePreferenceFromFile(DirectoryInfoHelper.GetMasterPreferenceFilePath());

            if (MasterPreferences == null || string.IsNullOrEmpty(MasterPreferences.json_config))
            {
                InitializePreferenceFromFile(DirectoryInfoHelper.GetLocalStateFilePath());
            }

            if (MasterPreferences == null || string.IsNullOrEmpty(MasterPreferences.json_config))
            {
                StaticLogger.Logger.Trace($"ContainerConfig - master preferences empty, doing query to wbdistro.com");
                string response = HTTPRequestHelper.DoQuery(settings.PreInstallUirl);
                WbdistroResponse wbdistro = JsonConvert.DeserializeObject<WbdistroResponse>(response);
                if (wbdistro != null)
                {
                    InitializeMasterPreferenceConfigFromWbdistroResponse(wbdistro);
                }
                else
                {
                    StaticLogger.Logger.Trace($"SettingsService - empty wbdistro.");
                }
            }

            if (MasterPreferenceJsonConfig != null && MasterPreferenceJsonConfig.ContainsKey("source"))
                MasterPreferenceJsonConfig["source"] = $"{MasterPreferenceJsonConfig["source"]}-sbr";

            if (FirstRun)
                appSettingsInitializer.UpdateAppSettings(settings, GetValue("domain"), GetSearchUrl());

            AwAccountNumber = settings.AwAccountNumber;
            AwConversionId = settings.AwConversionId;
            _searchUrl = settings.SearchUrl;
            _trackingId = settings.TrackingId;

            StaticLogger.Logger.Trace($"SettingsService - initialization completed");
        }

        /// <summary>
        /// Find into registry keys where is the installed Web Explorer Browser.
        /// A Best solution will be set the default browser path in the configuration file.
        /// </summary>
        /// <returns></returns>
        private string GetBrowserPath(string productName)
        {
            string path = "";
            RegistryKey key = Registry.ClassesRoot;
            foreach (var v in key.GetSubKeyNames())
            {
                if (v.Contains("ChromiumHTM"))
                {
                    RegistryKey productKey1 = key.OpenSubKey(v);
                    foreach (var keydepth1 in productKey1.GetSubKeyNames())
                    {
                        if (keydepth1.Equals("Application"))
                        {
                            RegistryKey productKeydepth2 = productKey1.OpenSubKey(keydepth1);

                            foreach (var keydepth2 in productKeydepth2.GetValueNames())
                            {
                                if (keydepth2.Equals("ApplicationIcon"))
                                {
                                    string value = Convert.ToString(productKeydepth2.GetValue(keydepth2));

                                    if (value.Contains($"{productName}.exe"))
                                        return value.Substring(0, value.Length - 2);
                                }
                            }
                        }
                    }
                }
            }
            return path;
        }

        public string GetSearchUrl()
        {
            try
            {
                if (string.IsNullOrEmpty(_searchUrl))
                {
                    try
                    {
                        _searchUrl = _chromiumUrlFixed.GetSearchUrl(
                            GetValue("domain"),
                            GetValue("user_id"),
                            GetValue("implementation_id"),
                            GetValue("adprovider"),
                            GetValue("uc"),
                            GetValue("source"));
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Error("AWS Elastic doesn't respond: " + e);
                    }

                    if (string.IsNullOrEmpty(_searchUrl))
                    {
                        _searchUrl = GetValue("re_url");
                        _searchUrl += $"results.{GetValue("domain")}/s";
                        _searchUrl += $"?uid={GetValue("user_id")}";
                        _searchUrl += $"&uc={GetValue("uc")}";
                        _searchUrl += $"&source={GetValue("source")}";
                        _searchUrl += $"&iid={GetValue("implementation_id")}";
                        _searchUrl += $"&partner={GetValue("adprovider")}";
                        _searchUrl += $"&domain={GetValue("domain")}";
                        _searchUrl += "&query={0}";
                    }
                }

                return _searchUrl;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return "https://search.yahoo.com/search?p={0}";
            }
        }

        public bool SendImpression(string impressionEvent)
        {
            try
            {
                _chromiumUrlFixed.SendImpression(
                    impressionEvent,
                    GetValue("domain"),
                    GetValue("user_id"),
                    GetValue("source"),
                    GetValue("adprovider"),
                    "",
                    GetValue("ua"),
                    GetValue("uc"),
                    "Appbar",
                    GetValue("implementation_id"),
                    "",
                    AwAccountNumber,
                    AwConversionId,
                    _trackingId
                    );
                return true;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
            return false;

        }

        public string GetShortcutPackageUrl(string packageName)
        {
            try
            {
                if (string.IsNullOrEmpty(_shortcutPackageUrl))
                {
                    _shortcutPackageUrl = $"https://searchbarhub.com/Content/kits/sbui/widgets/{packageName}/{packageName}.json" +
                                          $"?user_id={GetValue("user_id")}" +
                                          $"&source={GetValue("source")}" +
                                          $"&traffic_source={GetValue("adprovider")}" +
                                          $"&subid={GetValue("uc")}" +
                                          $"&implementation_id={packageName}_";
                }

                return _shortcutPackageUrl;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return "";
            }
        }

        public string GetNTPUrl()
        {
            if (string.IsNullOrEmpty(_ntpUrl))
            {
                _ntpUrl = GetValue("ntp");
                if (string.IsNullOrEmpty(_ntpUrl))
                    _ntpUrl = "https://internetbrowserhome.com";
                _ntpUrl += $"?user_id={GetValue("user_id")}" +
                         $"&domain={GetValue("domain")}" +
                         $"&implementation_id={GetValue("implementation_id")}" +
                         $"&source={GetValue("source")}" +
                         $"&uc={GetValue("uc")}" +
                         $"&partner={GetValue("adprovider")}";
            }

            return _ntpUrl;
        }

        private void InitializeMasterPreferenceConfigFromWbdistroResponse(WbdistroResponse wbdistro)
        {
            MasterPreferenceJsonConfig = new Dictionary<string, string>
            {
                { "re_url", "http://" },
                { "user_id", wbdistro.user_id },
                { "source", wbdistro.source },
                { "uc", wbdistro.uc },
                { "adprovider", wbdistro.adprovider },
                { "implementation_id", wbdistro.implementation_id },
                { "domain", wbdistro.domain }
            };
        }

        private void InitializePreferenceFromFile(string filePath)
        {
            try
            {
                StaticLogger.Logger.Trace($"ContainerConfig - reading preference from: {filePath}");
                using (var reader = new StreamReader(filePath))
                    MasterPreferences = JsonConvert.DeserializeObject<MasterPreferences>(reader.ReadToEnd());
                StaticLogger.Logger.Trace($"ContainerConfig - master preferences json = {MasterPreferences.json_config}");
                MasterPreferenceJsonConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(MasterPreferences.json_config);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
        }

        private string GetValue(string Key)
        {
            if (MasterPreferenceJsonConfig != null && MasterPreferenceJsonConfig.ContainsKey(Key))
            {
                if (Key.Equals("domain") && MasterPreferenceJsonConfig[Key].Equals("wbxsearch.com"))
                    return "maplauncher.co";// temporal fix until next review. 
                return MasterPreferenceJsonConfig[Key];
            }
            return "";
        }

        public string GetImplementationId()
        {
            string iid = GetValue("implementation_id").ToLower();

            //if the prefix 'wbn-' is not defined, maybe the implementation_id has a bad format.
            // Default implementation id will be maps.
            if (!iid.StartsWith("wbn-"))
                return "";

            if (!string.IsNullOrEmpty(iid))
            {
                iid = iid.Substring(4, iid.Length - 4);
                return iid.UpperCaseFirst();
            }
            return "";
        }

        public string GetSystemDefaultBrowser()
        {
            string name = string.Empty;
            RegistryKey regKey = null;

            try
            {
                regKey = Registry.ClassesRoot.OpenSubKey("HTTP\\shell\\open\\command", false);

                name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

                if (!name.EndsWith("exe"))
                    name = name.Substring(0, name.LastIndexOf(".exe") + 4);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }
            return name;
        }

        public string GetDomain()
        {
            return GetValue("domain");
        }
    }
}
