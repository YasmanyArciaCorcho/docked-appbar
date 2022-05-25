using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataStructures.SplayTree
{
    public sealed class SplayTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public readonly TKey Key;

        public TValue Value;
        public SplayTreeNode<TKey, TValue> LeftChild;
        public SplayTreeNode<TKey, TValue> RightChild;
        public DateTime LastSplayTime
        { get; set; }

        public SplayTreeNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
