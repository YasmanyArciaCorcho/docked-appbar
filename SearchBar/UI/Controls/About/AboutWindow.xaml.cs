using Common.Settings.Chromium;
using SearchBar.UI.WebBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchBar.UI.Controls.About
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private bool _isClosing;
        private const string versionText = "Version: {0}";
        private const string copyrightText = "Copyright © 2020 {0}. All rights reserved.";

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint SC_CLOSE = 0xF060;
        readonly WebBarViewModel _webBarViewModel;

        public AboutWindow(WebBarViewModel webBarViewModel)
        {
            InitializeComponent();

            _webBarViewModel = webBarViewModel;

            Title += $" {ProducSettings.ProducName}";
            VersionTextblock.Text = string.Format(versionText, webBarViewModel.Settings.ProductVersion);
            CopyrighTextBlock.Text = string.Format(copyrightText, ProducSettings.ProducName);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero)
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
          => CallClose();

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            _webBarViewModel.OpenDirectUrlBrowser(e.Uri.AbsoluteUri);
            CallClose();
        }

        private void CallClose()
        {
            if (!_isClosing)
            {
                _isClosing = true;
                Close();
            }
        }
    }
}
