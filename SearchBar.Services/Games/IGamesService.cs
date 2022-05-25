using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Games
{
    public interface IGamesService
    {
        IEnumerable<string> GetCategories();

        IEnumerable<IGame> GetGames(string category);

        IEnumerable<IGame> GetGames(string category, string gameName);
    }
}
