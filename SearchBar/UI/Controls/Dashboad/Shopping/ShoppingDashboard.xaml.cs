using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.WebBar;
using System.Windows.Controls;
using System.Windows.Input;

namespace SearchBar.UI.Controls.Dashboad.Shopping
{
    /// <summary>
    /// Interaction logic for ShoppingDashboard.xaml
    /// </summary>
    public partial class ShoppingDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Shopping";
        public static string ImagePath = "shopping_logo";

        public WebBarViewModel WebBarViewModel { get; set; }
        public ShoppingDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            WebBarViewModel = webBarViewModel;

            Amazon.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.amazon.com/"); };
            Walmart.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.walmart.com/"); };
            HomeDepot.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.homedepot.com/"); };
            Pricegrabber.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("http://www.pricegrabber.com/"); };
            Retailmenot.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.retailmenot.com/"); };
            Shopzila.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("http://www.shopzilla.com/"); };
            Groupon.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("http://www.groupon.com/"); };
            Ebay.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("http://www.ebay.com/"); };
            ButtonSearch.PreviewMouseLeftButtonDown += ButtonSearch_PreviewMouseLeftButtonDown;
        }

        public void ButtonSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WebBarViewModel.OpenDirectUrlBrowser($"https://www.amazon.com/s?k={Keyword.Text}");
            Keyword.Text = "";
        }

        private void Keyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonSearch_PreviewMouseLeftButtonDown(sender, null);
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "shopping_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(Amazon.Image, string.Format(imageNamespace, "amazon"));
            imageSourceBuilder.SetImageSource(Walmart.Image, string.Format(imageNamespace, "walmart"));
            imageSourceBuilder.SetImageSource(HomeDepot.Image, string.Format(imageNamespace, "thehomedepot"));
            imageSourceBuilder.SetImageSource(Ebay.Image, string.Format(imageNamespace, "ebay"));
            imageSourceBuilder.SetImageSource(Pricegrabber.Image, string.Format(imageNamespace, "pricegrabber"));
            imageSourceBuilder.SetImageSource(Retailmenot.Image, string.Format(imageNamespace, "retailmenot"));
            imageSourceBuilder.SetImageSource(Shopzila.Image, string.Format(imageNamespace, "shopzilla"));
            imageSourceBuilder.SetImageSource(Groupon.Image, string.Format(imageNamespace, "groupon"));
        }
    }
}
