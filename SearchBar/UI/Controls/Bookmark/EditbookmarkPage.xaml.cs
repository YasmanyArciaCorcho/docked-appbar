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

namespace SearchBar.UI.Controls.Bookmarks
{
    /// <summary>
    /// Interaction logic for EditIBookmarkPage.xaml
    /// </summary>
    public partial class EditBookmarkPage : UserControl
    {
        readonly IBookmarkContainer _bookmarkContainer;
        private readonly IBookmarkHandler _bookmarkHandle;
        private readonly Bookmark _oldBookmark;

        public EditBookmarkPage(IBookmarkContainer handledBookmarkContainer,IBookmarkHandler bookmarkHandle, Bookmark Bookmark)
        {
            InitializeComponent();

            _bookmarkContainer = handledBookmarkContainer;
            _bookmarkHandle = bookmarkHandle;
            PreviewKeyDown += Window_KeyDown;

            _oldBookmark= Bookmark;

            TextBoxName.Text = Bookmark.Name;
            TextBoxUrl.Text = Bookmark.Url;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
           => Close();

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            _bookmarkHandle.EditBookmarkPage( _oldBookmark, TextBoxName.Text, TextBoxUrl.Text);
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
                _bookmarkHandle.EditBookmarkPage( _oldBookmark, TextBoxName.Text, TextBoxUrl.Text);
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
