using SearchBar.UI.Builders.Image;
using SearchBar.UI.Handles;
using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBar.UI.Controls.Dashboad.News.Categories
{
    /// <summary>
    /// Interaction logic for BusinessDashboard.xaml
    /// </summary>
    public partial class BusinessDashboard : UserControl
    {
        public BusinessDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            cell20.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://edition.cnn.com/business");
            cell30.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.foxbusiness.com/");
            cell40.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://abcnews.go.com/Business/");
            cell50.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.bbc.com/news/business");
            cell22.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.cbsnews.com/evening-news/business/");
            cell32.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.cnbc.com/business/");
            cell42.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.nbcnews.com/business");
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "tv_{0}";
            imageSourceBuilder.SetImageSource(cell20.Imange, string.Format(imageNamespace, "cnn"));
            imageSourceBuilder.SetImageSource(cell30.Imange, string.Format(imageNamespace, "foxnews"));
            imageSourceBuilder.SetImageSource(cell40.Imange, string.Format(imageNamespace, "abc"));
            imageSourceBuilder.SetImageSource(cell50.Imange, string.Format(imageNamespace, "bbc"));
            imageSourceBuilder.SetImageSource(cell22.Imange, string.Format(imageNamespace, "cbs"));
            imageSourceBuilder.SetImageSource(cell32.Imange, "news_cnbc");
            imageSourceBuilder.SetImageSource(cell42.Imange, string.Format(imageNamespace, "msnbc"));
        }
    }
}
