using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers.Image
{
    public interface IImageProvider
    {
        string GetImagePath(string fullImageNamespace);

        Task AddImageModuleSource(string moduleNamespace);
    }
}
