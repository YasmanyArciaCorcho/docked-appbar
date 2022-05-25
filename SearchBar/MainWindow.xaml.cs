using SearchBar.UI.Base;
using SearchBar.UI.Controls.About;
using SearchBar.UI.Controls.Dashboad;
using SearchBar.UI.Handles;
using SearchBar.UI.Handles.Bookmarks;
using SearchBar.UI.Handles.Shortcut.PinShortcut;
using SearchBar.UI.WebBar;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using SearchBar.UI.Controls.Shortcut;
using Services.Office;
using Common;
using Common.Logger;
using SearchBar.UI.Handles.Widgets;
using Common.Settings.Chromium;
using Services.ComputerManagement;
using Services.NetworkServices;
using Services.AudioServices;
using SearchBar.UI.Controls.Base;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Windows.Threading;
using Common.Impressions;
using Microsoft.Win32;

namespace SearchBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDashboardControlParent
    {
        const int _currentDashboardIndex = 0;
        bool _isDashBoardActive;
        public MainDashBoard MainDashboard;
        public WidgetsPresentationDashborad ApplicationDaskboard;
        public UserControl CurrentDashBoard { get; private set; }

        public MainWindow SingleMainWindow { get { return this; } }

        private bool HasDefaultBackground
        { get; set; }
        public Brush DefaultSearchBarBackground
        { get; set; }

        public WebBarViewModel WebBarViewModel
        { get; }

        private readonly IWidgetsPresentationHandler _widgetsHandle;

        public MainWindow(IWidgetsPresentationHandler widgetsHandle,
            IPinShortcutHandler pinShortcutHandle,
            IBookmarkHandler IBookmarkHandle,
            WebBarViewModel webBarViewModel,
            MainDashBoard mainDashBoard)
        {
            try
            {
                InitializeComponent();
                Left = 0;
                Width = AppBarHandler.ScreenWidth();

                UpdateSrbPinnedShortcutZone();
                SystemEvents.DisplaySettingsChanged += (object sender, EventArgs e) => { UpdateSrbPinnedShortcutZone(); };

                _widgetsHandle = widgetsHandle;
                DataContext = webBarViewModel;
                MainDashboard = mainDashBoard;
                WebBarViewModel = webBarViewModel;
                Closing += new CancelEventHandler(this.CloseSearhcBar);
                ProductLogo.ToolTip = ProducSettings.ProducName;

                if (pinShortcutHandle is PinShortcutHandler pinHandle)
                {
                    pinHandle.SetShortcutContainer(this);
                }

                if (IBookmarkHandle is BookmarkHandler bkHandle)
                    bkHandle.BookmarkDashboardContainer = this;

                DashboardContainer.Visibility = Visibility.Collapsed;

                if (HasDefaultBackground)
                {
                    AppBarGrid.Background = DefaultSearchBarBackground;
                }
                else
                {
                    DefaultSearchBarBackground = AppBarGrid.Background;
                }
            }
            catch (Exception e)
            {
                UnRegisterAsAppBar();
                StaticLogger.Logger.Error(e);
                throw e;
            }
        }

        public void SetDefaultBackground(Brush backgroundBrush)
        {
            HasDefaultBackground = true;
            AppBarGrid.Background = DefaultSearchBarBackground = backgroundBrush;
            BackgroundGridImage.Visibility = Visibility.Collapsed;
        }

        public void SetDefaultButtonColor(Brush backgroundBrush)
        {
            PlusButtonImage.Foreground = backgroundBrush;
            ApplicationButtonImage.Foreground = backgroundBrush;
            MenuButtonImage.Foreground = backgroundBrush;
        }

        #region Window

        private void Window_Deactivated(object sender, EventArgs e)
        {
            ClearDashboard();

            SetDashboardContainerVisibility(Visibility.Collapsed);

            HideWindowsApp();
        }

        private void RegisterAsAppBar()
            => AppBarHandler.SetAppBar(this, ABEdge.Top);

        private void UnRegisterAsAppBar()
        {
            var registerInfor = AppBarHandler.GetRegisterInfo(this);
            if (registerInfor.IsRegistered)
                AppBarHandler.SetAppBar(this, ABEdge.None);
        }

        private void CloseSearhcBar(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnRegisterAsAppBar();
            StaticImpressions.SendImpression("sbr-exit");
        }

        public int GetAppBarDesiredheight()
            => 40;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterAsAppBar();
            AppBarHandler.HideAppbarFromWindows();

            if (!HasDefaultBackground)
                ImageHandler.SetBlurImageInBackground(this);
        }

        #endregion

        #region DashBoard

        public void ShowDefaultDashboard()
        {
            UserControl defaultDashboard = _widgetsHandle.GetDefaultWidgetByImplementationId();

            if (defaultDashboard != null)
                SetDashboard(defaultDashboard);
        }

        public void ToggleDashboard(UserControl dashborad)
        {
            if (dashborad == MainDashboard)
            {
                CloseDashboardBtn.Visibility = Visibility.Collapsed;
            }
            else 
            {
                CloseDashboardBtn.Visibility = Visibility.Visible;
            }

            if (_isDashBoardActive && CurrentDashBoard.Equals(dashborad))
            {
                ClearDashboard();

                SetDashboardContainerVisibility(Visibility.Collapsed);
            }
            else
            {
                SetDashboard(dashborad);
                HideWindowsApp();
            }
        }

        public void SetDashboard(UserControl dashborad)
        {
            if (DashboardContainer.Children.Count > 1)
                DashboardContainer.Children.RemoveAt(_currentDashboardIndex);

            DashboardContainer.Children.Insert(_currentDashboardIndex, dashborad);

            SetDashboardContainerVisibility(Visibility.Visible);

            _isDashBoardActive = true;

            CurrentDashBoard = dashborad;

            CurrentDashBoard.Width = AppBarHandler.ScreenWidth();

            if (!HasDefaultBackground)
                AppBarGrid.Background = Brushes.Black;
        }

        public void ClearDashboard()
        {
            _isDashBoardActive = false;

            CurrentDashBoard = null;

            if (DashboardContainer.Children.Count > 1)
                DashboardContainer.Children.RemoveAt(_currentDashboardIndex);

            AppBarGrid.Background = DefaultSearchBarBackground;
        }

        private void SetDashboardContainerVisibility(Visibility visibility)
            => DashboardContainer.Visibility = visibility;

        private void MenuBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleDashboard(MainDashboard);
            HideWindowsApp();
        }

        private void Apps_PreviewMouseDown(object sender, MouseButtonEventArgs e)
           => ToggleDashboard(ApplicationDaskboard);

        #endregion

        private void UpdateSrbPinnedShortcutZone()
        {
            // 50: product icon.
            // 400: search bar.
            // 80: NTP button and application dashboard button.
            // 40: main dashnoard button.
            ShortcutZone.ShortcutScrollViewer.Width = AppBarHandler.ScreenWidth() - 50 - 400 - 80 - 40;
        }

        private void HideWindowsApp()
        {
            // need to add the code (inheritance)
            // to handle all common elements. 
            // define their behavior with the search bar operations/actions.
        }

        private void CloseDashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleDashboard(CurrentDashBoard);
        }

        private void TrayBtn_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        #region Windows widgets: Example Office and Control Center.

        //We will create a builder and provider to handler this applications like dashboards.
        // We could have a builder and provider and dashboard builder and provider will would inherit from it.  


        #endregion
    }
}
