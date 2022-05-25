using SearchBar.UI.Base;
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

namespace SearchBar.UI.Controls.Dashboad.Package
{
    /// <summary>
    /// Interaction logic for PackageDashboard.xaml
    /// </summary>
    public partial class PackageDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Package";
        public static string ImagePath = "package_logo";

        public string[] TrackingSourceName
        { get; set; }
        public WebBarViewModel WebBarViewModel
        { get; set; }

        readonly Dictionary<string, string> _trackingOptions;

        public PackageDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            _trackingOptions = new Dictionary<string, string>()
            {
                {"Amazon","http://parcelsapp.com/en/tracking/{0}"},
                {"DHL","https://www.dhl.com/en/express/tracking.html?AWB={0}&brand=DHL"},
                {"UPS","https://www.ups.com/track?loc=en_US&tracknum={0}&requester=ST/"},
                {"USPS","https://tools.usps.com/go/TrackConfirmAction?tRef=fullpage&tLabels={0}"},
                {"Canada Post","https://www.trackingmore.com/canada-post-tracking.html?number={0}"},
                {"FedEx","https://www.fedex.com/apps/fedextrack/index.html?tracknumbers={0}&cntry_code=us" },
                {"Lasership","https://lasership.com/track.php?track_number_input={0}"},
                {"TNT","https://www.tnt.com/express/en_us/site/tracking.html?searchType=con&cons={0}"},
                {"Yodel","https://www.trackingmore.com/yodel-tracking.html?number={0}"}
            };

            DataContext = this;

            TrackingSourceName = _trackingOptions.Keys.ToArray();
            Array.Sort(TrackingSourceName);

            PackageOptionComboBox.SelectedIndex = 0;
            PackageOptionComboBox.Foreground = Brushes.White;

            WebBarViewModel = webBarViewModel;

            Amazon.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.amazon.com/");
            DHL.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.dhl.com/us-en/home.html");
            UPS.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.ups.com/us/en/Home.page");
            USPS.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.usps.com/");
            CanadaPost.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.canadapost.ca/cpc/en/home.page");
            FedEx.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.fedex.com/en-us/home.html");
            Lasership.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://lasership.com/");
            TNT.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.tnt.com/express/en_us/site/home.html");
            Yodel.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.parcelmonitor.com/track-yodel-international/");
        }

        private void Track_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TrackingNumberValue.Text is object
                && !TrackingNumberValue.Equals(string.Empty))
            {
                string trackerUrl = PackageOptionComboBox.SelectedItem.ToString();
                WebBarViewModel.OpenDirectUrlBrowser(string.Format(_trackingOptions[trackerUrl], TrackingNumberValue.Text));
            }
        }


        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "package_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(Amazon.Imange, string.Format(imageNamespace, "amazon"));
            imageSourceBuilder.SetImageSource(DHL.Imange, string.Format(imageNamespace, "dhl"));
            imageSourceBuilder.SetImageSource(UPS.Imange, string.Format(imageNamespace, "ups"));
            imageSourceBuilder.SetImageSource(USPS.Imange, string.Format(imageNamespace, "usps"));
            imageSourceBuilder.SetImageSource(CanadaPost.Imange, string.Format(imageNamespace, "canadapost"));
            imageSourceBuilder.SetImageSource(FedEx.Imange, string.Format(imageNamespace, "FedEx"));
            imageSourceBuilder.SetImageSource(Lasership.Imange, string.Format(imageNamespace, "lasership"));
            imageSourceBuilder.SetImageSource(TNT.Imange, string.Format(imageNamespace, "tnt"));
            imageSourceBuilder.SetImageSource(Yodel.Imange, string.Format(imageNamespace, "yodel"));
        }
    }
}
