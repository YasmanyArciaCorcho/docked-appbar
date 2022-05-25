using Common;
using Common.Models;
using Common.Logger;
using Common.Settings.Chromium;
using Common.String;
using Newtonsoft.Json;
using Services.Bookmark.Importer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Stores.Providers;
using Stores.Providers.BookmarkProvider;
using Common.ChromiumSettings;

namespace Services.Bookmark
{
    public class BookmarkService : BookmarkProvider, IBookmarkService
    {
        public Dictionary<string, Common.Models.Bookmark> Bookmarks
        { get; set; }

        public string BookmarkJoinPathString
        { get; private set; }

        private string _bookmarkStorePath;

        public BookmarkService(IChromiumSettingsService settingsService, IBookmarkImporter bookmarkImporter) : base()
        {
            BookmarkJoinPathString = StringConstants.BookmarkJoinPath;

            if (settingsService.FirstRun && bookmarkImporter != null)
                ImportBookmark(bookmarkImporter);
        }

        public IEnumerable<Common.Models.Bookmark> GetBookmarkRoots()
        {
            foreach (var bookmark in Bookmarks.Values)
            {
                if (bookmark.Parent == null)
                    yield return bookmark;
            }
        }

        public Common.Models.Bookmark AddBookmarkPage(Common.Models.Bookmark parent, string name, string url)
        => AddBookmark(parent, name, url);

        public Common.Models.Bookmark EditBookmarkPage(string oldName, string newName, string newUrl)
        => EditBookmark(oldName, newName, newUrl);

        public Common.Models.Bookmark AddBookmarkFolder(Common.Models.Bookmark parent, string name)
         => AddBookmark(parent, name, null, true);

        public Common.Models.Bookmark EditBookmarkFolder(string oldfolderPath, string newName)
        => EditBookmark(oldfolderPath, newName, null);

        public Common.Models.Bookmark RemoveBookmark(string bookmarkPath)
        {
            if (Contains(bookmarkPath))
            {
                Common.Models.Bookmark toRemove = Bookmarks[bookmarkPath];

                if (toRemove.Parent != null)
                    toRemove.Parent.Children.Remove(toRemove);

                RemoveBookmarkTree(toRemove, bookmarkPath);

                Save();

                return toRemove;
            }
            return null;
        }

        public async override Task<bool> Load()
        {
            Bookmarks = new Dictionary<string, Common.Models.Bookmark>();
            _bookmarkStorePath = DirectoryInfoHelper.GetBookmarkStorePath();
            BookmarkJoinPathString = StringConstants.BookmarkJoinPath;
            try
            {
                if (File.Exists(_bookmarkStorePath))
                {
                    await Task.Yield();

                    List<Common.Models.Bookmark> readBookmarks = JsonConvert.DeserializeObject<List<Common.Models.Bookmark>>(File.ReadAllText(_bookmarkStorePath));
                    foreach (var bookmark in readBookmarks)
                    {
                        AddBookmarkTree(bookmark, null);
                    }
                    StaticLogger.Logger.Info($"BookmarkService - load Bookmarkfrom disk.");
                }
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return false;
            }

            return true;
        }

        public override Task<bool> Save()
        {
            return Task.Run(() =>
             {
                 lock (_saveLock)
                 {
                     try
                     {
                         if (Bookmarks != null && Bookmarks.Count != 0)
                         {
                             List<Common.Models.Bookmark> toSave = new List<Common.Models.Bookmark>();

                             foreach (var pairBookmark in Bookmarks)
                             {
                                 if (pairBookmark.Value.Parent == null)
                                     toSave.Add(pairBookmark.Value);
                             }

                             string serializedBookmars = JsonConvert.SerializeObject(toSave);
                             File.WriteAllText(_bookmarkStorePath, serializedBookmars);
                         }
                     }
                     catch (Exception e)
                     {
                         StaticLogger.Logger.Error(e);
                         return false;
                     }

                     return true;
                 }
             });
        }

        public Common.Models.Bookmark FindBookmark(string bookmarkPath)
        {
            if (Contains(bookmarkPath))
                return Bookmarks[bookmarkPath];
            return null;
        }

        public bool Contains(string bookmarkPath)
        {
            if (string.IsNullOrEmpty(bookmarkPath))
                return true;
            return Bookmarks.ContainsKey(bookmarkPath);
        }

        private Common.Models.Bookmark AddBookmark(Common.Models.Bookmark parent, string name, string url, bool isFolder = false)
        {
            while (parent != null && !parent.IsForder)
                parent = parent.Parent;

            string newBookmarkPath = name;
            if (parent != null)
                newBookmarkPath = GetBookmarkPath(parent) + BookmarkJoinPathString + name;

            if (!Contains(newBookmarkPath))
            {
                Common.Models.Bookmark Bookmark = new Common.Models.Bookmark(parent, name, url, isFolder);

                Bookmarks.Add(newBookmarkPath, Bookmark);

                if (parent != null)
                    parent.Children.Add(Bookmark);

                Save();

                StaticLogger.Logger.Info($"BookmarkService - Bookmarkadded: {name}");

                return Bookmark;
            }
            return null;
        }

        private Common.Models.Bookmark EditBookmark(string oldIBookmarkPath, string newName, string newUrl)
        {
            if (Contains(oldIBookmarkPath))
            {
                Common.Models.Bookmark editBookmark = FindBookmark(oldIBookmarkPath);

                Bookmarks.Remove(oldIBookmarkPath);

                editBookmark.Name = newName;
                editBookmark.Url = newUrl;

                Bookmarks.Add(GetBookmarkPath(editBookmark), editBookmark as Common.Models.Bookmark);

                Save();

                StaticLogger.Logger.Info($"BookmarkService - Bookmarkedited: {oldIBookmarkPath}");

                return editBookmark;
            }
            return null;
        }

        public string GetBookmarkPath(Common.Models.Bookmark bookmark)
        {
            StringBuilder path = new StringBuilder(bookmark.Name);

            var currentBookmark = bookmark;
            while (currentBookmark.Parent != null)
            {
                currentBookmark = currentBookmark.Parent;
                path.Insert(0, $"{currentBookmark.Name}{BookmarkJoinPathString}");
            }

            return path.ToString();
        }

        private void AddBookmarkTree(Common.Models.Bookmark bookmark, Common.Models.Bookmark parent, string path = "")
        {
            bookmark.Parent = parent;

            string bookmarkPath = bookmark.Name;

            if (!string.IsNullOrEmpty(path))
                bookmarkPath = path + BookmarkJoinPathString + bookmarkPath;

            if (bookmark.IsForder)
            {
                foreach (var child in bookmark.Children)
                {
                    AddBookmarkTree(child, bookmark, bookmarkPath);
                }
            }

            if (!Bookmarks.ContainsKey(bookmarkPath))
                Bookmarks.Add(bookmarkPath, bookmark);
        }

        private void RemoveBookmarkTree(Common.Models.Bookmark bookmark, string bookmarkPath)
        {
            Bookmarks.Remove(bookmarkPath);

            foreach (var child in bookmark.Children)
            {
                RemoveBookmarkTree(child, bookmarkPath + BookmarkJoinPathString + child.Name);
            }
        }

        private void ImportBookmark(IBookmarkImporter bookmarkImporter)
        {
            StaticLogger.Logger.Info("Importing bookmarks");
            foreach (var importedBookmark in bookmarkImporter.ImportBookmarks())
                AddBookmarkTree(importedBookmark, null, "");
            Save();
        }

        public string GetBookmarkPath(Common.Models.Bookmark bookmarkParent, string bookmarkName)
        {
            string path = bookmarkName;

            if (bookmarkParent != null)
                path = GetBookmarkPath(bookmarkParent) + BookmarkJoinPathString + path;

            return path;
        }
    }
}