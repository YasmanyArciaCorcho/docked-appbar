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
    /// Interaction logic for EntertainmentDashboard.xaml
    /// </summary>
    public partial class EntertainmentDashboard : UserControl
    {
        public EntertainmentDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            cell20.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://edition.cnn.com/entertainment");
            cell30.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.foxnews.com/category/entertainment");
            cell40.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://abcnews.go.com/Entertainment/");
            cell50.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.bbc.com/news/entertainment_and_arts");
            cell22.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.cbsnews.com/entertainment/");
            cell32.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.cnbc.com/entertainment/");
            cell42.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.msn.com/en-us/entertainment/news");
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
