using Common.Settings.Chromium;
using SearchBar.UI.Builders.Dashboard;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Dashboad;
using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Handles.Shortcut.PinShortcut;
using Common.ChromiumSettings;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchBar.UI.Handles.Widgets
{
    public class WidgetsPresentationHandler : IWidgetsPresentationHandler<WidgetsPresentationDashborad>
    {
        public WidgetsPresentationDashborad Dashboard
        { get; set; }
        public IPinShortcutHandler PinShortcutHandle
        { get; set; }

        readonly IDashboardControlBuilder _dashboardControlBuilder;
        readonly IImageSourceBuilder _imageSourceBuilder;
        readonly IChromiumSettingsService _settingsService;

        Dictionary<int, List<AppDashboardShorcut>> _shortcutPackages;
        private readonly string _defaultAppByIdd = null;

        public WidgetsPresentationHandler(
            IChromiumSettingsService settingsService,
            IDashboardControlBuilder dashboardControlBuilder,
            IImageSourceBuilder imageSourceBuilder)
        {
            _settingsService = settingsService;
            _dashboardControlBuilder = dashboardControlBuilder;
            _imageSourceBuilder = imageSourceBuilder;
            _defaultAppByIdd = _settingsService.GetImplementationId();
        }

        public void Initialize()
        {
            _shortcutPackages = new Dictionary<int, List<AppDashboardShorcut>>();

            //Dashboard.TitleZone.Children.Add(CreateTitleButton($"Pin your favorite applications", 0));
            Dashboard.TitleZone.Children.Add(new TextBlock()
            {
                FontSize = 18,
                Text = "Pin your favorite widgets",
                Foreground = Brushes.White
            });

            int currentIndex = 0;
            foreach (var dashboardName in _dashboardControlBuilder.GetListOfDashboard())
            {
                AddWidgetLink(dashboardName, _dashboardControlBuilder.GetWidgetImageLogoPath(dashboardName), currentIndex);
                currentIndex++;
            }
        }

        private void AddWidgetLink(string widgetName, string imagePath, int index)
        {
            _shortcutPackages.Add(index, new List<AppDashboardShorcut>());

            AppDashboardShorcut shorcut = new AppDashboardShorcut();
            shorcut.Name.Text = widgetName;
            _imageSourceBuilder.SetImageSource(shorcut.Imange, imagePath);
            _shortcutPackages[index].Add(shorcut);
            Dashboard.ShortcutContent.Children.Add(shorcut);

            shorcut.MouseLeftButtonDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            {
                bool isDefaultAppById = _defaultAppByIdd != null && _defaultAppByIdd.Equals(widgetName);

                if (shorcut.IsPinned)
                {
                    if (!isDefaultAppById)
                        PinShortcutHandle.UnPinApplicationtoBar(widgetName);
                }
                else
                {
                    PinShortcutHandle.PinApplicationtoBar(widgetName);
                }
            };
        }

        public string GetWidgetImageLogoPath(string widgetName)
        {
            if (ContainsApp(widgetName))
                return _dashboardControlBuilder.GetWidgetImageLogoPath(widgetName);
            return null;
        }

        public bool ContainsApp(string widgetName)
        => _dashboardControlBuilder.ContainsDashboardDefinition(widgetName);

        public UserControl GetWidgetDashboard(string widgetName)
        {
            return _dashboardControlBuilder.BuildDashboard(Dashboard.WebBarViewModel, widgetName);
        }

        public List<string> GetAllWidgetDashboard()
        {
            return _dashboardControlBuilder.GetListOfDashboard().ToList();
        }

        public UserControl GetDefaultWidgetByImplementationId()
        {
            if (ContainsApp(_defaultAppByIdd))
                return GetWidgetDashboard(_defaultAppByIdd);
            return null;
        }

        public bool ChangeAppPinStatus(string widgetName, bool pin)
        {
            int shortcutIndex = -1;
            int currentIndex = 0;

            foreach (var key in GetAllWidgetDashboard())
            {
                if (key.Equals(widgetName))
                {
                    shortcutIndex = currentIndex;
                    break;
                }

                currentIndex++;
            }

            if (shortcutIndex != -1)
            {
                _shortcutPackages[shortcutIndex][0].IsPinned = pin;
                return true;
            }
            return false;
        }
    }
}
