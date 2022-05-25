using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Base
{
    public interface IDashboard
    {
        public WebBarViewModel WebBarViewModel
        { get; set; }
    }
}
