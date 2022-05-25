using Store.Models;
using System.Collections.Generic;

namespace Stores.Providers.FaviconStore
{
    public interface IFaviconStore
    {
        IEnumerable<FaviconModel> GetFavIcons(IEnumerable<string> pagesUrls);

        FaviconModel GetFavicon(string pageUrl);

        void AddFavicon(string pageUrl, string faviconUrl, byte[] content);

        void AddFavoiconToPageUrl(string pageUrl, int favoiconId);

        int ExistsFavIcon(string favoiconUrl);
    }
}
