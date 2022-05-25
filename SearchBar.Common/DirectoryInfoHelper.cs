using Common.Settings.Chromium;
using Common.String;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DirectoryInfoHelper
    {
        public static string GetCurrentAppRunningLocation()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static string GetMasterPreferenceFilePath()
        {
            return $"{GetCurrentAppRunningLocation()}\\{StringConstants.MasterPreferenceFileName}";
        }

        public static string GetLocalStateFilePath()
        {
            return $"{Directory.GetParent(GetCurrentAppRunningLocation()).FullName}\\{StringConstants.UserDataFolderName + "\\" + StringConstants.LocalStateFolderName}";
        }

        public static string GetAppSettingFilePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.AppSettingFileName}";
        }

        public static string GetWidgetFilePath(string widgetName)
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.Widget}\\{widgetName}\\{widgetName}.json";
        }

        public static string GetBookmarkStorePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{GetBookmarkStoreFilePath()}";
        }

        public static string GetHistoryStorePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{GetHistoryStoreFilePath()}";
        }

        public static string GetBookmarkPathToImportPath()
        {
            return Directory.GetParent(GetCurrentAppRunningLocation()).FullName + $"\\{StringConstants.UserDataFolderName}\\{StringConstants.Default}\\{StringConstants.Bookmarks}";
        }

        public static string GetFaviconDatabasePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.Favicons}";
        }

        public static string GetFaviconBrowserDatabasePath()
        {
            return Directory.GetParent(GetCurrentAppRunningLocation()).FullName + $"\\{StringConstants.UserDataFolderName}\\{StringConstants.Default}\\{StringConstants.Favicons}";
        }

        public static string GetHistoryChromiumDatabasePath()
        {
            return Directory.GetParent(GetCurrentAppRunningLocation()).FullName + $"\\{StringConstants.UserDataFolderName}\\{StringConstants.Default}\\{StringConstants.History}";
        }

        public static string GetHistoryDatabasePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.History}";
        }

        public static string GetMyDocumetsUserPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public static string GetPinShortcutFilePath()
        {
            return $"{ProducSettings.ProducName}.{StringConstants.PinShortCutFileName}.{StringConstants.StoreFileExtension}";
        }

        public static string GetBookmarkStoreFilePath()
        {
            return $"{ProducSettings.ProducName}.{StringConstants.Bookmark}.{StringConstants.StoreFileExtension}";
        }

        public static string GetHistoryStoreFilePath()
        {
            return $"{ProducSettings.ProducName}.{StringConstants.History}.{StringConstants.StoreFileExtension}";
        }

        public static string GetGamesFilePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.GameFileName}.{StringConstants.StoreFileExtension}";
        }

        public static string GetNotesFilePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.Notes}";
        }

        public static string GetUserLocationFilePath()
        {
            return GetCurrentAppRunningLocation() + $"\\{StringConstants.UserIpLocation}";
        }
    }
}
