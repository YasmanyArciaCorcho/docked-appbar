using System;
using System.ComponentModel;
using System.Linq;
using Services.AudioServices;
using Services.ComputerManagement;
using Services.NetworkServices;

namespace SearchBar.UI.Controls.Shortcut
{
    public class ActionTrayModel: INotifyPropertyChanged, IDisposable
    {
        private readonly IAudioServices _audioServices;
        private readonly IScreenLightService _lightServices;
        private readonly INetworkConnectionsProvider _networkConnectionsProvider;
        private double _volumeSetting;
        private double _lightSetting;
        
        private string _activeWifi = "Bluetooth Settings";
        private string _activeBlueTooth = "Wifi Settings";
        
        public ActionTrayModel(IActiveUserInformation activeUserInformation, IAudioServices audioServices, IScreenLightService lightServices, 
            INetworkConnectionsProvider networkConnectionsProvider): this()
        {
            UserName = activeUserInformation.UsersDisplayName();
            _audioServices = audioServices;
            _lightServices = lightServices;
            _volumeSetting = _audioServices.GetAudioLevel();
            _lightSetting = _lightServices.GetBrightness();
            //TODO: add code to actually list wifi profiles in the service, and allow changing in the ui.. 
            //var fullNetworkList = networkConnectionsProvider.GetNetworkConnections(); //.GetNetworkStatus().ToArray();
            //var networkConnections = fullNetworkList as INetworkConnection[] ?? fullNetworkList.ToArray();
            ActiveWifi = "Wifi";// networkConnections.FirstOrDefault(y => y.Active && y.NetworkType == NetworkTypes.Wireless)?.Name ?? string.Empty;
            ActiveBlueTooth = "Bluetooth";// networkConnections.FirstOrDefault(y => y.Active && y.NetworkType == NetworkTypes.Bluetooth)?.Name ?? "Bluetooth";
            // var uiNetworkList = networkConnections.Where(y=> !y.Active).ToArray();
            _networkConnectionsProvider = networkConnectionsProvider;
        }

        public ActionTrayModel() { }

        public bool CanPause { get; set; } = false;
        
        public bool CanPlay { get; set; } = false;
        
        public bool SelectedAudio { get; set; } = false;

        public string UserName { get; private set; } = "User Settings";

        public string ActiveWifi { 
            get => _activeWifi;
            set
            {
                _activeWifi = value;
                RaisePropertyChanged("ActiveWifi");
            } 
        }
        public string ActiveBlueTooth { 
            get => _activeBlueTooth;
            set
            {
                _activeBlueTooth = value;
                RaisePropertyChanged("ActiveBlueTooth");
            } 
        }
        public double LightSetting { 
            get => _lightSetting;
            set
            {
                _lightSetting = value;
                var light = Convert.ToInt32(value);
                _lightServices.SetBrightness(light);
                RaisePropertyChanged("LightSetting");
            } 
        }
        
        public double VolumeSetting { 
            get => _volumeSetting;
            set
            {
                _volumeSetting = value;
                _audioServices.ChangeAudioLevel(value);
                RaisePropertyChanged("VolumeSetting");
            } 
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        /*
        public void ChangeActiveConnection(INetworkConnection connection)
        {
            switch (connection.NetworkType)
            {
                case NetworkTypes.Bluetooth:
                    ActiveBlueTooth = connection.Name;
                    break;
                case NetworkTypes.Wireless:
                    ActiveWifi = connection.Name;
                    break;
            }
        }
        */

        public void Dispose()
        {
            // _audioServices.Dispose();
        }
    }
}