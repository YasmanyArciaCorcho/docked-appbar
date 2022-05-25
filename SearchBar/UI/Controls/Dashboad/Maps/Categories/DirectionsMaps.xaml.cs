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
    /// Interaction logic for DirectionsMaps.xaml
    /// </summary>
    public partial class DirectionsMaps : UserControl
    {
        public DirectionsMaps(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            GoogleMaps.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.google.com/maps/"); };
            Bingmaps.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.bing.com/maps"); };
            WeGo.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://wego.here.com/"); };

            Flighttime.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://flighttime-calculator.com/"); };
            FlightView.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.flightview.com/traveltools/"); };

            TrainTravel.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.seat61.com/UnitedStates.htm"); };
            OpenRail.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://openrailwaymap.org/"); };
            Virail.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.virail.com/train-times-usa"); };
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "maps_{0}";
            imageSourceBuilder.SetImageSource(GoogleMaps.Image, string.Format(imageNamespace, "googlemap"));
            imageSourceBuilder.SetImageSource(Bingmaps.Image, string.Format(imageNamespace, "bingmaps"));
            imageSourceBuilder.SetImageSource(WeGo.Image, string.Format(imageNamespace, "wego"));
            imageSourceBuilder.SetImageSource(Flighttime.Image, string.Format(imageNamespace, "flighttime"));
            imageSourceBuilder.SetImageSource(FlightView.Image, string.Format(imageNamespace, "flightview"));
            imageSourceBuilder.SetImageSource(TrainTravel.Image, string.Format(imageNamespace, "61"));
            imageSourceBuilder.SetImageSource(OpenRail.Image, string.Format(imageNamespace, "rail"));
            imageSourceBuilder.SetImageSource(Virail.Image, string.Format(imageNamespace, "virail"));
        }
    }
}
