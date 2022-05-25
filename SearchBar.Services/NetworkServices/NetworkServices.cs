using System.Collections.Generic;
using System.Linq;
using InTheHand.Net.Bluetooth;

namespace Services.NetworkServices
{
    public class NetworkServices: INetworkConnectionsProvider
    {
        private static NetworkStatus MapModeToStatus(RadioMode givenRadio)
        {
            switch (givenRadio)
            {
                case RadioMode.Discoverable:
                    return NetworkStatus.Connected;
                case RadioMode.PowerOff:
                    return NetworkStatus.Disabled;
                case RadioMode.Connectable:
                default:
                    return NetworkStatus.Disconnected;
            }
        }
        private static RadioMode MapModeToStatus(NetworkStatus givenRadio)
        {
            return givenRadio switch
            {
                NetworkStatus.Connected => RadioMode.Discoverable,
                NetworkStatus.Disabled => RadioMode.PowerOff,
                NetworkStatus.Disconnected => RadioMode.Discoverable,
                _ => RadioMode.Connectable,
            };
        }

        private static void SetBlueToothNetwork(INetworkConnection connection)
        {
            var newStatus = MapModeToStatus(connection.Status);
            BluetoothRadio.PrimaryRadio.Mode = newStatus;//TODO: when we upgrade to core this changes to BluetoothRadio.Default;
        }

        private static NetworkConnection GetBlueToothNetworkFeature(string name, RadioMode modeForRadio)
        {
            return new NetworkConnection
            {
                Active = false,
                Name = $"{name}-{modeForRadio}",
                Status = MapModeToStatus(modeForRadio),
                NetworkType = NetworkTypes.Bluetooth
            };
        }
        
        private static IEnumerable<INetworkConnection> GetBlueTooth()
        {
            var blueTooth = new List<INetworkConnection>();
            try
            {

                var defaultRadio = BluetoothRadio.PrimaryRadio;//TODO: when we upgrade to core this changes to BluetoothRadio.Default;
                var activeConnect = GetBlueToothNetworkFeature(defaultRadio.Name, defaultRadio.Mode);
                activeConnect.Active = true;
                blueTooth.Add(activeConnect);
                if (defaultRadio.Mode != RadioMode.PowerOff)
                {
                    blueTooth.Add(GetBlueToothNetworkFeature(defaultRadio.Name, RadioMode.PowerOff));
                }
                if (defaultRadio.Mode != RadioMode.Discoverable)
                {
                    blueTooth.Add(GetBlueToothNetworkFeature(defaultRadio.Name, RadioMode.Discoverable));
                }
                if (defaultRadio.Mode != RadioMode.Connectable)
                {
                    blueTooth.Add(GetBlueToothNetworkFeature(defaultRadio.Name, RadioMode.Connectable));
                }
            }
            catch
            {
                //ignored
            }
            return blueTooth;
        }
        private static NetworkStatus MapWifiStatus(Wlan.WlanInterfaceState givenStatus)
        {
            switch (givenStatus)
            {
                case Wlan.WlanInterfaceState.Disconnecting:
                case Wlan.WlanInterfaceState.Disconnected:
                case Wlan.WlanInterfaceState.Discovering:
                    return NetworkStatus.Disconnected;
                case Wlan.WlanInterfaceState.Associating:
                case Wlan.WlanInterfaceState.Connected:
                case Wlan.WlanInterfaceState.Authenticating:
                case Wlan.WlanInterfaceState.AdHocNetworkFormed:
                    return NetworkStatus.Connected;
                case Wlan.WlanInterfaceState.NotReady:
                    return NetworkStatus.Disabled;
                default:
                    return NetworkStatus.Disabled;
            }
        }
        private static IEnumerable<INetworkConnection> GetWifi()
        {
            try
            {
                using (WlanClient wifiClient = new WlanClient())
                {
                    var wifiInterfaces = wifiClient.Interfaces;

                    var activeWifi = wifiInterfaces.Select(wifiInterface =>
                        new NetworkConnection
                        {
                            Name = $"{wifiInterface.InterfaceName}-{wifiInterface.InterfaceState}",
                            Status = MapWifiStatus(wifiInterface.InterfaceState),
                            NetworkType = NetworkTypes.Wireless,
                            Active = true,
                        });

                    return activeWifi;
                }
            }
            catch
            {
                return new INetworkConnection[0];
            }
        }
        public IEnumerable<INetworkConnection> GetNetworkConnections()
        {
            var wifiList = GetWifi().ToList();
            wifiList.AddRange(GetBlueTooth());
            return wifiList;
        }
        public static void SelectNetwork(INetworkConnection connection)
        {
            //var cli = new BluetoothClient();
            if (connection.Active) return;
            switch (connection.NetworkType)
            {
                case NetworkTypes.Bluetooth:
                    SetBlueToothNetwork(connection);
                    break;
                case NetworkTypes.Wireless:
                    //TODO: add code for calling the correct action
                    break;
            }
        }
    }
}