using System;
using System.Collections.Generic;

namespace Services.Bookmark
{
    public interface IBookmarkService
    {
        string BookmarkJoinPathString { get; }

        Dictionary<string, Common.Models.Bookmark> Bookmarks { get; set; }

        IEnumerable<Common.Models.Bookmark> GetBookmarkRoots();

        Common.Models.Bookmark AddBookmarkPage(Common.Models.Bookmark parent, string name, string url);

        Common.Models.Bookmark EditBookmarkPage(string oldIBookmarkPath, string newName, string newUrl);

        Common.Models.Bookmark AddBookmarkFolder(Common.Models.Bookmark parent, string name);

        Common.Models.Bookmark EditBookmarkFolder(string oldfolderPath, string newName);

        Common.Models.Bookmark RemoveBookmark(string bookmarkPath);

        Common.Models.Bookmark FindBookmark(string bookmarkPath);

        bool Contains(string bookmarkPath);

        string GetBookmarkPath(Common.Models.Bookmark bookmark);

        string GetBookmarkPath(Common.Models.Bookmark bookmarkParent, string bookmarkName);
    }
}
