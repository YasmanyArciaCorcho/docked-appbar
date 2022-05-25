using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers
{
    /// <summary>
    /// Todo: do a better hierarchy around the elements that can be saved on disk.
    /// Faster version to get alive the mvp.
    /// </summary>
    public interface IProvider
    {
        Task<bool> Load();

        Task<bool> Save();
    }
}
