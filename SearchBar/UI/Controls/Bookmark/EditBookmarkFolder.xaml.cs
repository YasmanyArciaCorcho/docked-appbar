using Common.Models;
using SearchBar.UI.Base;
using SearchBar.UI.Handles.Bookmarks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SearchBar.UI.Controls.Bookmarks
{
    /// <summary>
    /// Interaction logic for EditIBookmarkFolder.xaml
    /// </summary>
    public partial class EditBookmarkFolder : UserControl
    {
        readonly IBookmarkContainer _bookmarkContainer;
        private readonly IBookmarkHandler _bookmarkHandle;
        private readonly Bookmark _oldBookmark;

        public EditBookmarkFolder(IBookmarkContainer handledBookmarkContainer,IBookmarkHandler bookmarkHandle, Bookmark Bookmark)
        {
            InitializeComponent();

            _bookmarkContainer = handledBookmarkContainer;
            _bookmarkHandle = bookmarkHandle;
            PreviewKeyDown += Window_KeyDown;

            _oldBookmark= Bookmark;

            TextBoxName.Text = Bookmark.Name;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
           => Close();

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            _bookmarkHandle.EditBookmarkFolder( _oldBookmark, TextBoxName.Text);
            Close();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            _bookmarkHandle.RemoveBookmark(_bookmarkContainer, _oldBookmark);
            Close();
        }

        private void Close()
            => _bookmarkContainer.RemoveBookmarkWindows(this);

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _bookmarkHandle.EditBookmarkFolder( _oldBookmark, TextBoxName.Text);
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
