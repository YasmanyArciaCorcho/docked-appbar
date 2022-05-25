using Common.Models;
using SearchBar.UI.Base;
using SearchBar.UI.Handles.Bookmarks;
using Services.Bookmark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBar.UI.Controls.Dashboad.Bookmarks
{
    /// <summary>
    /// Interaction logic for IBookmark.xaml
    /// </summary>
    public partial class BookmarkDashboard : UserControl, IBookmarkContainer
    {
        readonly IBookmarkHandler _BookmarkHandle;
        public Bookmark BaseBookmark
        { get; set; }

        public BookmarkDashboard(IBookmarkHandler bookmarkHandle)
        {
            InitializeComponent();

            _BookmarkHandle = bookmarkHandle;
        }

        public void AddBookmarkUIElement(UIElement uiElement)
         => BookmarkZone.Children.Add(uiElement);

        public void AddBookmarkWindow(UserControl BookmarkWindow)
        {
            BookmarkWindow.Margin = new Thickness(BookmarkZone.Margin.Left, BookmarkZone.Margin.Top, 0, 0);
            BookmarkWindow.Visibility = Visibility.Visible;
            MainGrid.Children.Add(BookmarkWindow);
        }

        public void ClearBookmarksUIElement()
        {
            BookmarkZone.Children.Clear();
        }

        public bool ContainBookmarkUIElement(UIElement uiElement)
        => BookmarkZone.Children.Contains(uiElement);

        public void RemoveBookmarkUIElement(UIElement uiElement)
        {
            BookmarkZone.Children.Remove(uiElement);
        }

        public void RemoveBookmarkWindows(UserControl IBookmarkWindow)
        {
            MainGrid.Children.Remove(IBookmarkWindow);
        }

        private void BookmarkImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _BookmarkHandle.ShowBookmarkManager(this, BaseBookmark, sender as UIElement);
        }

        public void UpdateWindowsTitle(string title)
        {
            Title.Content = title;
        }

        private void StarBookmark_MouseLeave(object sender, MouseEventArgs e)
         => BookmarkImage.Foreground = new SolidColorBrush(Colors.White);

        private void StarBookmark_MouseMove(object sender, MouseEventArgs e)
        => BookmarkImage.Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 215));

        private void ArrowRight_MouseMove(object sender, MouseEventArgs e)
        => ArrowRight.Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 215));

        private void ArrowRight_MouseLeave(object sender, MouseEventArgs e)
        => ArrowRight.Foreground = new SolidColorBrush(Colors.White);

        private void ArrowRight_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (BaseBookmark.Parent != null)
            {
                _BookmarkHandle.Initialize(this, BaseBookmark.Parent);
            }
            else
            {
                _BookmarkHandle.Initialize(null, BaseBookmark);
            }
        }

        public void UpdateBookmarkCurrentParent(Bookmark bookmark)
        {
            BaseBookmark = bookmark;
        }
    }
}
