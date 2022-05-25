using Common.Shortcut;
using SearchBar.UI.Base.Shortcut;
using SearchBar.UI.Controls.Dashboad;
using SearchBar.UI.Handles.Shortcut;
using SearchBar.UI.Handles.Shortcut.PinShortcut;
using Services.ShortcutPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SearchBar.UI.Handles.ShortcutPackage
{
    public class ShortcutPackageHandler : IShortcutPackageHandler<MainDashBoard>
    {
        public MainDashBoard Dashboard { get; set; }
        public IShortcutPackageService ShortcutPackageService
        { get; set; }
        public IPinShortcutHandler _pinHandle;

        public ShortcutPackageHandler(IShortcutPackageService shortcutPackageService, IPinShortcutHandler pinHandle)
        {
            ShortcutPackageService = shortcutPackageService;
            _pinHandle = pinHandle;
        }

        public void Initialize()
        {
            Thread thre = new Thread(new ThreadStart(() =>
           {
               var packages = ShortcutPackageService.Packages;

               System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
               {
                   foreach (var pack in packages)
                   {
                       foreach (var shortcut in pack.Shortcuts)
                       {
                           var shortcutControl = await ShortcutBuilder.BuildShortcutAsync(shortcut.Name, shortcut.Url, false, Dashboard.WebBarViewModel.OpenBrowser, shortcut.Url,
                           (object sender, MouseButtonEventArgs e) => { _pinHandle.PintoBar(shortcut.Name, shortcut.Url, false, Dashboard.WebBarViewModel.OpenBrowser, shortcut.Url); });
                           Dashboard.ShortCutZone.shortcutsPackagePanel.Children.Add(shortcutControl);
                       }
                   }
               },
                   DispatcherPriority.Background);
               Dispatcher.Run();
           }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();
        }
    }
}
