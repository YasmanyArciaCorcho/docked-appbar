using Common;
using Common.Logger;
using Dapper;
using Newtonsoft.Json;
using Common.ChromiumSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stores.Providers.Image.Assets.AWS
{
    public class AssetsASWImageProvider : AssetsImageProvider
    {
        readonly string _imageModuleUrl;
        readonly string _imageUrl;

        public AssetsASWImageProvider(IChromiumSettingsService settingsService)
        {
            string domain = settingsService.GetDomain();
            _imageModuleUrl = "https://api." + domain + "/img/n/contains/{0}";
            _imageUrl = "https://api." + domain + "/img/n/{0}";
        }

        public override string GetImagePath(string imageNamespace)
        {
            string path = base.GetImagePath(imageNamespace);

            if (!File.Exists(path))
            {
                try
                {
                    string query = string.Format(_imageUrl, imageNamespace);
                    string result = HTTPRequestHelper.DoQuery(query);
                    //Temporal fix.
                    // the endpoint is returning something like this '<svg>..<sgv>' and should be without ''
                    if (result != null && !result.Equals("null"))
                    {
                        string newString = result.Substring(1, result.Length - 2);
                        newString = newString.Replace("\\\"", "\"");
                        newString = newString.Replace("\\n", "");
                        WriteFile(path, newString);
                    }
                }
                catch (Exception ex)
                {
                    StaticLogger.Logger.Error($"Fatal error loading images: {ex}");
                }
            }

            return path;
        }

        public override async Task AddImageModuleSource(string moduleNamespace)
        {
            string modulePath = GetModulePath(moduleNamespace);
            if (!Directory.Exists(modulePath) || Directory.EnumerateFiles(modulePath).Count() <= 1)
            {
                try
                {
                    string query = string.Format(_imageModuleUrl, moduleNamespace);
                    string result = await HTTPRequestHelper.DoAsyncQuery(query);
                    List<AWSImageModelAPI> imagesData = JsonConvert.DeserializeObject<List<AWSImageModelAPI>>(result);
                    if (imagesData != null)
                    {
                        foreach (var imageData in imagesData)
                        {
                            WriteFile(base.GetImagePath(imageData.Namespace), imageData.Data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StaticLogger.Logger.Error($"Fatal error loading images: {ex}");
                }
            }
        }

        private void WriteFile(string filePath, string fileData)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(filePath, fileData);
        }
    }
}
