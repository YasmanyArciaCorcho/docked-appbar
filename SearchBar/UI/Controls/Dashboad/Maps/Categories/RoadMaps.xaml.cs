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
    /// Interaction logic for RoadMaps.xaml
    /// </summary>
    public partial class RoadMaps : UserControl
    {
        public RoadMaps(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            GasBuddy.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.gasbuddy.com/gaspricemap"); };
            Autoblog.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("http://autoblog.com/cheap-gas-prices/"); };
            Fuel.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.fueleconomy.gov/"); };

            GEICO.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.geico.com"); };
            MyQuickMaps.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("http://myquickmaps.org/the-truth-about-insurance-companies"); };
            LibertyMutual.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.libertymutual.com/"); };

            Yelp.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.yelp.com/search?find_desc=Roadside+Assistance"); };
            AAA.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.aaa.com/"); };
            RoadSide.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.roadsideassistance24.com/"); };

            Waze.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.waze.com/"); };
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "maps_{0}";
            imageSourceBuilder.SetImageSource(Autoblog.Image, string.Format(imageNamespace, "gasbuddy"));
            imageSourceBuilder.SetImageSource(Autoblog.Image, string.Format(imageNamespace, "autoblog"));
            imageSourceBuilder.SetImageSource(GEICO.Image, string.Format(imageNamespace, "fueleconomy"));
            imageSourceBuilder.SetImageSource(GEICO.Image, string.Format(imageNamespace, "geico"));
            imageSourceBuilder.SetImageSource(MyQuickMaps.Image, string.Format(imageNamespace, "myquickmaps"));
            imageSourceBuilder.SetImageSource(LibertyMutual.Image, string.Format(imageNamespace, "liberty"));
            imageSourceBuilder.SetImageSource(Yelp.Image, string.Format(imageNamespace, "yelp"));
            imageSourceBuilder.SetImageSource(AAA.Image, string.Format(imageNamespace, "aaa"));
            imageSourceBuilder.SetImageSource(RoadSide.Image, string.Format(imageNamespace, "roadsideAssistance"));
            imageSourceBuilder.SetImageSource(Waze.Image, string.Format(imageNamespace, "waze"));
        }
    }
}
