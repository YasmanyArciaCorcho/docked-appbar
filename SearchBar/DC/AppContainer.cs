using System;
using Autofac;
using SearchBar.UI.Controls.Dashboad;
using SearchBar.UI.Controls.Dashboad.Recipes;
using SearchBar.UI.Controls.Shortcut;
using SearchBar.UI.Handles.Bookmarks;
using SearchBar.UI.Handles.News;
using SearchBar.UI.Handles.Recipes;
using SearchBar.UI.Handles.Shortcut.PinShortcut;
using SearchBar.UI.Handles.ShortcutPackage;
using SearchBar.UI.Handles.TopSites;
using SearchBar.UI.SearchBarViewModel;
using SearchBar.UI.WebBar;
using SearchBar.UI.Handles.Games;
using SearchBar.UI.Controls.Dashboad.Games;
using SearchBar.UI.Handles.Classifields;
using SearchBar.UI.Controls.Dashboad.Classifieds;
using SearchBar.UI.Builders.Dashboard;
using Common.Logger;
using Common.Settings.Chromium;
using Common;
using SearchBar.UI.Handles.Widgets;
using SearchBar.UI.Handles.Notes;
using SearchBar.UI.Controls.Dashboad.Notes;
using Common.ChromiumSettings;
using Common.Settings.UserIp;
using Common.Settings.Chromium.AWSElastic;
using SearchBar.DC.Modules;
using SearchBar.UI.Builders.Image;

namespace SearchBar.DC
{
    public static class AppContainer
    {
        private static ILifetimeScope _currentScope;
        public static ILifetimeScope CurrentScope
        {
            get
            {
                return GetCurrentScope();
            }
        }

        public static IContainer Container
        { get; private set; }

        public static IContainer Configure()
        {
            StaticLogger.Logger.Trace("ContainerConfig - configure started");

            try
            {
                var builder = new ContainerBuilder();

                IcanhazipResolver userIpResolver = new IcanhazipResolver();
                AWSElasticChromiumUrlFixed aWSElasticChromiumUrlFixed = new AWSElasticChromiumUrlFixed(userIpResolver);
                AWSElasticAppSettingsInitializer appSettingsInitializer = new AWSElasticAppSettingsInitializer();
                builder.Register(c => userIpResolver).As<IUserExternalIpResolver>().SingleInstance();
                builder.Register(c => aWSElasticChromiumUrlFixed).As<IChromiumUrlFixed>().SingleInstance();
                builder.Register(c => appSettingsInitializer).As<IAppSettingsInitializer>().SingleInstance();
                builder.Register(c => new ChromiumSettingsService(aWSElasticChromiumUrlFixed, appSettingsInitializer)).As<IChromiumSettingsService>().SingleInstance();

                builder.RegisterModule(new ModuleServices());
                builder.RegisterModule(new ModuleStores());

                builder.RegisterType<DashboardControlBuilder>().As<IDashboardControlBuilder>();
                builder.RegisterType<SvgImageSourceBuilder>().As<IImageSourceBuilder>().SingleInstance();
                builder.RegisterType<MainWindow>().SingleInstance();
                builder.RegisterType<MainDashBoard>().SingleInstance();
                builder.RegisterType<ActionTrayShortcut>().As<ActionTrayShortcut>().SingleInstance();
                builder.RegisterType<SearchBarViewModel>().As<ISearchBarViewModel>().SingleInstance();
                builder.RegisterType<WebBarViewModel>().SingleInstance();
                builder.RegisterType<WidgetsPresentationHandler>().As<IWidgetsPresentationHandler<WidgetsPresentationDashborad>, IWidgetsPresentationHandler>().SingleInstance();
                builder.RegisterType<BookmarkHandler>().As<IBookmarkHandler>().SingleInstance();
                builder.RegisterType<NewsHandler>().As<INewsHandler<MainDashBoard>>().SingleInstance();
                builder.RegisterType<ShortcutPackageHandler>().As<IShortcutPackageHandler<MainDashBoard>>().SingleInstance();
                builder.RegisterType<TopSitesHandler>().As<ITopSitesHandler<MainDashBoard>>().SingleInstance();
                builder.RegisterType<PinShortcutHandler>().As<IPinShortcutHandler>().SingleInstance();
                builder.RegisterType<RecipesHandler>().As<IRecipesHandler<RecipesDashboard>>().SingleInstance();
                builder.RegisterType<GamesHandler>().As<IGamesHandler<GamesDashboard>>().SingleInstance();
                builder.RegisterType<ClassifieldHandler>().As<IClassifieldHandler<ClassifiedsDashboard>>().SingleInstance();
                builder.RegisterType<NotesHandler>().As<INotesHandler<NotesDashboard>>().SingleInstance();

                Container = builder.Build();
                if (_currentScope == null)
                    _currentScope = Container.BeginLifetimeScope();
                return Container;
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                throw e;
            }
        }

        static readonly object _scopeLock = new object();
        public static ILifetimeScope GetCurrentScope()
        {
            if (_currentScope != null)
                return _currentScope;
            lock (_scopeLock)
            {
                if (_currentScope == null)
                {
                    _currentScope = Configure().BeginLifetimeScope();
                }
                return _currentScope;
            }
        }
    }
}