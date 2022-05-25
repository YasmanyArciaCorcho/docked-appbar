using Services.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchBar.UI.Builders.Image
{
    public interface IImageSourceBuilder
    {
        void SetImageSource(System.Windows.Controls.Image image, string imageNamespace);

        Task AddModuleNamespace(string moduleNamespace);
    }
}
