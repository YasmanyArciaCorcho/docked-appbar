using SearchBar.UI.Builders.Image;
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

namespace SearchBar.UI.Controls.Dashboad.Maps.Categories
{
    /// <summary>
    /// Interaction logic for EarthMaps.xaml
    /// </summary>
    public partial class EarthMaps : UserControl
    {
        public EarthMaps(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            RemotePixel.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://search.remotepixel.ca/"); };
            Farearth.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("http://live.farearth.com/observer/"); };
            GoogleEarth.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://earth.google.com/web"); };

            Findstarlink.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://findstarlink.com/"); };
            Gse.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.gsat.us/tools/iridium-satellite-location-map-tool"); };
            Nasa.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.nasa.gov/mission_pages/hubble/main/index.html"); };
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "maps_{0}";
            imageSourceBuilder.SetImageSource(RemotePixel.Image, string.Format(imageNamespace, "remotepixel"));
            imageSourceBuilder.SetImageSource(Farearth.Image, string.Format(imageNamespace, "bingmaps"));
            imageSourceBuilder.SetImageSource(GoogleEarth.Image, string.Format(imageNamespace, "googleearth"));
            imageSourceBuilder.SetImageSource(Findstarlink.Image, string.Format(imageNamespace, "starlink"));
            imageSourceBuilder.SetImageSource(Gse.Image, string.Format(imageNamespace, "gse"));
            imageSourceBuilder.SetImageSource(Nasa.Image, string.Format(imageNamespace, "nasa"));
        }
    }
}
