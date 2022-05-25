using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using SearchBar.UI.Base.Shortcut;
using SearchBar.UI.Controls.Dashboad;
using SearchBar.UI.Controls.ShortCut;
using Store.Models;
using Common.Logger;
using Stores.Providers.TopSiteStore;

namespace SearchBar.UI.Handles.TopSites
{
    public class TopSitesHandler : ITopSitesHandler<MainDashBoard>
    {
        List<string> TopSiteRegistered { get; set; }

        public TopSitesHandler()
        {
            TopSiteRegistered = new List<string>();
        }

        public void Initialize(MainDashBoard dashboard)
        {
            Thread thre = new Thread(new ThreadStart(() =>
           {
               List<TopSiteModel> topSites = new TopSiteSqlLiteStore().GetTopSites().ToList();

               System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
               {
                   foreach (var site in topSites)
                   {
                       try
                       {
                           if (!TopSiteRegistered.Contains(site.Title))
                           {
                               ShortcutControl shortcut = await ShortcutBuilder.BuildShortcutAsync(
                                   site.Title, site.Url, false,
                                   dashboard.WebBarViewModel.OpenBrowser, site.Url);

                               shortcut.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                               shortcut.ToolTip = site.Url;

                               dashboard.TopSitesZonePanel.Children.Add(shortcut);

                               TopSiteRegistered.Add(site.Title);
                           }
                       }
                       catch (Exception e)
                       {
                           StaticLogger.Logger.Error($"Fatal error running AddUserControl in TopSitesHandler: {e}");
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
