using Common;
using Common.Logger;
using Common.Settings.Chromium;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings.Chromium.AWSElastic
{
    public class AWSElasticAppSettingsInitializer : IAppSettingsInitializer
    {
        readonly string _cacheUrlTemplate = "https://api.{0}/cache/ut/?tid={1}";
        readonly string settingsFilePath;

        public AWSElasticAppSettingsInitializer()
        {
            settingsFilePath = DirectoryInfoHelper.GetAppSettingFilePath();
        }

        public AppSettings Initialize(out bool firstRun)
        {
            AppSettings appSettings = new AppSettings();
            if (File.Exists(settingsFilePath))
            {
                using var reader = new StreamReader(settingsFilePath);
                appSettings = JsonConvert.DeserializeObject<AppSettings>(reader.ReadToEnd());
                if (appSettings == null)
                    appSettings = new AppSettings();
            }

            firstRun = appSettings.FirstRun;
            return appSettings;
        }

        public void UpdateAppSettings(AppSettings appSettings, params object[] parameters)
        {
            appSettings.FirstRun = false;

            if (!string.IsNullOrEmpty(appSettings.TrackingId) && parameters.Length > 0)
            {
                try
                {
                    string queryResult = HTTPRequestHelper.DoQuery(string.Format(_cacheUrlTemplate, parameters[0], appSettings.TrackingId),
                        new List<KeyValuePair<string, string>>()
                    {
                            AWSElasticSettings.AWSElasticApiKey,
                            AWSElasticSettings.AWSElasticName
                    });
                    AWSElasticCacheModelAPI cacheResult = JsonConvert.DeserializeObject<AWSElasticCacheModelAPI>(queryResult);
                    appSettings.AwAccountNumber = cacheResult.AwAccountNumber;
                    appSettings.AwConversionId = cacheResult.AwConversionId;
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Trace($"AppSettingsInitializer - {e}.");
                }

                if (parameters.Length > 1)
                {
                    appSettings.SearchUrl = parameters[1] as string;
                }
            }

            File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(appSettings));
        }
    }
}
