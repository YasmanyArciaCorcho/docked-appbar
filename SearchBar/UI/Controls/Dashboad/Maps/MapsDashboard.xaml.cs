using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Dashboad.Maps.Categories;
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

namespace SearchBar.UI.Controls.Dashboad.Maps
{


    /// <summary>
    /// Interaction logic for MapsWidget.xaml
    /// </summary>
    public partial class MapsDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Maps";
        public static string ImagePath = "maps_logo";

        int Conveyance;
        readonly GridTabManager gridTabManager;

        static readonly List<string> _travelModel = new List<string>()
        {
           "!4m2!4m1!3e0",
           "!4m2!4m1!3e2",
           "!4m2!4m1!3e3"
        };

        static readonly SolidColorBrush blueColor = new SolidColorBrush(Color.FromRgb(0, 120, 215));

        DirectionsMaps _directionsMaps;
        TravelMaps _travelMaps;
        RoadMaps _roadMaps;
        EarthMaps _earthMaps;
        readonly IImageSourceBuilder _imageSourceBuilder;

        public WebBarViewModel WebBarViewModel
        { get; set; }

        public MapsDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            _imageSourceBuilder = imageSourceBuilder;

            InitializeComponent();

            _imageSourceBuilder.SetImageSource(logo, ImagePath);

            Conveyance = 0;
            CarIcon.Foreground = blueColor;

            WebBarViewModel = webBarViewModel;
            gridTabManager = new GridTabManager();

            DirectionsPanel.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            TravelPanel.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            RoadPanel.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            EartPanel.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;

            if (_travelMaps == null)
                _travelMaps = new TravelMaps(WebBarViewModel, _imageSourceBuilder);

            gridTabManager.SelectDefaultCategory(RootMapsGrid, _travelMaps, TravelPanel);
        }

        private void PackIcon_MouseLeftButtonDown_car(object sender, MouseButtonEventArgs e)
        {
            ResetCoveyance();
            Conveyance = 0;
            CarIcon.Foreground = blueColor;
        }

        private void PackIcon_MouseLeftButtonDown_walk(object sender, MouseButtonEventArgs e)
        {
            ResetCoveyance();
            Conveyance = 1;
            WalkIcon.Foreground = blueColor;
        }

        private void PackIcon_MouseLeftButtonDown_airplane(object sender, MouseButtonEventArgs e)
        {
            ResetCoveyance();
            Conveyance = 2;
            BusIcon.Foreground = blueColor;
        }

        private void ResetCoveyance()
        {
            CarIcon.Foreground = Brushes.White;
            WalkIcon.Foreground = Brushes.White;
            BusIcon.Foreground = Brushes.White;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url;
            if (string.IsNullOrEmpty(StartAddress.Text))
            {
                url = @$"https://www.google.com/maps/search/?api=1&query={GoAddress.Text}";
            }
            else if (string.IsNullOrEmpty(GoAddress.Text))
            {
                url = @$"https://www.google.com/maps/dir/?api=1&origin={StartAddress.Text}&destination={GoAddress.Text}";

            }
            else
            {
                url = $@"https://www.google.com/maps/dir/{StartAddress.Text}/{GoAddress.Text}/data={_travelModel[Conveyance]}";
            }

            WebBarViewModel.OpenDirectUrlBrowser(url);
            StartAddress.Text = "";
            GoAddress.Text = "";
        }

        private void GoAddress_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(sender, null);
        }

        private void MenuTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is UIElement uIElement)
            {
                if (int.TryParse(uIElement.Uid, out int index))
                {
                    UpdateCategory(index);
                }
            }
        }

        private void UpdateCategory(int index)
        {
            switch (index)
            {
                case 0:
                    if (_directionsMaps == null)
                        _directionsMaps = new DirectionsMaps(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootMapsGrid, _directionsMaps, DirectionsPanel);
                    break;
                case 1:
                    if (_travelMaps == null)
                        _travelMaps = new TravelMaps(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootMapsGrid, _travelMaps, TravelPanel);
                    break;
                case 2:
                    if (_roadMaps == null)
                        _roadMaps = new RoadMaps(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootMapsGrid, _roadMaps, RoadPanel);
                    break;
                default:
                    if (_earthMaps == null)
                        _earthMaps = new EarthMaps(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootMapsGrid, _earthMaps, EartPanel);
                    break;
            }
        }
    }
}
