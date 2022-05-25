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
    /// Interaction logic for TravelMaps.xaml
    /// </summary>
    public partial class TravelMaps : UserControl
    {
        public TravelMaps(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            Booking.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.booking.com/"); };
            Kayak.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.kayak.com/"); };
            Expedia.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.expedia.com/"); };

            Hotels.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://ssl.hotels.com/"); };
            Marriott.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.marriott.com/default.mi"); };
            Vrbo.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.vrbo.com/"); };

            Alamo.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.alamo.com/en_US/car-rental/home.html"); };
            NationalCar.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.nationalcar.com/en/home.html"); };
            Enterprise.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.enterprise.com/en/home.html"); };

            Yelp.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.yelp.com/search?cflt=restaurants"); };
            Tripadvisor.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.tripadvisor.com/Restaurants"); };
            YellowPages.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.yellowpages.com/restaurants"); };

            TravelandLeisure.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("http://travelandleisure.com/travel-guide"); };
            Nomadic.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.nomadicmatt.com/travel-guides/"); };
            Fodors.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.fodors.com/"); };

            DanFlaying.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.danflyingsolo.com/"); };
            BrokenBackpack.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://abrokenbackpack.com/"); };
            Lili.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.lilistravelplans.com/"); };
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "maps_{0}";
            imageSourceBuilder.SetImageSource(Booking.Image, string.Format(imageNamespace, "booking"));
            imageSourceBuilder.SetImageSource(Kayak.Image, string.Format(imageNamespace, "kayak"));
            imageSourceBuilder.SetImageSource(Expedia.Image, string.Format(imageNamespace, "expedia"));
            imageSourceBuilder.SetImageSource(Hotels.Image, string.Format(imageNamespace, "hotel"));
            imageSourceBuilder.SetImageSource(Marriott.Image, string.Format(imageNamespace, "marriott"));
            imageSourceBuilder.SetImageSource(Vrbo.Image, string.Format(imageNamespace, "vrbo"));
            imageSourceBuilder.SetImageSource(Alamo.Image, string.Format(imageNamespace, "alamo"));
            imageSourceBuilder.SetImageSource(NationalCar.Image, string.Format(imageNamespace, "nationalcar"));
            imageSourceBuilder.SetImageSource(Enterprise.Image, string.Format(imageNamespace, "enterprise"));
            imageSourceBuilder.SetImageSource(Yelp.Image, string.Format(imageNamespace, "yelp"));
            imageSourceBuilder.SetImageSource(Tripadvisor.Image, string.Format(imageNamespace, "tripadvisor"));
            imageSourceBuilder.SetImageSource(YellowPages.Image, string.Format(imageNamespace, "yellowpages"));
            imageSourceBuilder.SetImageSource(TravelandLeisure.Image, string.Format(imageNamespace, "travelleisure"));
            imageSourceBuilder.SetImageSource(Nomadic.Image, string.Format(imageNamespace, "nomadicmatt"));
            imageSourceBuilder.SetImageSource(Fodors.Image, string.Format(imageNamespace, "fodors"));
            imageSourceBuilder.SetImageSource(DanFlaying.Image, string.Format(imageNamespace, "danflying"));
            imageSourceBuilder.SetImageSource(BrokenBackpack.Image, string.Format(imageNamespace, "brokenbackpack"));
            imageSourceBuilder.SetImageSource(Lili.Image, string.Format(imageNamespace, "lili"));
        }
    }
}
