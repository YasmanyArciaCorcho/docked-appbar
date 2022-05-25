using Common.Shortcut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ShortcutPackage
{
    public interface ICommonVerticalLinksService
    {
        IEnumerable<IShortcut> GetCommonShortcuts();
    }
}
