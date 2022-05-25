using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Common;
using Common.Logger;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Dashboad.Recipes;
using Services.Recipes;

namespace SearchBar.UI.Handles.Recipes
{
    public class RecipesHandler : IRecipesHandler<RecipesDashboard>
    {
        public IRecipesService RecipesService
        { get; set; }

        public RecipesDashboard Dashboard
        { get; set; }

        public RecipesHandler(IRecipesService recipesService)
        {
            RecipesService = recipesService;
        }

        public void Initialize()
        {
            Thread thre = new Thread(new ThreadStart(async () =>
            {
                List<INewsFormat> recipes = await RecipesService.GetRecipes();
                Dictionary<INewsFormat, byte[]> images = new Dictionary<INewsFormat, byte[]>();
                foreach (var site in recipes)
                    images.Add(site, HTTPRequestHelper.GetFileFromURL(site.ImagePath));

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
               {
                   foreach (var site in recipes)
                   {
                       try
                       {
                           NewsFormatControl newControl = new NewsFormatControl();
                           newControl.Image.Source = ImageHandler.BytesToImage(images[site]);
                           newControl.DataContext = site;
                           newControl.PreviewMouseLeftButtonDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                           {
                               Dashboard.WebBarViewModel.OpenDirectUrlBrowser(site.Url);
                           };
                           Dashboard.DailyPickForYouZone.Children.Add(newControl);

                       }
                       catch (Exception e)
                       {
                           StaticLogger.Logger.Error($"Fatal error running AddUserControl in RecipesHandler: {e}");
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
