using Common.GarbageCollector;
using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchBar.UI.Builders.Dashboard
{
    public interface IDashboardControlBuilder
    {
        string GetWidgetImageLogoPath(string dashboardName);

        bool ContainsDashboardDefinition(string dashboardName);

        IEnumerable<string> GetListOfDashboard();

        UserControl BuildDashboard(WebBarViewModel webBarView, string dashboardName);
    }

    public interface IDashboardControlBuilderGarbageCollector<TKey, TValue> : IDashboardControlBuilder where TKey : IComparable<TKey>
    {
        ICollectionGarbageCollector<TKey, TValue> Collection
        { get; }
    }
}
