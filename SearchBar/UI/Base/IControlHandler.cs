using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Base
{
    /// <summary>
    /// Todo: do a better hierarchy around the elements that can be added, modified, removed.
    /// Faster version to get alive the mvp.
    /// </summary>
  public  interface IControlHandler
    {
        void Initialize();
    }
}
