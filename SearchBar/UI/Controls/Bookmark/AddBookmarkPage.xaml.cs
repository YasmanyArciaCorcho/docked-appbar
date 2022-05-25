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
    /// Interaction logic for AddIBookmarkPage.xaml
    /// </summary>
    public partial class AddBookmarkPage : UserControl
    {
        readonly IBookmarkContainer _handledBookmarkContainer;
        readonly IBookmarkHandler _bookmarkHandle;
        readonly Bookmark _bookmarkParent;

        public AddBookmarkPage(IBookmarkContainer handledBookmarkContainer, IBookmarkHandler bookmarkHandle, Bookmark bookmarkParent)
        {
            _handledBookmarkContainer = handledBookmarkContainer;
            _bookmarkHandle = bookmarkHandle;
            _bookmarkParent = bookmarkParent;

            KeyDown += Window_KeyDown;

            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         => Close();

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            _bookmarkHandle.AddBookmarkPage(_handledBookmarkContainer, _bookmarkParent, TextBoxName.Text, TextBoxUrl.Text);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Close()
        => _handledBookmarkContainer.RemoveBookmarkWindows(this);

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _bookmarkHandle.AddBookmarkPage(_handledBookmarkContainer, _bookmarkParent, TextBoxName.Text, TextBoxUrl.Text);
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
