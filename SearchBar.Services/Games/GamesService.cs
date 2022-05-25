using Common;
using Common.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Games
{
    public class GamesService : IGamesService
    {
        private readonly string _gameProviderUrl = "https://affiliate-games.s3.amazonaws.com/games.json";
        private Dictionary<string, SortedSet<IGame>> _gameDefinition;

        public GamesService()
        {
        }

        public IEnumerable<IGame> GetGames(string category)
        {
            return GetGames(category, "");
        }

        public IEnumerable<IGame> GetGames(string category, string gameName)
        {
            UpdateLaterstGame();

            if (_gameDefinition != null && _gameDefinition.ContainsKey(category))
            {
                foreach (var game in _gameDefinition[category])
                {
                    if (game.Title.ToLower().Contains(gameName.ToLower()))
                        yield return game;
                }
            }
            else
            {
                foreach (var gameCategory in _gameDefinition.Keys)
                    foreach (var game in _gameDefinition[gameCategory])
                        if (game.Title.ToLower().Contains(gameName.ToLower()))
                            yield return game;
            }
        }

        public IEnumerable<string> GetCategories()
        {
            UpdateLaterstGame();

            List<string> categories = new List<string>()
            {
                "All Categories"
            };

            if (_gameDefinition != null)
                categories.AddRange(_gameDefinition.Keys);

            return categories;
        }

        private void UpdateLaterstGame()
        {
            // Maybe we will want to update the json with new games.
            // Right now its static list.
            try
            {
                if (_gameDefinition == null)
                {
                    GamesModelAPI gamesModelApi;
                    string gameFilePath = DirectoryInfoHelper.GetGamesFilePath();
                    if (File.Exists(gameFilePath))
                    {
                        using var reader = new StreamReader(gameFilePath);
                        gamesModelApi = JsonConvert.DeserializeObject<GamesModelAPI>(reader.ReadToEnd());
                    }
                    else
                    {
                        string jsonGamesModelApi = HTTPRequestHelper.DoQuery(_gameProviderUrl);
                        gamesModelApi = JsonConvert.DeserializeObject<GamesModelAPI>(jsonGamesModelApi);
                        File.WriteAllText(gameFilePath, jsonGamesModelApi);
                    }

                    InitializeGameDefinition(gamesModelApi);
                }
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }
        }

        private void InitializeGameDefinition(GamesModelAPI gamesModelApi)
        {
            _gameDefinition = new Dictionary<string, SortedSet<IGame>>();

            if (gamesModelApi != null)
            {
                for (int i = 0; i < gamesModelApi.Categories.Count; i++)
                {
                    _gameDefinition.Add(gamesModelApi.Categories[i], new SortedSet<IGame>());
                }

                for (int i = 0; i < gamesModelApi.Games.Count; i++)
                {
                    string gameCategory = gamesModelApi.Games[i].Category;
                    if (!string.IsNullOrEmpty(gameCategory))
                    {
                        if (!_gameDefinition.ContainsKey(gameCategory))
                            _gameDefinition.Add(gameCategory, new SortedSet<IGame>());

                        _gameDefinition[gameCategory].Add(gamesModelApi.Games[i]);
                    }
                }
            }
        }
    }
}
