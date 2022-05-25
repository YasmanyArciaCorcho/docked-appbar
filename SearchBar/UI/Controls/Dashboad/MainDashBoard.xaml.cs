using Common.ChromiumSettings;
using Common.Models;
using Common.Settings.Chromium;
using SearchBar.UI.Base;
using SearchBar.UI.Controls.About;
using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Handles;
using SearchBar.UI.Handles.Bookmarks;
using SearchBar.UI.Handles.News;
using SearchBar.UI.Handles.ShortcutPackage;
using SearchBar.UI.Handles.TopSites;
using SearchBar.UI.WebBar;
using Services.AudioServices;
using Services.Bookmark;
using Services.ComputerManagement;
using Services.NetworkServices;
using Services.Weather;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SearchBar.UI.Controls.Dashboad
{
    /// <summary>
    /// Interaction logic for MainDashBoard.xaml
    /// </summary>
    public partial class MainDashBoard : UserControl, IDashboard, IBookmarkContainer
    {
        private DispatcherTimer _updateNewsTimer;

        readonly INewsHandler<MainDashBoard> _newsHandler;
        readonly IBookmarkHandler _bookmarkHandler;
        readonly IShortcutPackageHandler<MainDashBoard> _shortcutPackageHandle;
        readonly ITopSitesHandler<MainDashBoard> _topSitesHandler;
        readonly IChromiumSettingsService _chromiumSettingsService;
        public WebBarViewModel WebBarViewModel
        {
            get;
            set;
        }
        public ICommand OpenIBookmarkPopupMenu
        { get; private set; }

        public MainDashBoard(WebBarViewModel webBarViewModel,
            IBookmarkHandler bookmarkHandler,
            INewsHandler<MainDashBoard> newsHandler,
            IShortcutPackageHandler<MainDashBoard> shortcutPackageHandler,
            ITopSitesHandler<MainDashBoard> topSitesHandler,
            IWeatherService weatherService,
            IChromiumSettingsService chromiumSettingsService)
        {
            InitializeComponent();

            WebBarViewModel = webBarViewModel;

            _bookmarkHandler = bookmarkHandler;
            _newsHandler = newsHandler;
            _shortcutPackageHandle = shortcutPackageHandler;
            _topSitesHandler = topSitesHandler;
            _chromiumSettingsService = chromiumSettingsService;

            TodayWeather.DataContext = weatherService.GetTodayWeather();

            Initialize();
        }

        protected void Initialize()
        {
            SetHandlers();

            _bookmarkHandler.Initialize(this);
            _newsHandler.Initialize();
            _shortcutPackageHandle.Initialize();
            _topSitesHandler.Initialize(this);

            this._updateNewsTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(15)
            };
            this._updateNewsTimer.Tick += ((sender, args) =>
            {
                _newsHandler.UpdateNewsZone();
            });

            _updateNewsTimer.Start();

            BookmarkZoneScrollViewer.Width = Math.Max(300, _newsHandler.NewsZoneWidth - BookmarkZoneScrollViewer.Margin.Left - 15) / 2;
            TopSitesZoneScrollViewer.Width = BookmarkZoneScrollViewer.Width;
            TopSitesZonePanel.Width = BookmarkZoneScrollViewer.Width - 10;
        }

        private void SetHandlers()
        {
            _newsHandler.Dashboard = _shortcutPackageHandle.Dashboard = this;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutMenu = new AboutWindow(WebBarViewModel);
            aboutMenu.Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
         => Application.Current.Shutdown();

        private void StarBookmark_MouseMove(object sender, MouseEventArgs e)
          => BookmarkImage.Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 215));

        private void StarBookmark_MouseLeave(object sender, MouseEventArgs e)
          => BookmarkImage.Foreground = new SolidColorBrush(Colors.White);

        private void BookmarkImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
          => _bookmarkHandler.ShowBookmarkManager(this, sender as UIElement);

        #region Bookmark container

        public Bookmark BaseBookmark { get { return null; } set { } }

        public void AddBookmarkUIElement(UIElement uiElement)
        {
            BookmarkZone.Children.Add(uiElement);
        }

        public void RemoveBookmarkUIElement(UIElement uiElement)
        {
            BookmarkZone.Children.Remove(uiElement);
        }

        public void ClearBookmarksUIElement()
        {
            BookmarkZone.Children.Clear();
        }

        public void AddBookmarkWindow(UserControl bookmarkWindow)
        {
            double left = (RootCanvas.ActualWidth - bookmarkWindow.Width) / 2;
            Canvas.SetLeft(bookmarkWindow, left);

            double top = (RootCanvas.ActualHeight - bookmarkWindow.Height) / 2;
            Canvas.SetTop(bookmarkWindow, top);

            bookmarkWindow.Visibility = Visibility.Visible;

            RootCanvas.Children.Add(bookmarkWindow);
        }

        public void RemoveBookmarkWindows(UserControl bookmarkWindow)
        {
            RootCanvas.Children.Remove(bookmarkWindow);
        }

        public bool ContainBookmarkUIElement(UIElement uiElement)
        => RootCanvas.Children.Contains(uiElement);

        public void UpdateWindowsTitle(string title)
        {
        }

        public void UpdateBookmarkCurrentParent(Bookmark bookmark)
        {
        }

        #endregion
    }
}
