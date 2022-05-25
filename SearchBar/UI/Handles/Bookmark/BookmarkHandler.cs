using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SearchBar.UI.Base;
using SearchBar.UI.Base.Shortcut;
using SearchBar.UI.Controls.ShortCut;
using SearchBar.UI.Handles.Shortcut.PinShortcut;
using SearchBar.UI.WebBar;
using Services.Bookmark;
using SearchBar.UI.Controls.Dashboad.Bookmarks;
using SearchBar.UI.Controls.Bookmarks;
using System.Windows.Threading;
using System.Threading;
using SearchBar.UI.Controls.Base;
using Common;
using Common.Logger;
using Common.Models;
using Common.Impressions;

namespace SearchBar.UI.Handles.Bookmarks
{
    public class BookmarkHandler : IBookmarkHandler<MainWindow>
    {
        public bool IsBookmarkActive
        { get; private set; }

        private SortedList<string, ShortcutControl> _bookmarkControls;

        private readonly IBookmarkService _bookmarkService;

        private Bookmark _handledBookmark;
        private IBookmarkContainer _handledBookmarkContainer;

        private readonly IPinShortcutHandler _pinHandle;
        readonly WebBarViewModel WebBarViewModel;

        public MainWindow BookmarkDashboardContainer
        { get; set; }

        BookmarkDashboard _bookmarkDashboard;

        public BookmarkHandler(IBookmarkService bookmarkService, IPinShortcutHandler pinHandle, WebBarViewModel webBarViewModel)
        {
            _bookmarkService = bookmarkService;
            _pinHandle = pinHandle;
            WebBarViewModel = webBarViewModel;
            _bookmarkControls = new SortedList<string, ShortcutControl>();
        }

        public async Task AddBookmarkPage(IBookmarkContainer bookmarkContainer, Bookmark parent, string name, string url)
        {
            Bookmark newBookmark = _bookmarkService.AddBookmarkPage(parent, name, url);
            if (newBookmark != null)
                await AddBookmark(bookmarkContainer, newBookmark);
        }

        public async Task AddBookmarkFolder(IBookmarkContainer bookmarkContainer, Bookmark parent, string name)
        {
            Bookmark newBookmark = _bookmarkService.AddBookmarkFolder(parent, name);
            if (newBookmark != null)
                await AddBookmark(bookmarkContainer, newBookmark);
        }

        public void RemoveBookmark(IBookmarkContainer bookmarkContainer, string bookmarkPath)
        {
            if (_bookmarkControls.ContainsKey(bookmarkPath))
            {
                ShortcutControl toRemove = _bookmarkControls[bookmarkPath];

                _bookmarkControls.Remove(bookmarkPath);

                _bookmarkService.RemoveBookmark(bookmarkPath);

                bookmarkContainer.RemoveBookmarkUIElement(toRemove);

                StaticImpressions.SendImpression("sbr-rm-bkm");
            }
        }

        public void RemoveBookmark(IBookmarkContainer bookmarkContainer, Bookmark bookmark)
        => RemoveBookmark(bookmarkContainer, _bookmarkService.GetBookmarkPath(bookmark));

        public async Task EditBookmarkPage(string oldBookmarkPath, string newName, string newUrl)
        => await EditBookmark(oldBookmarkPath, newName, newUrl);

        public async Task EditBookmarkFolder(string oldBookmarkPath, string newName)
         => await EditBookmark(oldBookmarkPath, newName, null);

        public async Task EditBookmarkPage(Bookmark oldIBookmark, string newName, string newUrl)
        => await EditBookmarkPage(_bookmarkService.GetBookmarkPath(oldIBookmark), newName, newUrl);

        public async Task EditBookmarkFolder(Bookmark oldBookmark, string newName)
        => await EditBookmarkFolder(_bookmarkService.GetBookmarkPath(oldBookmark), newName);

        public void ShowBookmarkManager(IBookmarkContainer bookmarkContainer, UIElement placementTarget)
        => CreateBookmarkPupupMenu(bookmarkContainer, null, placementTarget);

        public void ShowBookmarkManager(IBookmarkContainer bookmarkContainer, Bookmark bookmarkParent, UIElement placementTarget)
        => CreateBookmarkPupupMenu(bookmarkContainer, bookmarkParent, placementTarget);

        public void Initialize(IBookmarkContainer bookmarkContainer, Bookmark bookmarkParent)
        {
            string parentPath = null;
            if (bookmarkParent != null)
                parentPath = _bookmarkService.GetBookmarkPath(bookmarkParent);

            Initialize(bookmarkContainer, parentPath);
        }

        public void Initialize(IBookmarkContainer bookmarkContainer, string parentPath = null)
        {
            if (_bookmarkControls == null)
                _bookmarkControls = new SortedList<string, ShortcutControl>();

            if (bookmarkContainer == null)
            {
                BookmarkDashboardContainer.ToggleDashboard(BookmarkDashboardContainer.MainDashboard);
            }
            else
            {
                UpdateBookmarkContainer(bookmarkContainer, parentPath);

                StaticLogger.Logger.Info("BookmarkHandle - added Bookmark.");
            }
        }

        public void UpdateBookmarkContainer(IBookmarkContainer bookmarkContainer, string parentPath = null)
        {
            if (_bookmarkService.Contains(parentPath))
            {
                bookmarkContainer.UpdateWindowsTitle(parentPath);

                bookmarkContainer.ClearBookmarksUIElement();

                if (string.IsNullOrEmpty(parentPath))
                {
                    bookmarkContainer.UpdateBookmarkCurrentParent(null);
                }
                else
                {
                    bookmarkContainer.UpdateBookmarkCurrentParent(_bookmarkService.Bookmarks[parentPath]);
                }


                Thread thre = new Thread(new ThreadStart(() =>
               {
                   foreach (var bookmark in GetBookmarks(parentPath))
                   {
                       string bookmarkPath = _bookmarkService.GetBookmarkPath(bookmark);

                       System.Windows.Application.Current.Dispatcher.Invoke(async () =>
                       {
                           ShortcutControl buildedBookmarkControl;

                           if (!_bookmarkControls.ContainsKey(bookmarkPath))
                           {
                               buildedBookmarkControl = await BuildBookmarkInCustomButtonAsync(bookmarkContainer, bookmark);
                               _bookmarkControls.Add(bookmarkPath, buildedBookmarkControl);
                           }
                           else
                           {
                               buildedBookmarkControl = _bookmarkControls[bookmarkPath];
                           }

                           if (!bookmarkContainer.ContainBookmarkUIElement(buildedBookmarkControl))
                               bookmarkContainer.AddBookmarkUIElement(buildedBookmarkControl);
                       },
                       DispatcherPriority.Background);
                   }
                   Dispatcher.Run();
               }));
                thre.SetApartmentState(ApartmentState.MTA);
                thre.IsBackground = true;
                thre.Start();
            }
        }

        private async Task AddBookmark(IBookmarkContainer bookmarkContainer, Bookmark Bookmark)
        {
            ShortcutControl newIBookmarkControl = await BuildBookmarkInCustomButtonAsync(bookmarkContainer, Bookmark);
            _bookmarkControls.Add(_bookmarkService.GetBookmarkPath(Bookmark), newIBookmarkControl);

            if (Bookmark.Parent == bookmarkContainer.BaseBookmark)
                bookmarkContainer.AddBookmarkUIElement(newIBookmarkControl);

            StaticImpressions.SendImpression("sbr-add-bkm");
        }

        private async Task EditBookmark(string oldBookmarkPath, string newName, string newUrl)
        {
            if (_bookmarkControls.ContainsKey(oldBookmarkPath))
            {
                Bookmark editedBookmark = _bookmarkService.EditBookmarkPage(oldBookmarkPath, newName, newUrl);

                ShortcutControl editControl = _bookmarkControls[oldBookmarkPath];

                _bookmarkControls.Remove(oldBookmarkPath);

                if (editControl.Content is CustomButton button)
                {
                    button.ToolTip = newName;

                    button.CommandParameter = newUrl;

                    await ShortcutBuilder.UpdateShortcutControlImageAsync(editControl, newUrl, editedBookmark.IsForder);
                }

                _bookmarkControls.Add(newName, editControl);
            }
        }

        private async Task<ShortcutControl> BuildBookmarkInCustomButtonAsync(IBookmarkContainer bookmarkContainer, Bookmark bookmark)
        {
            if (bookmark.IsForder)
            {
                return await ShortcutBuilder.BuildShortcutAsync(bookmark.Name, bookmark.Url, true, RefreshBookmarkDashboard(bookmark),
                         (object o, MouseButtonEventArgs e) => { ShowBookmarkManager(bookmarkContainer, bookmark.Parent, o as UIElement); });
            }
            return await ShortcutBuilder.BuildShortcutAsync(bookmark.Name, bookmark.Url, false, WebBarViewModel.OpenBrowser, bookmark.Url,
                  (object o, MouseButtonEventArgs e) => { ShowBookmarkManager(bookmarkContainer, bookmark.Parent, o as UIElement); });
        }

        private Action<object, MouseButtonEventArgs> RefreshBookmarkDashboard(Bookmark bookmark)
        {
            return (object o, MouseButtonEventArgs e) =>
              {
                  BookmarkDashboard bkDashboard = GetBookmarkDashboard();

                  bkDashboard.BaseBookmark = bookmark;

                  bkDashboard.ClearBookmarksUIElement();

                  Initialize(bkDashboard, _bookmarkService.GetBookmarkPath(bookmark));

                  if (!ReferenceEquals(bkDashboard, BookmarkDashboardContainer.CurrentDashBoard))
                      BookmarkDashboardContainer.ToggleDashboard(bkDashboard);
              };
        }

        private void CreateBookmarkPupupMenu(IBookmarkContainer bookmarkContainer, Bookmark bookmarkParent, UIElement placementTarget)
        {
            ContextMenu bookmarkPopupMenu = new ContextMenu
            {
                FontSize = 14
            };

            #region Add Items

            if (placementTarget is CustomButton customButton
                && customButton.ToolTip != null)
            {
                string toolTipText = customButton.ToolTip.ToString();
                string bookmarkPath = _bookmarkService.GetBookmarkPath(bookmarkParent, toolTipText);

                if (_bookmarkControls.ContainsKey(bookmarkPath))
                {
                    IsBookmarkActive = true;
                    _handledBookmark = _bookmarkService.FindBookmark(bookmarkPath);

                    if (!_handledBookmark.IsForder)
                    {
                        MenuItem pin = new MenuItem
                        {
                            Header = "Pin to search bar ..."
                        };

                        pin.Click += (object sender, RoutedEventArgs e) =>
                        {
                            _pinHandle.PintoBar(toolTipText, _handledBookmark.Url, _handledBookmark.IsForder, WebBarViewModel.OpenBrowser, _handledBookmark.Url);
                        };

                        bookmarkPopupMenu.Items.Add(pin);

                        Separator separator1 = new Separator
                        {
                            Background = Brushes.Gray
                        };
                        bookmarkPopupMenu.Items.Add(separator1);
                    }

                    MenuItem editBookmark = new MenuItem
                    {
                        Header = "Edit"
                    };
                    editBookmark.Click += EditBookmark_Click;
                    bookmarkPopupMenu.Items.Add(editBookmark);

                    MenuItem removeBookmark = new MenuItem
                    {
                        Header = "Remove"
                    };
                    removeBookmark.Click += RemoveBookmark_Click;
                    bookmarkPopupMenu.Items.Add(removeBookmark);

                    Separator separator2 = new Separator
                    {
                        Background = Brushes.Gray
                    };
                    bookmarkPopupMenu.Items.Add(separator2);
                }
            }

            if (_handledBookmark == null)
                _handledBookmark = bookmarkParent;

            MenuItem addPage = new MenuItem
            {
                Header = "Add Bookmark page..."
            };
            addPage.Click += OpenAddBookmarkPageWindow;
            bookmarkPopupMenu.Items.Add(addPage);

            MenuItem addFolder = new MenuItem
            {
                Header = "Add Bookmark folder..."
            };
            addFolder.Click += OpenAddBookmarkFolderWindow;
            bookmarkPopupMenu.Items.Add(addFolder);

            #endregion

            bookmarkPopupMenu.Closed += (object sender, RoutedEventArgs e) =>
            {
                _handledBookmarkContainer = null;
                _handledBookmark = null;
                IsBookmarkActive = false;
            };

            _handledBookmarkContainer = bookmarkContainer;

            bookmarkPopupMenu.IsEnabled = true;
            bookmarkPopupMenu.PlacementTarget = placementTarget;
            bookmarkPopupMenu.Placement = PlacementMode.Bottom;
            bookmarkPopupMenu.IsOpen = true;
        }

        private void EditBookmark_Click(object sender, RoutedEventArgs e)
        {
            if (IsBookmarkActive)
                OpenEditBookmarkWindow();
        }

        private void RemoveBookmark_Click(object sender, RoutedEventArgs e)
        {
            if (IsBookmarkActive)
            {
                RemoveBookmark(_handledBookmarkContainer, _handledBookmark);
            }
        }

        private void OpenAddBookmarkPageWindow(object sender, RoutedEventArgs e)
        => OpenAddBookmarkWindow(false);

        private void OpenAddBookmarkFolderWindow(object sender, RoutedEventArgs e)
        => OpenAddBookmarkWindow(true);

        private void OpenAddBookmarkWindow(bool isFolder)
        {
            UserControl addBkPageWindow;
            if (isFolder)
            {
                addBkPageWindow = new AddBookmarkFolder(_handledBookmarkContainer, this, _handledBookmark);
            }
            else
            {
                addBkPageWindow = new AddBookmarkPage(_handledBookmarkContainer, this, _handledBookmark);
            }

            _handledBookmarkContainer.AddBookmarkWindow(addBkPageWindow);
        }

        private void OpenEditBookmarkWindow()
        {
            UserControl editBbWindow;
            if (_handledBookmark != null && _handledBookmark.IsForder)
            {
                editBbWindow = new EditBookmarkFolder(_handledBookmarkContainer, this, _handledBookmark);
            }
            else
            {
                editBbWindow = new EditBookmarkPage(_handledBookmarkContainer, this, _handledBookmark);
            }
            _handledBookmarkContainer.AddBookmarkWindow(editBbWindow);
        }

        private IEnumerable<Bookmark> GetBookmarks(string parentPath = null)
        {
            if (string.IsNullOrEmpty(parentPath))
            {
                return _bookmarkService.GetBookmarkRoots();
            }
            {
                return _bookmarkService.Bookmarks[parentPath].Children;
            }
        }

        private BookmarkDashboard GetBookmarkDashboard()
        {
            if (_bookmarkDashboard == null)
            {
                _bookmarkDashboard = new BookmarkDashboard(this)
                {
                    Height = BookmarkDashboardContainer.Height
                };
            }
            return _bookmarkDashboard;
        }
    }
}
