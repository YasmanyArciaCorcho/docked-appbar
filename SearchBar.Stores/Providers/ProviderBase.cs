using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stores.Providers
{
    public abstract class ProviderBase : IProvider
    {
        protected readonly object _saveLock = new object();

        public ProviderBase()
        {
            Task.Run(async () => await Load()).Wait();
        }

        public abstract Task<bool> Load();

        public abstract Task<bool> Save();
    }
}
