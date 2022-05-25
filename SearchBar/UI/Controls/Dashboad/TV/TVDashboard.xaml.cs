using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Dashboad.TV.Categories;
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

namespace SearchBar.UI.Controls.Dashboad.TV
{
    /// <summary>
    /// Interaction logic for TVDashboard.xaml
    /// </summary>
    public partial class TVDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Tv";
        public static string ImagePath = "tv_logo";

        public WebBarViewModel WebBarViewModel
        { get; set; }

        readonly GridTabManager gridTabManager;
        TVStreaming _tvStreaming;
        TVSports _tvSports;
        TVNews _tvNews;
        readonly IImageSourceBuilder _imageSourceBuilder;

        public TVDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            _imageSourceBuilder = imageSourceBuilder;

            Streaming.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            Sports.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            News.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;

            WebBarViewModel = webBarViewModel;

            gridTabManager = new GridTabManager();
            _tvStreaming = new TVStreaming(WebBarViewModel, imageSourceBuilder);
            gridTabManager.SelectDefaultCategory(RootTVGrid, _tvStreaming, StreamingPanel);
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
                    if (_tvStreaming == null)
                        _tvStreaming = new TVStreaming(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootTVGrid, _tvStreaming, StreamingPanel);
                    break;
                case 1:
                    if (_tvSports == null)
                        _tvSports = new TVSports(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootTVGrid, _tvSports, SportsPanel);
                    break;
                default:
                    if (_tvNews == null)
                        _tvNews = new TVNews(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootTVGrid, _tvNews, NewsPanel);
                    break;
            }
        }
    }
}
