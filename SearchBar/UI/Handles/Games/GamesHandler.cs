using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Dashboad.Games;
using Services.Games;

namespace SearchBar.UI.Handles.Games
{
    public class GamesHandler : IGamesHandler<GamesDashboard>
    {
        public GamesDashboard Dashboard { get; set; }

        readonly IGamesService _gamesService;

        public GamesHandler(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public void SetCategories()
        {
            Thread thre = new Thread(new ThreadStart(() =>
            {
                string[] games = _gamesService.GetCategories().ToArray();

                System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
               {
                   Dashboard.GamesCategories = games;

                   if (Dashboard.GamesCategories.Length > 0)
                       UpdateGameZone(Dashboard.GamesCategories[0], "");
                   Dashboard.DataContext = Dashboard;

               }, DispatcherPriority.Background);
                Dispatcher.Run();
            }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();
        }

        public IEnumerable<IGame> GetGames(string category, string gameName)
        => _gamesService.GetGames(category, gameName);

        public void UpdateGameZone(string category, string gameName)
        {
            Dashboard.GamesListZone.Children.Clear();

            if (string.IsNullOrWhiteSpace(gameName))
                gameName = string.Empty;

            Thread thre = new Thread(new ThreadStart(() =>
            {
                IEnumerable<IGame> games = _gamesService.GetGames(category, gameName);

                System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
               {
                   foreach (var game in games)
                   {
                       await Task.Yield();

                       CustomButton gameButtonOption = new CustomButton()
                       {
                           Content = new Label()
                           {
                               Content = game.Title,
                               Foreground = Brushes.White
                           },
                           VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                           Height = 40
                       };

                       gameButtonOption.Click += (object sender, System.Windows.RoutedEventArgs e) =>
                       {
                           Dashboard.WebBarViewModel.OpenDirectUrlBrowser(game.Url);
                       };

                       Dashboard.GamesListZone.Children.Add(gameButtonOption);
                   };

                   Dashboard.TotalGamesText.Text = Dashboard.GamesListZone.Children.Count.ToString();
               }, DispatcherPriority.Background);
                Dispatcher.Run();
            }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();
        }
    }
}
