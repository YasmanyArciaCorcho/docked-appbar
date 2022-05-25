using MaterialDesignThemes.Wpf;
using SearchBar.UI.Controls.Shortcut;
using Service.RecycleBin;
using Service.WindowsTasksView;
using Services.AudioServices;
using Services.ComputerManagement;
using Services.NetworkServices;
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

namespace SearchBar.UI.Controls.Base.Accordion
{
    /// <summary>
    /// Interaction logic for AccordionControl.xaml
    /// </summary>
    public partial class AccordionControl : UserControl
    {
        ActionTrayShortcut _trayShortcut;
        OfficeShortcut _officeShortcut;

        public AccordionControl()
        {
            InitializeComponent();
        }

        private void TryToControlCenterApplications()
        {
            _trayShortcut = new ActionTrayShortcut(
                new UserInformationService(),
                new ControlUIServices(),
                new NetworkServices(),
                new ComputerAudioServices(),
                new ScreenLightServices());

            _trayShortcut.VerticalAlignment = VerticalAlignment.Top;
            _trayShortcut.HorizontalAlignment = HorizontalAlignment.Center;
            ControlScrollViewer.Content = _trayShortcut;
        }

        private void TryToAddOfficeApplications()
        {
            _officeShortcut = new OfficeShortcut();
            _officeShortcut.VerticalAlignment = VerticalAlignment.Top;
            _officeShortcut.HorizontalAlignment = HorizontalAlignment.Right;
            Panel2.Children.Insert(1, _officeShortcut);
        }


        private void Label_ControlCenter_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_trayShortcut == null)
            {
                TryToControlCenterApplications();
            }
        }

        private void Label_Office_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_officeShortcut == null)
            {
                TryToAddOfficeApplications();
            }
        }
        
        private void Label_Recycle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new RecycleBinService().OpenRecycleBin();
        }

        private void Label_ManageDesktops_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new WindowsTaskViewer().ShowWindowsTaskView();
        }
    }
}
