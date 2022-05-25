using Common.Shortcut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ShortcutPackage
{
    public class BasicCommonVerticalLinksService : ICommonVerticalLinksService
    {
        public IEnumerable<IShortcut> GetCommonShortcuts()
        {
            List<IShortcut> commonLinks = new List<IShortcut>
            {
                new BaseShortcut("Youtube", "https://www.youtube.com/", ""),
                new BaseShortcut("Amazon", "https://www.amazon.com/", ""),
                new BaseShortcut("Wikipedia", "https://en.wikipedia.org/", ""),
                new BaseShortcut("Twitter", "https://twitter.com/", ""),
                new BaseShortcut("Facebook", "https://www.facebook.com/", ""),
                new BaseShortcut("Instagram", "https://www.instagram.com/", "")
            };

            return commonLinks;
        }
    }
}
