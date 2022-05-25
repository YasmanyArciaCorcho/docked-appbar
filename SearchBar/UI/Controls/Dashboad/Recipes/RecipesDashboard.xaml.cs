using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Handles.Recipes;
using SearchBar.UI.WebBar;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SearchBar.UI.Controls.Dashboad.Recipes
{
    /// <summary>
    /// Interaction logic for RecipesDashboard.xaml
    /// </summary>
    public partial class RecipesDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Recipes";
        public static string ImagePath = "recipes_logo";
        public WebBarViewModel WebBarViewModel
        { get; set; }

        const string _searchBarText = "Search {0}";
        Border _previousSelected;
        static Brush _selectedColorBrush;

        public RecipesDashboard(WebBarViewModel webBarViewModel, IRecipesHandler<RecipesDashboard> recipesHandler, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            WebBarViewModel = webBarViewModel;
            _selectedColorBrush = new SolidColorBrush(Color.FromRgb(151, 187, 30));
            recipesHandler.Dashboard = this;
            recipesHandler.Initialize();

            Food.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { UpdateSearchEngine(Food, "Food", "https://www.food.com/search/{0}"); };
            FoodNetwork.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { UpdateSearchEngine(FoodNetwork, "Food Network", "https://www.foodnetwork.com/search/{0}-"); };
            AllRecipes.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { UpdateSearchEngine(AllRecipes, "AllRecipes", "https://www.allrecipes.com/search/results/?wt={0}"); };
            RecipeSearch.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { UpdateSearchEngine(RecipeSearch, "Recipe Search", "https://s.recipesearch-serp.info/public/home.html?q={0}"); };

            _previousSelected = Food;
            UpdateSearchEngine(Food, "Food", "https://www.food.com/search/{0}");

            Keto.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => WebBarViewModel.OpenDirectUrlBrowser("https://www.dketodiet.trimdownclub.com/");
            Noom.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => WebBarViewModel.OpenDirectUrlBrowser("https://www.noom.com/");
            EatingWell.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => WebBarViewModel.OpenDirectUrlBrowser("http://www.eatingwell.com/");
            Uber.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => WebBarViewModel.OpenDirectUrlBrowser("https://www.ubereats.com");
            Doordash.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => WebBarViewModel.OpenDirectUrlBrowser("https://www.doordash.com/en-US");
            PostMates.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => WebBarViewModel.OpenDirectUrlBrowser("https://postmates.com");

        }

        private void UpdateSearchEngine(Border sender, string name, string senderUrl)
        {
            SearchBar.PlaceHolderTextBlock.Text = string.Format(_searchBarText, name);
            SearchBar.QueryToComplete = senderUrl;

            _previousSelected.Background = sender.Background;
            _previousSelected = sender;
            sender.Background = _selectedColorBrush;
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "recipes_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(foodImage, string.Format(imageNamespace, "food"));
            imageSourceBuilder.SetImageSource(allrecipesImage, string.Format(imageNamespace, "allrecipes"));
            imageSourceBuilder.SetImageSource(foodnetworkImage, string.Format(imageNamespace, "foodnetwork"));
            imageSourceBuilder.SetImageSource(recipesearchImage, string.Format(imageNamespace, "recipesearch"));
            imageSourceBuilder.SetImageSource(Keto.Image, string.Format(imageNamespace, "keto"));
            imageSourceBuilder.SetImageSource(Noom.Image, string.Format(imageNamespace, "noom"));
            imageSourceBuilder.SetImageSource(EatingWell.Image, string.Format(imageNamespace, "eatwell"));
            imageSourceBuilder.SetImageSource(Uber.Image, string.Format(imageNamespace, "ubereats"));
            imageSourceBuilder.SetImageSource(Doordash.Image, string.Format(imageNamespace, "doordash"));
            imageSourceBuilder.SetImageSource(PostMates.Image, string.Format(imageNamespace, "postmates"));
        }
    }
}
