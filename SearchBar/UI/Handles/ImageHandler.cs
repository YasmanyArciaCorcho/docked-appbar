using Store;
using Microsoft.Win32;
using System;
using System.IO;
using System.Management;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Store.Models;
using Common;
using Stores.Providers.FaviconStore;

namespace SearchBar.UI.Handles
{
    public static class ImageHandler
    {
        public static FaviconSqlLiteStore FaviconStore
        { get; set; }

        public delegate void WallPaperChanged(string windowsWallPaperPath);
        public static event WallPaperChanged WallPaperChangedEvent;

        const string _registryKey = @"Control Panel\\Desktop";
        const string _wallPaperRegistry = "WallPaper";

        static readonly string _urlImageDashboard = "pack://application:,,,/Assets/";
        private static readonly string defaultIBookmarkIconPath = "earth.png";
        const string defaultIBookmarkFolderIconPath = "folder.png";

        public static bool IsWachingRegistry
        { get; private set; }

        public static string GetWallPaperImagePath()
        {
            var registry = Registry.CurrentUser.OpenSubKey(_registryKey);
          
            StartRegistryEventWatcher();

            return registry.GetValue(_wallPaperRegistry).ToString();
        }

        public static void SetBlurImageInBackground(Window window)
        {
            var blur = new BlurEffect();
            var current = window.Background;
            blur.Radius = 5;
            window.Background = new SolidColorBrush(Colors.DarkGray);
            window.Effect = blur;
            window.Effect = null;
            window.Background = current;
        }

        public static void StartRegistryEventWatcher()
        {
            if (!IsWachingRegistry)
            {
                IsWachingRegistry = true;

                var currentUser = WindowsIdentity.GetCurrent();
                var query = new WqlEventQuery(string.Format(
                "SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{0}\\\\{1}' AND ValueName='{2}'",
                currentUser.User.Value, _registryKey.Replace("\\", "\\\\"), _wallPaperRegistry));

                using ManagementEventWatcher watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += new EventArrivedEventHandler(RegistryEventWatcherHandleEvent);
                watcher.Start();
            }
        }

        public static void RegistryEventWatcherHandleEvent(object sender, EventArrivedEventArgs e)
        {
            WallPaperChangedEvent(GetWallPaperImagePath());
        }

        public static BitmapImage GetBitmapFromLocalSource(string localPath)
        {
            return new BitmapImage(new Uri(localPath));
        }

        public async static Task<BitmapImage> GetBitmapFromWebAsync(string url)
        {
            BitmapImage bitmapImage = FindFaviconInCache(url);

            if (bitmapImage != null)
                return bitmapImage;

            await Task.Yield();

            string faviconUrl = await HTTPRequestHelper.GetFaviconUrlAsync(url);

            int favoiconId = ExistsFaviconInCache(faviconUrl);

            if (favoiconId != 0)
            {
                AddFavoiconToPageUrl(url, favoiconId);

                bitmapImage = FindFaviconInCache(url);

                return bitmapImage;
            }
            else
            {
                if (!string.IsNullOrEmpty(faviconUrl))
                {
                    BitmapImage bi;
                    byte[] imageIcon = await HTTPRequestHelper.GetFileFromURLAsync(faviconUrl);

                    bi = BytesToImage(imageIcon);

                    SaveFavoiconInCache(url, faviconUrl, imageIcon);
                    return bi;
                }
            }

            return null;
        }

        public static BitmapImage GetBitmapFromWeb(string url)
        {
            BitmapImage bitmapImage = FindFaviconInCache(url);

            if (bitmapImage != null)
                return bitmapImage;

            string faviconUrl = HTTPRequestHelper.GetFaviconUrl(url);

            int favoiconId = ExistsFaviconInCache(faviconUrl);

            if (favoiconId != 0)
            {
                AddFavoiconToPageUrl(url, favoiconId);

                bitmapImage = FindFaviconInCache(url);

                return bitmapImage;
            }
            else
            {
                if (!string.IsNullOrEmpty(faviconUrl))
                {
                    BitmapImage bi;
                    byte[] imageIcon = HTTPRequestHelper.GetFileFromURL(faviconUrl);

                    bi = BytesToImage(imageIcon);

                    SaveFavoiconInCache(url, faviconUrl, imageIcon);
                    return bi;
                }
            }

            return null;
        }

        public static string BuildImageUrlFromPack(string suffix)
        {
            return _urlImageDashboard + suffix;
        }

        public static BitmapImage BuildImageFromPack(string suffix)
        {
            return GetBitmapFromLocalSource(BuildImageUrlFromPack(suffix));
        }

        public static BitmapImage GetDefaultFolderImage()
        {
            return GetBitmapFromLocalSource(BuildImageUrlFromPack(defaultIBookmarkFolderIconPath));
        }

        public static BitmapImage GetDefaultShortcutImage()
        {
            return GetBitmapFromLocalSource(BuildImageUrlFromPack(defaultIBookmarkIconPath));
        }

        private static BitmapImage FindFaviconInCache(string pageUrl)
        {
            FaviconModel faviconModel = FaviconStore.GetFavicon(pageUrl);

            if (faviconModel != null)
                return BytesToImage(faviconModel.Content);

            return null;
        }

        private static void SaveFavoiconInCache(string pageUrl, string faviconUrl, byte[] image)
        {
            FaviconStore.AddFavicon(pageUrl, faviconUrl, image);
        }

        private static void AddFavoiconToPageUrl(string pageUrl, int faviconId)
        {
            FaviconStore.AddFavoiconToPageUrl(pageUrl, faviconId);
        }

        private static int ExistsFaviconInCache(string faviconUrl)
        {
            return FaviconStore.ExistsFavIcon(faviconUrl);
        }

        public static BitmapImage BytesToImage(byte[] array)
        {
            using var ms = new System.IO.MemoryStream(array);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static byte[] BitmapImageByteArray(BitmapImage bitmapImage)
        {
            Stream stream = bitmapImage.StreamSource;
            Byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using BinaryReader br = new BinaryReader(stream);
                buffer = br.ReadBytes((Int32)stream.Length);
            }

            return buffer;
        }
    }
}
