using Common;
using Common.ExtensionMethods;
using Common.Logger;
using Common.Models;
using Dapper;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Stores.Providers.FaviconStore
{
    public class FaviconSqlLiteStore : IFaviconStore
    {
        readonly SortedList<string, FaviconModel> _memoryCache;
        static string _connectionString = "Data Source={0};Version=3;";
        private readonly object _favIconChromiumLock = new object();

        public FaviconSqlLiteStore(IEnumerable<Bookmark> bookmarks)
        {
            string browserFavDbPath = DirectoryInfoHelper.GetCurrentAppRunningLocation() + "\\" + DirectoryInfoHelper.GetFaviconBrowserDatabasePath();

            _connectionString = string.Format(_connectionString, DirectoryInfoHelper.GetFaviconDatabasePath());

            if (File.Exists(browserFavDbPath))
            {
                ExistsFaviconBrowserDb = false;
            }

            _memoryCache = new SortedList<string, FaviconModel>();

            if (!File.Exists(DirectoryInfoHelper.GetFaviconDatabasePath()))
            {
                try
                {
                    if (ExistsFaviconBrowserDb)
                    {
                        File.Copy(browserFavDbPath, DirectoryInfoHelper.GetFaviconDatabasePath(), true);
                    }
                    else
                    {
                        CreateFaviconDatabase();
                    }
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error(e);
                }
            }

            InitializeMemoryCache(_connectionString, bookmarks);
        }

        public bool ExistsFaviconBrowserDb
        { get; private set; }

        public void AddFavicon(string pageUrl, string faviconUrl, byte[] content)
        {
            AddFavicon(pageUrl, new FaviconModel() { PageUrl = faviconUrl, Content = content });
        }

        public void AddFavicon(string pageUrl, FaviconModel favoicon)
        {
            lock (_favIconChromiumLock)
            {
                using IDbConnection cnn = new SQLiteConnection(_connectionString);
                int faviconId = ExistsFavIcon(favoicon.PageUrl);

                string sqlStatement = "";

                if (faviconId == 0)
                {
                    sqlStatement = $"insert into favicons(url, icon_type) values('{favoicon.PageUrl}', 1) ";

                    cnn.Execute(sqlStatement);

                    faviconId = ExistsFavIcon(favoicon.PageUrl);

                    sqlStatement = $"insert into favicon_bitmaps (icon_id, last_updated, image_data) values({faviconId}, {DateTime.Now.ToUnixTime()}, @Content) ";

                    cnn.Execute(sqlStatement, favoicon);
                }

                AddFavoiconToPageUrl(pageUrl, faviconId, cnn);
            }
        }

        public void AddFavoiconToPageUrl(string pageUrl, int favoiconId)
        {
            lock (_favIconChromiumLock)
            {
                using IDbConnection cnn = new SQLiteConnection(_connectionString);
                AddFavoiconToPageUrl(pageUrl, favoiconId, cnn);
            }
        }

        public void AddFavoiconToPageUrl(string pageUrl, int favoiconId, IDbConnection cnn)
        {
            try
            {
                string sqlStatement = $"insert into icon_mapping (page_url, icon_id) values ('{pageUrl}', {favoiconId})";

                cnn.Execute(sqlStatement);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error($"FaviconStore - {e}");
            }
        }

        public int ExistsFavIcon(string favoiconUrl)
        {
            int result = 0;

            using (IDbConnection cnn = new SQLiteConnection(_connectionString))
            {
                result = ExistsFavIcon(favoiconUrl, cnn);
            }

            return result;
        }

        public int ExistsFavIcon(string favoiconUrl, IDbConnection cnn)
        {
            try
            {
                string sqlStatement = $"select id from favicons where favicons.url = '{favoiconUrl}' ";

                int faviconId = cnn.QuerySingle<int>(sqlStatement);
                return faviconId;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return 0;
            }
        }

        public void RemoveFavicon(string url)
        {
            lock (_favIconChromiumLock)
            {
                using IDbConnection cnn = new SQLiteConnection(_connectionString);
                cnn.Execute($"delete from favicon where domain_url = {url}", new DynamicParameters());
            }
        }

        /// <summary>
        /// imageName should be like imageName1, imangeName2, ... imageNameN.
        /// </summary>
        /// <param name="imageNames"></param>
        public void RemoveFaviconRange(string urls)
        {
            lock (_favIconChromiumLock)
            {
                using IDbConnection cnn = new SQLiteConnection(_connectionString);
                cnn.Execute($"delete from favicon where domain_url in (%s)", urls);
            }
        }

        public FaviconModel GetFavicon(string pageUrl)
        {
            if (_memoryCache.ContainsKey(pageUrl))
                return _memoryCache[pageUrl];

            lock (_favIconChromiumLock)
            {
                using IDbConnection cnn = new SQLiteConnection(_connectionString);
                return GetFavicon(pageUrl, cnn);
            }
        }

        public FaviconModel GetFavicon(string pageUrl, IDbConnection cnn)
        {
            try
            {
                string sqlStatement = string.Format("select image_data as Content, icon_mapping.page_url as PageUrl, favicons.url as FaviconUrl " +
                                                          " from favicon_bitmaps  " +
                                                          "join favicons on favicons.id = favicon_bitmaps.icon_id  " +
                                                          "join icon_mapping on favicons.id = icon_mapping.icon_id  " +
                                                          "where icon_mapping.page_url = '{0}'", pageUrl);

                List<FaviconModel> output = cnn.Query<FaviconModel>(sqlStatement, new DynamicParameters()).ToList();
                if (output != null && output.Count > 0)
                    return output[0];
            }
            catch
            {
            }

            return null;
        }

        public FaviconModel GetFaviconByFaviconURL(string faviconUrl)
        {
            try
            {
                using IDbConnection cnn = new SQLiteConnection(_connectionString);
                string sqlStatement = string.Format("select image_data as Content from favicon_bitmaps  " +
                                                      "join favicons on favicons.id = favicon_bitmaps.icon_id  " +
                                                      "where favicons.url == '{0}'", faviconUrl);

                List<FaviconModel> output = cnn.Query<FaviconModel>(sqlStatement, new DynamicParameters()).ToList();
                if (output != null && output.Count > 0)
                    return output[0];
            }
            catch
            {
            }

            return null;
        }

        public IEnumerable<FaviconModel> GetFavIcons(IEnumerable<string> pagesUrls)
        {
            List<FaviconModel> output = null;

            try
            {
                lock (_favIconChromiumLock)
                {
                    using IDbConnection cnn = new SQLiteConnection(_connectionString);
                    string set = pagesUrls.ToList().JoinList("(", ", ", ")");

                    string sqlStatement = string.Format("select image_data as Content, icon_mapping.page_url as PageUrl, favicons.url as FaviconUrl " +
                                                          " from favicon_bitmaps  " +
                                                          "join favicons on favicons.id = favicon_bitmaps.icon_id  " +
                                                          "join icon_mapping on favicons.id = icon_mapping.icon_id  " +
                                                          "where icon_mapping.page_url in {0}", set);

                    output = cnn.Query<FaviconModel>(sqlStatement, new DynamicParameters()).ToList();
                }

                if (output != null)
                    return output;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }

            return new List<FaviconModel>();
        }

        private void CreateFaviconDatabase()
        {
            StaticLogger.Logger.Info("Favicon database does not exists, Creating a new Database.");

            string dbPath = DirectoryInfoHelper.GetFaviconDatabasePath();

            SQLiteConnection.CreateFile(dbPath);

            using IDbConnection cnn = new SQLiteConnection(_connectionString);
            string query = "BEGIN TRANSACTION; " +
                            "CREATE TABLE IF NOT EXISTS icon_mapping" +
                            "(" +
                            "id INTEGER PRIMARY KEY," +
                            "page_url LONGVARCHAR NOT NULL," +
                            "icon_id INTEGER" +
                            "); " +
                            "CREATE TABLE IF NOT EXISTS favicons" +
                            "(" +
                            "id INTEGER PRIMARY KEY," +
                            "url LONGVARCHAR NOT NULL," +
                            "icon_type INTEGER DEFAULT 1" +
                            "); " +
                            "CREATE TABLE IF NOT EXISTS favicon_bitmaps" +
                            "(" +
                            "id INTEGER PRIMARY KEY," +
                            "last_updated INTEGER DEFAULT 0," +
                            "icon_id INTEGER NOT NULL," +
                            "image_data BLOB," +
                            "width INTEGER DEFAULT 0," +
                            "height INTEGER DEFAULT 0," +
                            "last_requested INTEGER DEFAULT 0" +
                            "); " +
                            "COMMIT;";

            try
            {
                cnn.Execute(query);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
        }

        private void InitializeMemoryCache(string connectionId, IEnumerable<Bookmark> bookmarks)
        {
            StaticLogger.Logger.Info($"Read bookmarks to add in favicon db.");

            List<string> pagesUrl = new List<string>();

            foreach (var bookmark in bookmarks)
            {
                if (!string.IsNullOrEmpty(bookmark.Url))
                    pagesUrl.Add(bookmark.Url);
            }

            StaticLogger.Logger.Info($"Read favicon to memory from {connectionId}.");

            foreach (var favicon in GetFavIcons(pagesUrl))
            {
                if (!_memoryCache.ContainsKey(favicon.PageUrl))
                    _memoryCache.Add(favicon.PageUrl, favicon);
            }
        }
    }
}
