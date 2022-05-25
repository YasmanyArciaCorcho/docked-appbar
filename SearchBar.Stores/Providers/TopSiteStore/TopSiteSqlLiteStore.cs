using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Store;
using Store.Models;
using Common;
using Common.Logger;

namespace Stores.Providers.TopSiteStore
{
    public class TopSiteSqlLiteStore : ITopSiteStore
    {
        public IEnumerable<TopSiteModel> GetTopSites()
        {
            List<TopSiteModel> topSites = new List<TopSiteModel>();

            string chromiumHistoryDbPath = DirectoryInfoHelper.GetHistoryChromiumDatabasePath();

            if (File.Exists(chromiumHistoryDbPath))
            {
                File.Copy(chromiumHistoryDbPath, DirectoryInfoHelper.GetHistoryDatabasePath(), true);

                try
                {
                    using IDbConnection cnn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DirectoryInfoHelper.GetHistoryDatabasePath()));
                    string sqlStatement = string.Format("select url as Url, visit_count as UrlRank, title as Title from urls order by visit_count desc");

                    topSites = cnn.Query<TopSiteModel>(sqlStatement, new DynamicParameters()).Take(15).ToList();
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error($"Fatal error loading Top site: {e}");
                }
            }

            return topSites;
        }
    }
}
