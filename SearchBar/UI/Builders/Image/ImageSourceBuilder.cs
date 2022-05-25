using Stores.Providers.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SearchBar.UI.Builders.Image
{
    public abstract class ImageSourceBuilder : IImageSourceBuilder
    {
        protected IImageProvider _imageProvider;

        public ImageSourceBuilder(IImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }

        public async Task AddModuleNamespace(string moduleNamesace)
        {
            await _imageProvider.AddImageModuleSource(moduleNamesace.ToLower());
        }

        public abstract void SetImageSource(System.Windows.Controls.Image image, string imageNamespace);
    }
}
