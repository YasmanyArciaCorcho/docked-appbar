using Common.Models;
using SearchBar.UI.Base;
using Services.Bookmark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SearchBar.UI.Handles.Bookmarks
{
    public interface IBookmarkHandler
    {
        public bool IsBookmarkActive
        { get; }

        Task AddBookmarkPage(IBookmarkContainer bookmarkContainer, Bookmark parent, string name, string url);

        Task AddBookmarkFolder(IBookmarkContainer bookmarkContainer, Bookmark parent, string name);

        Task EditBookmarkPage(string oldBookmarkPath, string newName, string newUrl);

        Task EditBookmarkPage( Bookmark oldBookmark, string newName, string newUrl);

        Task EditBookmarkFolder( string oldBookmarkPath, string newName);

        Task EditBookmarkFolder( Bookmark oldBookmark, string newName);

        void RemoveBookmark(IBookmarkContainer bookmarkContainer, string bookmarkPath);

        void RemoveBookmark(IBookmarkContainer bookmarkContainer, Bookmark bookmark);

        void ShowBookmarkManager(IBookmarkContainer bookmarkContainer, UIElement placementTarget);

        void ShowBookmarkManager(IBookmarkContainer bookmarkContainer, Bookmark bookmarkParent, UIElement placementTarget);

        void Initialize(IBookmarkContainer bookmarkContainer, string parentPath = null);

        void Initialize(IBookmarkContainer bookmarkContainer, Bookmark bookmarkParent);
    }

    public interface IBookmarkHandler<T> : IBookmarkHandler
    {
        T BookmarkDashboardContainer { get; set; }
    }
}
