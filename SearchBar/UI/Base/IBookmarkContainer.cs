using Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace SearchBar.UI.Base
{
    public interface IBookmarkContainer
    {
        Bookmark BaseBookmark 
        { get; set; }

        void UpdateBookmarkCurrentParent(Bookmark bookmark);

        void UpdateWindowsTitle(string title);

        void AddBookmarkUIElement(UIElement uiElement);

        void RemoveBookmarkUIElement(UIElement uiElement);

        bool ContainBookmarkUIElement(UIElement uiElement);

        void ClearBookmarksUIElement();

        void AddBookmarkWindow(UserControl BookmarkWindow);

        void RemoveBookmarkWindows(UserControl BookmarkWindow);
    }
}
