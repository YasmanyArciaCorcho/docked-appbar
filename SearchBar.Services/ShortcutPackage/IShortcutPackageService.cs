using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ShortcutPackage
{
    public interface IShortcutPackageService
    {
        List<IShortcutPackage> Packages { get; }
    }
}
