using Autofac;
using Services.Autocomplete;
using Services.Autocomplete.OnlineSuggestion;
using Services.Autocomplete.OnlineSuggestion.Google;
using Services.Bookmark;
using Services.Bookmark.Importer;
using Services.Browser;
using Services.Classifields;
using Services.ComputerManagement;
using Services.Games;
using Services.Location;
using Services.News;
using Services.Recipes;
using Services.ShortcutPackage;
using Services.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SearchBar.DC.Modules
{
    public class ModuleServices : Module
    {
        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterType<BrowserService>().As<IBrowserService>();
            builder.RegisterType<GoogleSearchSuggestionsService>().As<IOnlineSuggestionService>().SingleInstance();
            builder.RegisterType<BookmarkService>().As<IBookmarkService>().SingleInstance();
            builder.RegisterType<NewsService>().As<INewsService>().SingleInstance();
            builder.RegisterType<SearchAutocompleteService>().As<ISearchAutocompleteService>().SingleInstance();
            builder.RegisterType<ShortcutPackageService>().As<IShortcutPackageService>().SingleInstance();
            builder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
            builder.RegisterType<WeatherService>().As<IWeatherService>().SingleInstance();
            builder.RegisterType<RecipesService>().As<IRecipesService>().SingleInstance();
            builder.RegisterType<ScreenLightServices>().As<IScreenLightService>().SingleInstance();
            builder.RegisterType<BasicCommonVerticalLinksService>().As<ICommonVerticalLinksService>().SingleInstance();
            builder.RegisterType<GamesService>().As<IGamesService>().SingleInstance();
            builder.RegisterType<ClassifieldsService>().As<IClassifieldsService>().SingleInstance();
            builder.RegisterType<JsonBookmarkImporter>().As<IBookmarkImporter>().SingleInstance();
        }
    }
}
