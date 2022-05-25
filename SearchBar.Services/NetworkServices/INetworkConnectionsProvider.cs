using System.Collections.Generic;

namespace Services.NetworkServices
{
    public interface INetworkConnectionsProvider
    {
        IEnumerable<INetworkConnection> GetNetworkConnections();
    }
}