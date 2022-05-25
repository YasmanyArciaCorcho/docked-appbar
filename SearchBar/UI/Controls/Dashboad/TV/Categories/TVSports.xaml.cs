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
    /// Interaction logic for Sports.xaml
    /// </summary>
    public partial class TVSports : UserControl
    {
        public TVSports(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            cell20.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.espn.co.uk/watch/");
            cell30.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://plus.espn.com/");
            cell40.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.nflgamepass.com/");
            cell50.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.foxsports.com/live");
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "tv_{0}";
            imageSourceBuilder.SetImageSource(cell20.Imange, string.Format(imageNamespace, "espn"));
            imageSourceBuilder.SetImageSource(cell30.Imange, string.Format(imageNamespace, "espnplus"));
            imageSourceBuilder.SetImageSource(cell40.Imange, string.Format(imageNamespace, "nfl"));
            imageSourceBuilder.SetImageSource(cell50.Imange, string.Format(imageNamespace, "foxsport"));
        }
    }
}
