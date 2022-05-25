using SearchBar.UI.Base;
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

namespace SearchBar.UI.Controls.Dashboad.Form
{
    /// <summary>
    /// Interaction logic for FormsDashboard.xaml
    /// </summary>
    public partial class FormsDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Forms";
        public static string ImagePath = "forms_logo";

        public WebBarViewModel WebBarViewModel
        { get; set; }

        public FormsDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            WebBarViewModel = webBarViewModel;

            form1040.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.irs.gov/pub/irs-pdf/f1040.pdf"); };
            formw4.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.irs.gov/pub/irs-pdf/fw4.pdf"); };
            formw9.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.irs.gov/pub/irs-pdf/fw9.pdf"); };

            DMV.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://dmv.dc.gov/page/dc-dmv-forms#main-content"); };
            USAGov.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.usa.gov/"); };
            Ssa.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.ssa.gov/"); };
            Medicare.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.medicare.gov/"); };
            Passport.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://travel.state.gov/content/travel/en/passports.html"); };
            Immigration.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.uscis.gov/"); };
            WhiteHouse.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.whitehouse.gov/"); };

            TaxEstimator.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.irs.gov/individuals/tax-withholding-estimator"); };
            TaxBrackets.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.bankrate.com/finance/taxes/tax-brackets.aspx"); };
            TaxStatistics.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://www.irs.gov/statistics"); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebBarViewModel.OpenDirectUrlBrowser($"https://www.irs.gov/site-index-search?search={formKeyword.Text}");
            formKeyword.Text = "";
        }

        private void FormKeyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, null);
            }
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "forms_{0}";
            imageSourceBuilder.SetImageSource(irsform, string.Format(imageNamespace, "irsform"));
            imageSourceBuilder.SetImageSource(form1040.Image, string.Format(imageNamespace, "1040"));
            imageSourceBuilder.SetImageSource(formw4.Image, string.Format(imageNamespace, "w4"));
            imageSourceBuilder.SetImageSource(formw9.Image, string.Format(imageNamespace, "w9"));
            imageSourceBuilder.SetImageSource(DMV.Image, string.Format(imageNamespace, "dmv"));
            imageSourceBuilder.SetImageSource(USAGov.Image, string.Format(imageNamespace, "usagovi"));
            imageSourceBuilder.SetImageSource(Ssa.Image, string.Format(imageNamespace, "socialsecurity"));
            imageSourceBuilder.SetImageSource(Medicare.Image, string.Format(imageNamespace, "medicare"));
            imageSourceBuilder.SetImageSource(Passport.Image, string.Format(imageNamespace, "passport"));
            imageSourceBuilder.SetImageSource(Immigration.Image, string.Format(imageNamespace, "immigration"));
            imageSourceBuilder.SetImageSource(WhiteHouse.Image, string.Format(imageNamespace, "whitehouse"));
            imageSourceBuilder.SetImageSource(TaxEstimator.Image, string.Format(imageNamespace, "estimator"));
            imageSourceBuilder.SetImageSource(TaxBrackets.Image, string.Format(imageNamespace, "bankbrackets"));
            imageSourceBuilder.SetImageSource(TaxStatistics.Image, string.Format(imageNamespace, "taxstatistics"));
        }
    }
}
