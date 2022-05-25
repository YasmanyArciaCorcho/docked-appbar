using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Games
{
   public class GamesModelAPI
    {
        public List<string> Categories 
        { get; set; }
        public List<Game> Games
        { get; set; }

        public GamesModelAPI()
        {
            Categories = new List<string>();
            Games = new List<Game>();
        }
    }
}
