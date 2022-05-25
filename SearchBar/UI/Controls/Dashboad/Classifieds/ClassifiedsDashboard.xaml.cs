using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Handles.Classifields;
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

namespace SearchBar.UI.Controls.Dashboad.Classifieds
{
    /// <summary>
    /// Interaction logic for Classifieds.xaml
    /// </summary>
    public partial class ClassifiedsDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Classifieds";
        public static string ImagePath = "classifieds_logo";

        public WebBarViewModel WebBarViewModel { get; set; }

        public SortedList<string, string> States { get; set; }
        public SortedList<string, string> Cities { get; set; }
        public SortedList<string, string> SearchCategory { get; set; }

        Border _previousSelected;
        static Brush _selectedColorBrush;
        static bool _comboBoxOptions;

        string _currentSearchUrl = "";
        readonly string _craigslistSearchUrl = "https://{0}.craigslist.org/search/{1}?";
        readonly string _ebaySearchUrl = "https://www.ebay.com/sch/i.html?_nkw={0}";
        readonly string _letgoSearchUrl = "https://www.letgo.com/en-us?searchTerm={0}";
        readonly IClassifieldHandler<ClassifiedsDashboard> _classifieldHandler;

        public ClassifiedsDashboard(WebBarViewModel webBarViewModel, IClassifieldHandler<ClassifiedsDashboard> classifieldHandler, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            _selectedColorBrush = new SolidColorBrush(Color.FromRgb(170, 167, 162));

            WebBarViewModel = webBarViewModel;

            Locanto.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.locanto.com/");
            ClassifiedadsAds.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.classifiedads.com/");
            Oodle.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.oodle.com/");

            FaceMarket.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.facebook.com/marketplace");
            Mercari.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.mercari.com/");
            Etsy.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.etsy.com/");

            Eventful.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://losangeles.eventful.com/events");
            Meetup.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.meetup.com/");
            EventBrite.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.eventbrite.com/");

            Zillow.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.zillow.com/");
            Trulia.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.trulia.com/");
            Realton.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => webBarViewModel.OpenDirectUrlBrowser("https://www.realtor.com/");

            _classifieldHandler = classifieldHandler;

            _classifieldHandler.Dashboard = this;
            DataContext = this;

            _classifieldHandler.UpdateStateList();
            _classifieldHandler.UpdateSearchCategory();

            StateComboBox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                _classifieldHandler.UpdateCitiesList(States[(string)StateComboBox.SelectedItem]);
            };

            Craigslist.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                UpdateSearchEngine(Craigslist, _craigslistSearchUrl, true);
            };

            Ebay.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                UpdateSearchEngine(Ebay, _ebaySearchUrl, false);
            };

            Letgo.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                UpdateSearchEngine(Letgo, _letgoSearchUrl, false);
            };

            _previousSelected = Craigslist;
            UpdateSearchEngine(Craigslist, _craigslistSearchUrl, true);
        }

        private void UpdateSearchEngine(Border sender, string searchUrl, bool comboBoxOption)
        {
            _previousSelected.Background = sender.Background;
            _previousSelected = sender;
            sender.Background = _selectedColorBrush;

            _currentSearchUrl = searchUrl;

            if (comboBoxOption != _comboBoxOptions)
            {
                if (comboBoxOption)
                {
                    StatePanel.Visibility = Visibility.Visible;
                    LocationPanel.Visibility = Visibility.Visible;
                    SearchCategoryComboBox.Visibility = Visibility.Visible;
                    SearchTextbox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    StatePanel.Visibility = Visibility.Collapsed;
                    LocationPanel.Visibility = Visibility.Collapsed;
                    SearchCategoryComboBox.Visibility = Visibility.Collapsed;
                    SearchTextbox.Visibility = Visibility.Visible;
                }

                _comboBoxOptions = comboBoxOption;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TriggerSearch();
        }

        private void SearchTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TriggerSearch();
        }

        private void TriggerSearch()
        {
            if (_comboBoxOptions)
            {
                string category = (string)SearchCategoryComboBox.SelectedItem;
                WebBarViewModel.OpenDirectUrlBrowser(string.Format(_craigslistSearchUrl,
                     Cities[(string)LocationComboBox.SelectedItem], SearchCategory[category]));
            }
            else
            {
                WebBarViewModel.OpenDirectUrlBrowser(string.Format(_currentSearchUrl, SearchTextbox.Text));
            }
            SearchTextbox.Text = string.Empty;
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "classifieds_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(craigslist, string.Format(imageNamespace, "craigslist"));
            imageSourceBuilder.SetImageSource(ebay, string.Format(imageNamespace, "ebay"));
            imageSourceBuilder.SetImageSource(letgo, string.Format(imageNamespace, "letgo"));
            imageSourceBuilder.SetImageSource(Locanto.Image, string.Format(imageNamespace, "locanto"));
            imageSourceBuilder.SetImageSource(ClassifiedadsAds.Image, string.Format(imageNamespace, "classifiedsad"));
            imageSourceBuilder.SetImageSource(Oodle.Image, string.Format(imageNamespace, "oodle"));
            imageSourceBuilder.SetImageSource(FaceMarket.Image, string.Format(imageNamespace, "facemarket"));
            imageSourceBuilder.SetImageSource(Mercari.Image, string.Format(imageNamespace, "mercari"));
            imageSourceBuilder.SetImageSource(Etsy.Image, string.Format(imageNamespace, "etsy"));
            imageSourceBuilder.SetImageSource(Eventful.Image, string.Format(imageNamespace, "eventful"));
            imageSourceBuilder.SetImageSource(Meetup.Image, string.Format(imageNamespace, "meetup"));
            imageSourceBuilder.SetImageSource(EventBrite.Image, string.Format(imageNamespace, "eventbrite"));
            imageSourceBuilder.SetImageSource(Zillow.Image, string.Format(imageNamespace, "zillow"));
            imageSourceBuilder.SetImageSource(Trulia.Image, string.Format(imageNamespace, "trulia"));
            imageSourceBuilder.SetImageSource(Realton.Image, string.Format(imageNamespace, "realtor"));
        }
    }
}
