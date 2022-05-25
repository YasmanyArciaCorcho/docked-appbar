using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Handles.Games;
using SearchBar.UI.WebBar;
using Services.Games;
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

namespace SearchBar.UI.Controls.Dashboad.Games
{
    /// <summary>
    /// Interaction logic for GamesDashboard.xaml
    /// </summary>
    public partial class GamesDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Games";
        public static string ImagePath = "games_logo";

        public string[] GamesCategories { get; set; }
        public WebBarViewModel WebBarViewModel { get; set; }

        private static readonly SolidColorBrush defaultForegroundGameFilterText = new SolidColorBrush(Color.FromRgb(170, 167, 162));
        private static readonly string TextExample = "e.g. the sims";
        readonly IGamesHandler<GamesDashboard> _gamesHandler;

        public GamesDashboard(WebBarViewModel webBarViewModel, IGamesHandler<GamesDashboard> gamesHandler, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            WebBarViewModel = webBarViewModel;
            _gamesHandler = gamesHandler;

            gamesHandler.Dashboard = this;

            _gamesHandler.SetCategories();

            GamesCategoryComboBox.SelectedIndex = 0;
            GamesCategoryComboBox.Foreground = Brushes.White;

            ResetGameFilterTextBox();

            GameFilter.PreviewGotKeyboardFocus += GameFilter_PreviewGotKeyboardFocus;

            Backgammon.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.247backgammon.org/"); };
            ThreeGates.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("http://www.coffeedoggames.com/games/threegatessolitaire/hd.html"); };
            Spider.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.spider-solitaire-game.com/"); };
            Mahjong.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.free-play-mahjong.com/"); };
            WarFrame.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.warframe.com/landing"); };
            WorldofTanks.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://worldoftanks.com/"); };
        }

        private void GameFilter_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GameFilter.Foreground = Brushes.White;
            GameFilter.Text = "";
        }

        private void GameFilter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UpdateGameList(GameFilter.Text);
        }

        private void UpdateGameList(string textFilter)
        {
            if (GamesCategoryComboBox.SelectedItem != null)
            {
                _gamesHandler.UpdateGameZone((string)GamesCategoryComboBox.SelectedItem, textFilter);
            }
        }

        private void ResetGameFilterTextBox()
        {
            GameFilter.Text = TextExample;
            GameFilter.Foreground = defaultForegroundGameFilterText;
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string textFilter = GameFilter.Text;
            if (textFilter.Equals(TextExample))
            {
                textFilter = "";
            }
            else
            {
                ResetGameFilterTextBox();
            }

            UpdateGameList(textFilter);
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "games_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(Backgammon.Image, string.Format(imageNamespace, "backgammon"));
            imageSourceBuilder.SetImageSource(ThreeGates.Image, string.Format(imageNamespace, "tgs"));
            imageSourceBuilder.SetImageSource(Spider.Image, string.Format(imageNamespace, "spider"));
            imageSourceBuilder.SetImageSource(Mahjong.Image, string.Format(imageNamespace, "mahjong"));
            imageSourceBuilder.SetImageSource(WarFrame.Image, string.Format(imageNamespace, "warframe"));
            imageSourceBuilder.SetImageSource(WorldofTanks.Image, string.Format(imageNamespace, "wft"));
        }
    }
}
