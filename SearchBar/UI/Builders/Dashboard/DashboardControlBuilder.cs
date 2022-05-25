using Autofac;
using Common.GarbageCollector;
using SearchBar.DC;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Dashboad.Classifieds;
using SearchBar.UI.Controls.Dashboad.Converter;
using SearchBar.UI.Controls.Dashboad.Dictionary;
using SearchBar.UI.Controls.Dashboad.Email;
using SearchBar.UI.Controls.Dashboad.Form;
using SearchBar.UI.Controls.Dashboad.Games;
using SearchBar.UI.Controls.Dashboad.Maps;
using SearchBar.UI.Controls.Dashboad.News;
using SearchBar.UI.Controls.Dashboad.Notes;
using SearchBar.UI.Controls.Dashboad.Package;
using SearchBar.UI.Controls.Dashboad.Recipes;
using SearchBar.UI.Controls.Dashboad.Shopping;
using SearchBar.UI.Controls.Dashboad.TV;
using SearchBar.UI.Controls.Dashboad.Weather;
using SearchBar.UI.Handles;
using SearchBar.UI.Handles.Classifields;
using SearchBar.UI.Handles.Games;
using SearchBar.UI.Handles.Notes;
using SearchBar.UI.Handles.Recipes;
using SearchBar.UI.WebBar;
using Services.Weather;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SearchBar.UI.Builders.Dashboard
{
    public class DashboardControlBuilder : IDashboardControlBuilderGarbageCollector<string, UserControl>
    {
        Dictionary<string, IDashboardControl> _dashboardControls;

        public ICollectionGarbageCollector<string, UserControl> Collection
        { get; private set; }

        readonly IImageSourceBuilder _imageSourceBuilder;

        public DashboardControlBuilder()
        {
            Collection = new SplayCollection<string, UserControl>();
            _imageSourceBuilder = AppContainer.CurrentScope.Resolve<IImageSourceBuilder>();
            _dashboardControls = EmbeddedDashboardControls();

            Initialize();
        }

        private Dictionary<string, IDashboardControl> EmbeddedDashboardControls()
        {
            return new Dictionary<string, IDashboardControl>
                {
                { ClassifiedsDashboard.DashboardName, new DashboardControl(ClassifiedsDashboard.DashboardName, ClassifiedsDashboard.ImagePath, view => new ClassifiedsDashboard(view, AppContainer.CurrentScope.Resolve<IClassifieldHandler<ClassifiedsDashboard>>(), _imageSourceBuilder))},
                { ConverterDashboard.DashboardName, new DashboardControl(ConverterDashboard.DashboardName, ConverterDashboard.ImagePath, view => new ConverterDashboard(view, _imageSourceBuilder))},
                { DictionaryDashboard.DashboardName, new DashboardControl(DictionaryDashboard.DashboardName, DictionaryDashboard.ImagePath, view => new DictionaryDashboard(view, _imageSourceBuilder))},
                { EmailDashboard.DashboardName, new DashboardControl(EmailDashboard.DashboardName, EmailDashboard.ImagePath, view => new EmailDashboard(view, _imageSourceBuilder))},
                { FormsDashboard.DashboardName, new DashboardControl(FormsDashboard.DashboardName, FormsDashboard.ImagePath, view => new FormsDashboard(view, _imageSourceBuilder)) },
                { GamesDashboard.DashboardName, new DashboardControl(GamesDashboard.DashboardName, GamesDashboard.ImagePath, view => new GamesDashboard(view, AppContainer.CurrentScope.Resolve<IGamesHandler<GamesDashboard>>(), _imageSourceBuilder))},
                { PackageDashboard.DashboardName, new DashboardControl(PackageDashboard.DashboardName, PackageDashboard.ImagePath, view => new PackageDashboard(view, _imageSourceBuilder))},
                { RecipesDashboard.DashboardName, new DashboardControl(RecipesDashboard.DashboardName, RecipesDashboard.ImagePath, view => new RecipesDashboard(view, AppContainer.CurrentScope.Resolve<IRecipesHandler<RecipesDashboard>>(), _imageSourceBuilder))},
                { MapsDashboard.DashboardName, new DashboardControl(MapsDashboard.DashboardName, MapsDashboard.ImagePath, view => new MapsDashboard(view, _imageSourceBuilder))},
                { NewsDashboard.DashboardName,new DashboardControl(NewsDashboard.DashboardName, NewsDashboard.ImagePath, view => new NewsDashboard(view, _imageSourceBuilder))},
                { NotesDashboard.DashboardName, new DashboardControl(NotesDashboard.DashboardName, NotesDashboard.ImagePath, view => new NotesDashboard(view, AppContainer.CurrentScope.Resolve<INotesHandler<NotesDashboard>>(), _imageSourceBuilder))},
                { ShoppingDashboard.DashboardName, new DashboardControl(ShoppingDashboard.DashboardName, ShoppingDashboard.ImagePath, view => new ShoppingDashboard(view, _imageSourceBuilder)) },
                { TVDashboard.DashboardName, new DashboardControl(TVDashboard.DashboardName, TVDashboard.ImagePath, view => new TVDashboard(view, _imageSourceBuilder))},
                { WeatherDashboard.DashboardName, new DashboardControl(WeatherDashboard.DashboardName, WeatherDashboard.ImagePath, view => new WeatherDashboard(view, AppContainer.CurrentScope.Resolve<IWeatherService>(), _imageSourceBuilder))},
            };
        }

        public UserControl BuildDashboard(WebBarViewModel webBarView, string dashboardName)
        {
            if (Collection.ContainsKey(dashboardName))
            {
                return Collection[dashboardName];
            }
            else
            {
                if (_dashboardControls.ContainsKey(dashboardName))
                {
                    UserControl dashboard = _dashboardControls[dashboardName].Build(webBarView);

                    Collection.Add(dashboardName, dashboard);

                    return dashboard;
                }
            }
            return null;
        }

        public string GetWidgetImageLogoPath(string dashboardName)
        {
            if (_dashboardControls.ContainsKey(dashboardName))
                return _dashboardControls[dashboardName].ImagePath;
            return string.Empty;
        }

        public IEnumerable<string> GetListOfDashboard()
        {
            foreach (var dashboardControl in _dashboardControls)
                yield return dashboardControl.Key;
        }

        public bool ContainsDashboardDefinition(string dashboardName)
            => _dashboardControls.ContainsKey(dashboardName);

        /// <summary>
        /// Load dashboards resources.
        /// </summary>
        private void Initialize()
        {
            new Thread(async () =>
           {
               Thread.CurrentThread.IsBackground = true;
               await LoadDashboardImages();
           }).Start();
        }

        private async Task LoadDashboardImages()
        {
            foreach (var dashboardName in GetListOfDashboard())
            {
                await _imageSourceBuilder.AddModuleNamespace(dashboardName);
            }
        }
    }
}
