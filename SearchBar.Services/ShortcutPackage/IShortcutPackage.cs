using Common.Shortcut;
using System.Collections.Generic;

namespace Services.ShortcutPackage
{
    public interface IShortcutPackage
    {
        string Name { get; set; }

        string PackageIconUrl { get; set; }

        List<IShortcut> Shortcuts { get; set; }
    }
}
