using Common.ChromiumSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers.Image.Assets.AWS
{
    public class SVGAssetsASWImageProvider : AssetsASWImageProvider
    {
        public SVGAssetsASWImageProvider(IChromiumSettingsService settingsService) : base(settingsService)
        {
            fileExtension = "svg";
        }
    }
}
