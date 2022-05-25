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

namespace SearchBar.UI.Controls.Dashboad.Dictionary
{
    /// <summary>
    /// Interaction logic for DictionaryDashboard.xaml
    /// </summary>
    public partial class DictionaryDashboard : UserControl
    {
        public static string DashboardName = "Dictionary";
        public static string ImagePath = "dictionary_logo";

        const string _searchBarText = "Search {0}";
        readonly Dictionary<string, string> _engineOptions;

        Border _previousSelected;
        static Brush _selectedColorBrush;

        public DictionaryDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            _selectedColorBrush = new SolidColorBrush(Color.FromRgb(77, 74, 69));

            _engineOptions = new Dictionary<string, string>()
            {
                {"Wikipedia", "https://en.wikipedia.org/wiki/{0}"},
                {"Dictionary", "https://www.dictionary.com/browse/{0}"},
                {"Thesaurus", "https://www.thesaurus.com/misspelling?term={0}"}
            };

            Wikipedia.PreviewMouseLeftButtonDown += DictionaryEngine_Click;
            Dictionary.PreviewMouseLeftButtonDown += DictionaryEngine_Click;
            Thesaurus.PreviewMouseLeftButtonDown += DictionaryEngine_Click;

            GoogleTranslate.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://translate.google.com/"); };
            ManualLib.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { webBarViewModel.OpenDirectUrlBrowser("https://www.manualslib.com/"); };

            _previousSelected = Wikipedia;
            UpdateSearchEngine(nameof(Wikipedia), Wikipedia);
        }

        private void DictionaryEngine_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Border border)
                UpdateSearchEngine(border.Uid, border);
        }

        private void UpdateSearchEngine(string engineName, Border newSeleced)
        {
            _previousSelected.Background = newSeleced.Background;
            _previousSelected = newSeleced;
            _previousSelected.Background = _selectedColorBrush;

            SearchBar.QueryToComplete = _engineOptions[engineName];
            SearchBar.PlaceHolderTextBlock.Text = string.Format(_searchBarText, engineName);
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "dictionary_{0}";
            imageSourceBuilder.SetImageSource(logo, string.Format(imageNamespace, "logo"));
            imageSourceBuilder.SetImageSource(powered_wikipedia, string.Format(imageNamespace, "powered_wikipedia"));
            imageSourceBuilder.SetImageSource(powered_dictionary, string.Format(imageNamespace, "powered_dictionary"));
            imageSourceBuilder.SetImageSource(powered_thesaurus, string.Format(imageNamespace, "powered_thesaurus"));
            imageSourceBuilder.SetImageSource(GoogleTranslate.Image, string.Format(imageNamespace, "googletranslate"));
            imageSourceBuilder.SetImageSource(ManualLib.Image, string.Format(imageNamespace, "manualslib"));
        }
    }
}
