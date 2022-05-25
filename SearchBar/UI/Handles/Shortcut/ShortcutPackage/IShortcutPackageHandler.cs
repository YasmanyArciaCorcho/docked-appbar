using SearchBar.UI.Base;
using Services.ShortcutPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Handles.ShortcutPackage
{
    public interface IShortcutPackageHandler : IControlHandler
    {
        IShortcutPackageService ShortcutPackageService { get; set; }
    }

    public interface IShortcutPackageHandler<T> : IShortcutPackageHandler, IDashboardHandler<T> where T : IDashboard
    {

    }
}
