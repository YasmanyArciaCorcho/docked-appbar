using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Games
{
    public interface IGame : IComparable
    {
        string Category { get; }

        string Url { get; set; }

        string Title { get; set; }

        string Event { get; set; }

        string Css { get; set; }
    }
}
