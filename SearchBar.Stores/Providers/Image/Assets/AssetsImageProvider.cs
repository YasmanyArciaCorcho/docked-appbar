using Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stores.Providers.Image.Assets
{
    public abstract class AssetsImageProvider : IImageProvider
    {
        static string _rootFolder = "Assets";
        protected string fileExtension;

        public AssetsImageProvider()
        {
            _rootFolder = $"{DirectoryInfoHelper.GetCurrentAppRunningLocation()}\\{_rootFolder}";
        }

        /// <summary>
        /// Return the location of the image inside the 'Assert' folder.
        /// </summary>
        /// <param name="imageNamespace"></param>
        /// <returns></returns>
        public virtual string GetImagePath(string imageNamespace)
        {
            return $"{_rootFolder}\\{imageNamespace.Replace('_', '\\')}.{fileExtension}";
        }

        public abstract Task AddImageModuleSource(string moduleNamespace);

        protected string GetModulePath(string moduleNamespace)
        {
            return $"{_rootFolder}\\{moduleNamespace.Replace('_', '\\')}";
        }
    }
}
