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

namespace SearchBar.UI.Controls.Dashboad.TV.Categories
{
    /// <summary>
    /// Interaction logic for Streaming.xaml
    /// </summary>
    public partial class TVStreaming : UserControl
    {
        public TVStreaming(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            cell20.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.netflix.com/");
            cell30.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.youtube.com/");
            cell40.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.hulu.com/");
            cell50.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.primevideo.com/");
            cell60.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.cbs.com/");
            cell22.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.hbo.com/");
            cell32.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.sho.com/");
            cell42.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.fubo.tv/welcome");
            cell52.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://corporate.pluto.tv/");
            cell62.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.disneyplus.com/home");
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "tv_{0}";
            imageSourceBuilder.SetImageSource(cell20.Imange, string.Format(imageNamespace, "netflix"));
            imageSourceBuilder.SetImageSource(cell30.Imange, string.Format(imageNamespace, "youtube"));
            imageSourceBuilder.SetImageSource(cell40.Imange, string.Format(imageNamespace, "hulu"));
            imageSourceBuilder.SetImageSource(cell50.Imange, string.Format(imageNamespace, "prime_video"));
            imageSourceBuilder.SetImageSource(cell60.Imange, string.Format(imageNamespace, "cbs"));
            imageSourceBuilder.SetImageSource(cell22.Imange, string.Format(imageNamespace, "hbo"));
            imageSourceBuilder.SetImageSource(cell32.Imange, string.Format(imageNamespace, "showtime"));
            imageSourceBuilder.SetImageSource(cell42.Imange, string.Format(imageNamespace, "fubotv"));
            imageSourceBuilder.SetImageSource(cell52.Imange, string.Format(imageNamespace, "plutotv"));
            imageSourceBuilder.SetImageSource(cell62.Imange, string.Format(imageNamespace, "disney"));
        }
    }
}
