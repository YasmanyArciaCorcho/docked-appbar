using Common.DataStructures.SplayTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Common.GarbageCollector
{
    public class SplayCollection<TKey, TValue> : ICollectionGarbageCollector<TKey, TValue> where TKey : IComparable<TKey>
    {
        readonly SplayTree<TKey, TValue> _collection;
        static readonly TimeSpan _freeUpResourcesTime = new TimeSpan(0, 30, 0);

        private readonly DispatcherTimer _collectionIterval;

        public SplayCollection()
            : this(5)
        {

        }

        public SplayCollection(int capacity)
        {
            _collection = new SplayTree<TKey, TValue>();

            Capacity = capacity;

            this._collectionIterval = new DispatcherTimer
            {
                Interval = _freeUpResourcesTime
            };
            this._collectionIterval.Tick += ((sender, args) =>
            {
                Thread thre = new Thread(new ThreadStart(() => { CollectGarbage(); }));
                thre.SetApartmentState(ApartmentState.STA);
                thre.IsBackground = true;
                thre.Start();
            });

            _collectionIterval.Start();
        }

        public TValue this[TKey key]
        {
            get
            {
                return _collection[key];
            }
            set
            {
                _collection[key] = value;
            }
        }

        public ICollection<TKey> Keys => _collection.Keys;

        public ICollection<TValue> Values => _collection.Values;

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        public TimeSpan FreeUpResourcesTime
        {
            get
            {
                return _freeUpResourcesTime;
            }
        }

        public int Capacity
        { get; private set; }

        public void Add(TKey key, TValue value)
        {
            _collection.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _collection.Add(item);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public void CollectGarbage()
        {
            _collection.CollectGarbage(FreeUpResourcesTime,Capacity);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _collection.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _collection.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _collection.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _collection.Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _collection.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
