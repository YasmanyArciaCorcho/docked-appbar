using Common.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataStructures.SplayTree
{
    public class SplayTree<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        private SplayTreeNode<TKey, TValue> _root;
        private int _count;
        private int version = 0;

        public void Add(TKey key, TValue value)
        {
            Set(key, value, throwOnExisting: true);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Set(item.Key, item.Value, throwOnExisting: true);
        }

        private void Set(TKey key, TValue value, bool throwOnExisting)
        {
            if (_count == 0)
            {
                version++;
                _root = new SplayTreeNode<TKey, TValue>(key, value);
                _count = 1;
                return;
            }

            Splay(key);

            int c = key.CompareTo(this._root.Key);
            if (c == 0)
            {
                if (throwOnExisting)
                {
                    StaticLogger.Logger.Trace("An item with the same key already exists in the tree.");
                }

                version++;
                _root.Value = value;
                return;
            }

            SplayTreeNode<TKey, TValue> n = new SplayTreeNode<TKey, TValue>(key, value);
            if (c < 0)
            {
                n.LeftChild = _root.LeftChild;
                n.RightChild = _root;
                _root.LeftChild = null;
            }
            else
            {
                n.RightChild = _root.RightChild;
                n.LeftChild = _root;
                _root.RightChild = null;
            }

            _root = n;
            _count++;
            Splay(key);
            version++;
        }

        public void Clear()
        {
            _root = null;
            _count = 0;
            version++;
        }

        public bool ContainsKey(TKey key)
        {
            if (_count == 0)
            {
                return false;
            }

            Splay(key);

            return key.CompareTo(this._root.Key) == 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (_count == 0)
            {
                return false;
            }

            Splay(item.Key);

            return item.Key.CompareTo(this._root.Key) == 0 && (object.ReferenceEquals(this._root.Value, item.Value) || (item.Value is object && item.Value.Equals(this._root.Value)));
        }

        public SplayTreeNode<TKey, TValue> GetRoot() 
        {
            return _root;
        }

        private void Splay(TKey key)
        {
            SplayTreeNode<TKey, TValue> l, r, t, y, header;
            l = r = header = new SplayTreeNode<TKey, TValue>(default, default);
            t = _root;
            while (true)
            {
                var c = key.CompareTo(t.Key);
                if (c < 0)
                {
                    if (t.LeftChild == null) break;
                    if (key.CompareTo(t.LeftChild.Key) < 0)
                    {
                        y = t.LeftChild;
                        t.LeftChild = y.RightChild;
                        y.RightChild = t;
                        t = y;
                        if (t.LeftChild == null) break;
                    }
                    r.LeftChild = t;
                    r = t;
                    t = t.LeftChild;
                }
                else if (c > 0)
                {
                    if (t.RightChild == null) break;
                    if (key.CompareTo(t.RightChild.Key) > 0)
                    {
                        y = t.RightChild;
                        t.RightChild = y.LeftChild;
                        y.LeftChild = t;
                        t = y;
                        if (t.RightChild == null) break;
                    }
                    l.RightChild = t;
                    l = t;
                    t = t.RightChild;
                }
                else
                {
                    break;
                }
            }
            l.RightChild = t.LeftChild;
            r.LeftChild = t.RightChild;
            t.LeftChild = header.RightChild;
            t.RightChild = header.LeftChild;
            _root = t;
            _root.LastSplayTime = DateTime.Now;
        }

        public bool Remove(TKey key)
        {
            if (_count == 0)
            {
                return false;
            }

            Splay(key);

            if (key.CompareTo(_root.Key) != 0)
            {
                return false;
            }

            if (_root.LeftChild == null)
            {
                _root = _root.RightChild;
            }
            else
            {
                var swap = _root.RightChild;
                _root = _root.LeftChild;
                Splay(key);
                _root.RightChild = swap;
            }

            version++;
            _count--;
            return true;
        }

        public void CollectGarbage(TimeSpan freeUpResourcesTime, int capacity) 
        {
            SplayTreeNode<TKey, TValue> currentNode = GetRoot();
            if (currentNode != null)
            {
                Stack<SplayTreeNode<TKey, TValue>> splayTreeNodes = new Stack<SplayTreeNode<TKey, TValue>>();
                splayTreeNodes.Push(currentNode);

                int countedNodes = 1;

                DateTime toCompare = DateTime.Now;

                while (splayTreeNodes.Count != 0)
                {
                    currentNode = splayTreeNodes.Pop();

                    if (countedNodes <= capacity)
                    {
                        if (currentNode.LeftChild != null)
                        {
                            if (toCompare - currentNode.LeftChild.LastSplayTime < freeUpResourcesTime)
                            {
                                splayTreeNodes.Push(currentNode.LeftChild);
                                countedNodes++;
                            }
                            else
                            {
                                currentNode.LeftChild = null;
                            }
                        }

                        if (currentNode.RightChild != null)
                        {
                            if (toCompare - currentNode.RightChild.LastSplayTime < freeUpResourcesTime)
                            {
                                splayTreeNodes.Push(currentNode.RightChild);
                                countedNodes++;
                            }
                            else
                            {
                                currentNode.RightChild = null;
                            }
                        }
                    }
                    else
                    {
                        currentNode.LeftChild = null;
                        currentNode.RightChild = null;

                        while (splayTreeNodes.Count != 0)
                        {
                            currentNode = splayTreeNodes.Pop();
                            currentNode.LeftChild = null;
                            currentNode.RightChild = null;
                        }
                    }
                }

                _count = countedNodes;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_count == 0)
            {
                value = default;
                return false;
            }

            Splay(key);
            if (key.CompareTo(_root.Key) != 0)
            {
                value = default;
                return false;
            }

            value = _root.Value;
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_count == 0)
                {
                    StaticLogger.Logger.Trace(new KeyNotFoundException("The key was not found in the tree.").Message);
                }

                Splay(key);
                if (key.CompareTo(this._root.Key) != 0)
                {
                    StaticLogger.Logger.Trace(new KeyNotFoundException("The key was not found in the tree.").Message);
                }

                return _root.Value;
            }

            set
            {
                Set(key, value, throwOnExisting: false);
            }
        }

        public int Count
        {
            get { return _count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_count == 0)
            {
                return false;
            }

            Splay(item.Key);

            if (item.Key.CompareTo(this._root.Key) == 0 && (object.ReferenceEquals(this._root.Value, item.Value) || (item.Value is object && item.Value.Equals(this._root.Value))))
            {
                return false;
            }

            if (_root.LeftChild == null)
            {
                _root = _root.RightChild;
            }
            else
            {
                var swap = _root.RightChild;
                _root = _root.LeftChild;
                Splay(item.Key);
                _root.RightChild = swap;
            }

            version++;
            _count--;
            return true;
        }

        public void Trim(int depth)
        {
            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException("depth", "The trim depth must not be negative.");
            }

            if (_count == 0)
            {
                return;
            }

            if (depth == 0)
            {
                this.Clear();
            }
            else
            {
                var prevCount = _count;
                _count = Trim(_root, depth - 1);
                if (prevCount != _count)
                {
                    version++;
                }
            }
        }

        private int Trim(SplayTreeNode<TKey, TValue> node, int depth)
        {
            if (depth == 0)
            {
                node.LeftChild = null;
                node.RightChild = null;
                return 1;
            }
            else
            { 
                int count = 1;

                if (node.LeftChild != null)
                {
                    count += Trim(node.LeftChild, depth - 1);
                }

                if (node.RightChild != null)
                {
                    count += Trim(node.RightChild, depth - 1);
                }

                return count;
            }
        }

        public ICollection<TKey> Keys
        {
            get { return new TiedList<TKey>(this, version, AsList(node => node.Key)); }
        }

        public ICollection<TValue> Values
        {
            get { return new TiedList<TValue>(this, version, AsList(node => node.Value)); }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.AsList(node => new KeyValuePair<TKey, TValue>(node.Key, node.Value)).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new TiedList<KeyValuePair<TKey, TValue>>(this, version, AsList(node => new KeyValuePair<TKey, TValue>(node.Key, node.Value))).GetEnumerator();
        }

        private IList<TEnumerator> AsList<TEnumerator>(Func<SplayTreeNode<TKey, TValue>, TEnumerator> selector)
        {
            if (_root == null)
            {
                return new TEnumerator[0];
            }

            var result = new List<TEnumerator>(_count);
            PopulateList(_root, result, selector);
            return result;
        }

        private void PopulateList<TEnumerator>(SplayTreeNode<TKey, TValue> node, List<TEnumerator> list, Func<SplayTreeNode<TKey, TValue>, TEnumerator> selector)
        {
            if (node.LeftChild != null) PopulateList(node.LeftChild, list, selector);
            list.Add(selector(node));
            if (node.RightChild != null) PopulateList(node.RightChild, list, selector);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class TiedList<T> : IList<T>
        {
            private readonly SplayTree<TKey, TValue> tree;
            private readonly int version;
            private readonly IList<T> backingList;

            public TiedList(SplayTree<TKey, TValue> tree, int version, IList<T> backingList)
            {
                if (tree == null)
                {
                    StaticLogger.Logger.Trace(new ArgumentNullException("tree").Message);
                }

                if (backingList == null)
                {
                    StaticLogger.Logger.Trace(new ArgumentNullException("backingList").Message);
                }

                this.tree = tree;
                this.version = version;
                this.backingList = backingList;
            }

            public int IndexOf(T item)
            {
                if (tree.version != this.version)
                    StaticLogger.Logger.Trace(new InvalidOperationException("The collection has been modified.").Message);
                return backingList.IndexOf(item);
            }

            public void Insert(int index, T item)
            {
                StaticLogger.Logger.Trace(new NotSupportedException().Message);
            }

            public void RemoveAt(int index)
            {
                StaticLogger.Logger.Trace(new NotSupportedException().Message);
            }

            public T this[int index]
            {
                get
                {
                    if (this.tree.version != this.version)
                        StaticLogger.Logger.Trace(new InvalidOperationException("The collection has been modified.").Message);
                    return this.backingList[index];
                }
                set
                {
                    StaticLogger.Logger.Trace(new NotSupportedException().Message);
                }
            }

            public void Add(T item)
            {
                StaticLogger.Logger.Trace(new NotSupportedException().Message);
            }

            public void Clear()
            {
                StaticLogger.Logger.Trace(new NotSupportedException().Message);
            }

            public bool Contains(T item)
            {
                if (this.tree.version != this.version)
                    StaticLogger.Logger.Trace(new InvalidOperationException("The collection has been modified.").Message);
                return this.backingList.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                if (this.tree.version != this.version)
                    StaticLogger.Logger.Trace(new InvalidOperationException("The collection has been modified.").Message);
                this.backingList.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return this.tree._count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(T item)
            {
                StaticLogger.Logger.Trace(new NotSupportedException().Message);
                return false;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (this.tree.version != this.version)
                    StaticLogger.Logger.Trace(new InvalidOperationException("The collection has been modified.").Message);

                foreach (var item in this.backingList)
                {
                    yield return item;
                    if (this.tree.version != this.version) 
                        StaticLogger.Logger.Trace(new InvalidOperationException("The collection has been modified.").Message);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
