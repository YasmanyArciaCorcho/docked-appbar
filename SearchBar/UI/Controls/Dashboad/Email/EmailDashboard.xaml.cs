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

namespace SearchBar.UI.Controls.Dashboad.Email
{
    /// <summary>
    /// Interaction logic for EmailDashboard.xaml
    /// </summary>
    public partial class EmailDashboard : UserControl, IDashboard
    {
        public static string DashboardName = "Email";
        public static string ImagePath = "email_logo";

        public WebBarViewModel WebBarViewModel
        { get; set; }

        public EmailDashboard(WebBarViewModel webBarViewModel, IImageSourceBuilder imageSourceBuilder)
        {
            InitializeComponent();
            InitializeImages(imageSourceBuilder);

            WebBarViewModel = webBarViewModel;

            Yahoo.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://mail.yahoo.com"); };
            Gmail.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://mail.google.com/mail/u/0/#inbox"); };
            Outlook.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => { WebBarViewModel.OpenDirectUrlBrowser("https://mail.live.com/"); };
        }

        private void InitializeImages(IImageSourceBuilder imageSourceBuilder)
        {
            string imageNamespace = "email_{0}";
            imageSourceBuilder.SetImageSource(yahoo, string.Format(imageNamespace, "yahoo"));
            imageSourceBuilder.SetImageSource(outlook, string.Format(imageNamespace, "outlook"));
            imageSourceBuilder.SetImageSource(gmail, string.Format(imageNamespace, "gmail"));
        }
    }
}
