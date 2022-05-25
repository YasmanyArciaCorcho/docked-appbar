using Autofac;
using Services.Classifields;
using Services.Location.Cities;
using Services.Location.States;
using Stores.Providers.FaviconStore;
using Stores.Providers.History;
using Stores.Providers.Image;
using Stores.Providers.Image.Assets.AWS;
using Stores.Providers.Notes;
using Stores.Providers.PinShortcut;

namespace SearchBar.DC.Modules
{
   public class ModuleStores : Module
    {
        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterType<TrieHistoryProvider>().As<IHistoryProvider>().SingleInstance();
            builder.RegisterType<PinShortcutProvider>().As<IPinShortcutProvider>().SingleInstance();
            builder.RegisterType<USAStateProvider>().As<IStateProvider>().SingleInstance();
            builder.RegisterType<USACitiesProvider>().As<ICitiesProvider>().SingleInstance();
            builder.RegisterType<ClassifieldsSearchCategoryProvider>().As<IClassifieldsSearchCategoryProvider>().SingleInstance();
            builder.RegisterType<NotesSqlLiteProvider>().As<INotesProvider>().SingleInstance();
            builder.RegisterType<FaviconSqlLiteStore>().As<FaviconSqlLiteStore>().SingleInstance();
            builder.RegisterType<SVGAssetsASWImageProvider>().As<IImageProvider>().SingleInstance();
        }
    }
}
