using SearchBar.UI.Base;
using SearchBar.UI.Builders.Image;
using SearchBar.UI.Controls.Base;
using SearchBar.UI.Controls.Dashboad.News.Categories;
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

namespace SearchBar.UI.Controls.Dashboad.News
{
    /// <summary>
    /// Interaction logic for NewsDashboard.xaml
    /// </summary>
    public partial class NewsDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "News";
        public static string ImagePath = "news_logo";

        public WebBarViewModel WebBarViewModel
        { get; set; }
        readonly GridTabManager gridTabManager;

        IImageSourceBuilder _imageSourceBuilder;

        EntertainmentDashboard _entertainmentDashboard;
        BusinessDashboard _businessDashboard;
        PoliticsDashboard _politicsDashboard;
        SportsDashboard _sportsDashboard;

        public NewsDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();

            WebBarViewModel = webBarViewModel;
            _imageSourceBuilder = imageSourceBuilder;

            Entertainment.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            Business.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            Politics.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;
            Sports.MouseLeftButtonDown += MenuTab_MouseLeftButtonDown;

            gridTabManager = new GridTabManager();

            _entertainmentDashboard = new EntertainmentDashboard(WebBarViewModel, imageSourceBuilder);
            gridTabManager.SelectDefaultCategory(RootNewsGrid, _entertainmentDashboard, EntertainmentPanel);
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
                    if (_entertainmentDashboard == null)
                        _entertainmentDashboard = new EntertainmentDashboard(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootNewsGrid, _entertainmentDashboard, EntertainmentPanel);
                    break;
                case 1:
                    if (_businessDashboard == null)
                        _businessDashboard = new BusinessDashboard(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootNewsGrid, _businessDashboard, BusinessPanel);
                    break;
                case 2:
                    if (_politicsDashboard == null)
                        _politicsDashboard = new PoliticsDashboard(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootNewsGrid, _politicsDashboard, PoliticsPanel);
                    break;
                default:
                    if (_sportsDashboard == null)
                        _sportsDashboard = new SportsDashboard(WebBarViewModel, _imageSourceBuilder);
                    gridTabManager.UpdatePanelCategory(RootNewsGrid, _sportsDashboard, Sportspanel);
                    break;
            }
        }
    }
}
