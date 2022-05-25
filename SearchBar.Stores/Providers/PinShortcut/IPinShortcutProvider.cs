using Common.Shortcut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers.PinShortcut
{
    public interface IPinShortcutProvider : IProvider
    {
        List<IShortcut> Shortcuts
        { get; set; }

        void AddShortCut(string name, string url = "", bool isApp = false);

        void RemoveShortCut(string name);
    }
}
