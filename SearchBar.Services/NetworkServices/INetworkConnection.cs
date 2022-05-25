namespace Services.NetworkServices
{
    public interface INetworkConnection
    {
        bool Active { get; }
        string Name { get; }

        NetworkTypes NetworkType { get; }
            
        NetworkStatus Status { get; }
    }
}