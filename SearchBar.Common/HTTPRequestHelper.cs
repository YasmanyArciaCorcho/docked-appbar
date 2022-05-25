using HtmlAgilityPack;
using Common.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Common
{
    public static class HTTPRequestHelper
    {
        public static string DoQuery(string query)
        {
            try
            {
                string queryResult = "";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.ExpectContinue = false;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    Task.Run(async () => { queryResult = await client.GetStringAsync(query); }).Wait();
                }
                return queryResult;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
            return string.Empty;
        }

        public async static Task<string> DoAsyncQuery(string query)
        {
            try
            {
                string queryResult = "";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.ExpectContinue = false;
                    queryResult = await client.GetStringAsync(query);
                }
                return queryResult;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
            return string.Empty;
        }

        public static string DoQuery(string query, IEnumerable<KeyValuePair<string, string>> headers)
        {
            try
            {
                using WebClient client = new WebClient();
                foreach (var header in headers)
                {
                    client.Headers.Add(header.Key, header.Value);
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                return client.DownloadString(query);
            }
            catch (Exception ex)
            {
                StaticLogger.Logger.Error(ex);
            }
            return string.Empty;
        }

        public static bool UrlExists(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "HEAD";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                using HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return false;
            }
        }

        private static Task<bool> UrlExistsAsync(string url)
        {
            return Task.Run<bool>((Func<bool>)(() => UrlExists(url)));
        }

        public static string GetDomainUrl(string url)
        {
            try
            {
                return GetDomainUrl(new UriBuilder(url).Uri);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return null;
            }
        }

        public static string GetDomainUrl(Uri uri)
        {
            try
            {
                return string.Format("{0}://{1}", uri.Scheme == "https" ? "https" : "http", uri.Host);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return null;
            }
        }

        public static byte[] GetFileFromURL(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                using WebClient client = new WebClient();

                return client.DownloadData(url);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return null;
            }
        }

        public static async Task<byte[]> GetFileFromURLAsync(string url)
        {
            try
            {
                using WebClient client = new WebClient();
                return await client.DownloadDataTaskAsync(new Uri(url));
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return null;
            }
        }

        private static string BuildAttributeUrlIcon(string attribute, string domain)
        {
            if (!attribute.StartsWith("http"))
            {
                return domain + attribute;
            }

            return attribute;
        }

        public static string GetFaviconUrl(string url)
        {
            Uri uri = new UriBuilder(url).Uri;

            string domain = GetDomainUrl(url);
            (HtmlDocument htmlDocument, _) = GroupedNodes(string.Format("{0}://{1}", uri.Scheme == "https" ? (object)"https" : (object)"http", (object)uri.Host));
            string faviconUrl = GetFaviconUrl(htmlDocument, domain);

            if (string.IsNullOrEmpty(faviconUrl))
            {
                if (uri.HostNameType == UriHostNameType.Dns)
                {
                    string _rootFavicon = string.Format("{0}/favicon.ico", domain);
                    if (UrlExists(_rootFavicon))
                        return _rootFavicon;
                }
            }
            return faviconUrl;
        }

        public static async Task<string> GetFaviconUrlAsync(string url)
        {
            Uri uri = new UriBuilder(url).Uri;

            string domain = GetDomainUrl(url);
            (HtmlDocument htmlDocument, _) = await GroupedNodesAsync(string.Format("{0}://{1}", uri.Scheme == "https" ? (object)"https" : (object)"http", (object)uri.Host));
            string faviconUrl = GetFaviconUrl(htmlDocument, domain);

            if (string.IsNullOrEmpty(faviconUrl))
            {
                if (uri.HostNameType == UriHostNameType.Dns)
                {
                    string _rootFavicon = string.Format("{0}/favicon.ico", domain);
                    if (await UrlExistsAsync(_rootFavicon))
                        return _rootFavicon;
                }
            }
            return faviconUrl;
        }

        public static byte[] UploadFile(string url, string fileToUploadPath)
        {
            return UploadFile(url, fileToUploadPath, new Dictionary<string, string>() { });
        }

        public static byte[] UploadFile(string url, string fileToUploadPath, Dictionary<string, string> headers)
        {
            try
            {
                using var client = new WebClient();
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.Headers.Add(header.Key, header.Value);
                    }
                }

                return client.UploadFile(new Uri(url), fileToUploadPath);
            }
            catch (WebException e)
            {
                StaticLogger.Logger.Error(e);
                return null;
            }
        }

        private static string GetFaviconUrl(HtmlDocument htmlDocument, string domain)
        {
            string _returnFavicon = null;

            HtmlNodeCollection source1 = htmlDocument.DocumentNode.SelectNodes("//link[contains(@rel, 'apple-touch-icon')]");
            if (source1 != null && source1.Any<HtmlNode>())
            {
                string attributeValue = source1.First<HtmlNode>().GetAttributeValue("href", null);
                if (!string.IsNullOrWhiteSpace(attributeValue))
                    return BuildAttributeUrlIcon(attributeValue, domain);
            }
            HtmlNodeCollection source2 = htmlDocument.DocumentNode.SelectNodes("//link[contains(@rel, 'icon')]");
            if (source2 != null && source2.Any<HtmlNode>())
            {
                string attributeValue = source2.First<HtmlNode>().GetAttributeValue("href", null);
                if (!string.IsNullOrWhiteSpace(attributeValue))
                    return BuildAttributeUrlIcon(attributeValue, domain);
            }
            HtmlNodeCollection source3 = htmlDocument.DocumentNode.SelectNodes("//link[contains(@rel, 'shortcut icon')]");
            if (source3 != null && source3.Any<HtmlNode>())
            {
                string attributeValue = source3.First<HtmlNode>().GetAttributeValue("href", null);
                if (!string.IsNullOrWhiteSpace(attributeValue))
                    return BuildAttributeUrlIcon(attributeValue, domain);
            }

            return _returnFavicon;
        }

        private static Task<(HtmlDocument, Uri)> GroupedNodesAsync(string URL)
        {
            return Task.Run<(HtmlDocument, Uri)>((Func<(HtmlDocument, Uri)>)(() => GroupedNodes(URL)));
        }

        private static (HtmlDocument, Uri) GroupedNodes(string url)
        {
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb
                {
                    PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest)
                };
                HtmlDocument htmlDocument = htmlWeb.Load(url);
                return (htmlDocument, htmlWeb.ResponseUri);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return (null, null);
            }
        }

        private static bool OnPreRequest(HttpWebRequest request)
        {
            request.AllowAutoRedirect = true;
            return true;
        }
    }
}
