using Common;
using Common.ExtensionMethods;
using Common.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bookmark.Importer
{
    /// <summary>
    /// Import bookmark from our browser.
    /// Read bookmark data from /User Data/Default/Bookmarks
    /// </summary>
    public class JsonBookmarkImporter : IBookmarkImporter
    {
        public List<Common.Models.Bookmark> ImportBookmarks()
        {
            List<Common.Models.Bookmark> importedBookmark = new List<Common.Models.Bookmark>();

            string bookmarksToImportPath = DirectoryInfoHelper.GetBookmarkPathToImportPath();
            if (File.Exists(bookmarksToImportPath))
            {
                StaticLogger.Logger.Info($"Importing bookmarks from {bookmarksToImportPath}");

                using var reader = new StreamReader(bookmarksToImportPath);
                BookmarkImporterModelAPI bookmarks = JsonConvert.DeserializeObject<BookmarkImporterModelAPI>(reader.ReadToEnd());
                foreach (var child in bookmarks.roots.bookmark_bar.children)
                {
                    Common.Models.Bookmark childBookmark = CreateBookmarkTree(child, null);
                    if (childBookmark != null)
                        importedBookmark.Add(childBookmark);
                }
            }

            StaticLogger.Logger.Info($"Bookmarks has been imported.");

            return importedBookmark;
        }

        private Common.Models.Bookmark CreateBookmarkTree(Child child, Common.Models.Bookmark parent)
        {
            Common.Models.Bookmark result = new Common.Models.Bookmark(parent, child.name, child.url, child.type.Equals("url") ? false : true);

            if (result.IsForder)
            {
                foreach (var grandchild in child.children)
                {
                    Common.Models.Bookmark grandchildBookmark = CreateBookmarkTree(grandchild, result);

                    if (grandchildBookmark != null)
                        result.Children.Add(grandchildBookmark);
                }
            }
            else if (string.IsNullOrEmpty(result.Url)                 // Means its a chrome quick shortcut like: chrome://settings
                || !result.Url.ContainsHTTPPreffix())     // No way to open this type of link found from search bar.
                return null;

            return result;
        }
    }
}
