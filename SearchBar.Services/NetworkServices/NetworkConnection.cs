namespace Services.NetworkServices
{
    internal class NetworkConnection: INetworkConnection
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public NetworkTypes NetworkType { get; set; }
        public NetworkStatus Status { get; set; }
    }
}