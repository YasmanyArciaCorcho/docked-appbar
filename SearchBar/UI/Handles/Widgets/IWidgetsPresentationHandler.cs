using SearchBar.UI.Base;
using SearchBar.UI.Builders.Dashboard;
using SearchBar.UI.Handles.Shortcut.PinShortcut;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SearchBar.UI.Handles.Widgets
{
    public interface IWidgetsPresentationHandler : IControlHandler
    {
        bool ContainsApp(string widgetName);

        string GetWidgetImageLogoPath(string widgetName);

        bool ChangeAppPinStatus(string widgetName, bool pin);

        UserControl GetDefaultWidgetByImplementationId();

        UserControl GetWidgetDashboard(string widgetName);

        List<string> GetAllWidgetDashboard();

        IPinShortcutHandler PinShortcutHandle { get; set; }
    }

    public interface IWidgetsPresentationHandler<T> : IWidgetsPresentationHandler, IDashboardHandler<T> where T : IDashboard
    {

    }
}
