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
    /// Interaction logic for AddIBookmarkFolder.xaml
    /// </summary>
    public partial class AddBookmarkFolder : UserControl
    {
        readonly IBookmarkHandler _bookmarkHandle;
        readonly IBookmarkContainer _bookmarkContainer;
        readonly Bookmark _bookmarkParent;

        public AddBookmarkFolder(IBookmarkContainer bookmarkContainer, IBookmarkHandler bookmarkHandle, Bookmark BookmarkParent)
        {
            _bookmarkHandle = bookmarkHandle;
            _bookmarkParent = BookmarkParent;
            _bookmarkContainer = bookmarkContainer;

            KeyDown += Window_KeyDown;

            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         => Close();

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            _bookmarkHandle.AddBookmarkFolder(_bookmarkContainer, _bookmarkParent, TextBoxName.Text);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Close()
        => _bookmarkContainer.RemoveBookmarkWindows(this);

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _bookmarkHandle.AddBookmarkFolder(_bookmarkContainer, _bookmarkParent, TextBoxName.Text);
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
