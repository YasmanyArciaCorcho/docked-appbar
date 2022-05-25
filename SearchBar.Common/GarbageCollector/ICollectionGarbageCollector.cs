using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.GarbageCollector
{
    public interface ICollectionGarbageCollector<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        TimeSpan FreeUpResourcesTime { get; }

        int Capacity { get; }

        void CollectGarbage();
    }
}
