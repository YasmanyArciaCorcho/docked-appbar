using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers.TopSiteStore
{
    public interface ITopSiteStore
    {
        IEnumerable<TopSiteModel> GetTopSites();
    }
}
