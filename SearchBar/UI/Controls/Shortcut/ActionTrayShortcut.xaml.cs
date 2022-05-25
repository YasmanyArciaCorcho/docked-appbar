using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Services.AudioServices;
using Services.ComputerManagement;
using Services.NetworkServices;

namespace SearchBar.UI.Controls.Shortcut
{
    public partial class ActionTrayShortcut
    {
        private readonly ActionTrayModel _model;
        private readonly ISystemControlUiOpener _controlUiOpener;

        public ActionTrayShortcut(IActiveUserInformation userInformation, ISystemControlUiOpener controlUiOpener, INetworkConnectionsProvider networkConnectionsProvider,
            IAudioServices audioServices, IScreenLightService lightServices)
        {
            _controlUiOpener = controlUiOpener;

            Thread thre = new Thread(new ThreadStart(() =>
            {
                var _model = new ActionTrayModel(userInformation, audioServices, lightServices, networkConnectionsProvider);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
               {
                   DataContext = _model;
               },
                   DispatcherPriority.Background);
                Dispatcher.Run();
            }));
            thre.SetApartmentState(ApartmentState.MTA);
            thre.IsBackground = true;
            thre.Start();

            InitializeComponent();
        }

        private void SettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _controlUiOpener.OpenSettings();
        }
        private void SettingsBtn_OnMouseDown(object sender, RoutedEventArgs e)
        {
            _controlUiOpener.OpenSettings();

        }

        private void UserBtn_OnMouseDown(object sender, RoutedEventArgs e)
        {
            _controlUiOpener.OpenUserAccount();
        }

        private void media_play_OnMouseDown(object sender, RoutedEventArgs e)
        {
            _model.CanPlay = false;
            _model.CanPause = true;
            if (_model.SelectedAudio)
            {
                AudioFile.Play();
            }
            else
            {
                SelectAudio();
            }
        }
        private void media_stop_OnMouseDown(object sender, RoutedEventArgs e)
        {
            _model.CanPlay = true;
            _model.CanPause = false;
            if (_model.SelectedAudio)
            {
                AudioFile.Stop();
            }
            else
            {
                SelectAudio();
            }
        }

        private void media_open_OnMouseDown(object sender, RoutedEventArgs e)
        {
            SelectAudio();
        }

        private void SelectAudio()
        {
            var openFileDlg = new Microsoft.Win32.OpenFileDialog();
            var result = openFileDlg.ShowDialog();
            if (result != true) return;
            var fileSelected = openFileDlg.FileName;
            AudioFile.Source = new Uri(fileSelected);
            AudioFile.Play();
            _model.CanPlay = false;
            _model.CanPause = true;
            _model.SelectedAudio = true;
        }

        private void UserBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _controlUiOpener.OpenUserAccount();
        }

        private void ActiveBlueTooth_OnClick(object sender, RoutedEventArgs e)
        {
            _controlUiOpener.OpenBlueToothSettings();
        }

        private void WifiBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _controlUiOpener.OpenWifiSettings();
        }
    }
}