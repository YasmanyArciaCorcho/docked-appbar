using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Games
{
  public  class Game : IGame
    {
        public string Category
        { get; set; }

        public string Url
        { get; set; }
      
        public string Title
        { get; set; }
        
        public string Event 
        { get; set; }
        
        public string Css { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is IGame other) 
                return Title.CompareTo(other.Title);
            return -1;
        }
    }
}
