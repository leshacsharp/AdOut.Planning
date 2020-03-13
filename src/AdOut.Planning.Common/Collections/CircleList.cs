using System;
using System.Collections;
using System.Collections.Generic;

namespace AdOut.Planning.Common.Collections
{
    public class CircleList<T> : IEnumerable<T>, ICollection<T>
    {
        private LinkedList<T> _list;
        private LinkedListNode<T> _current;

        public CircleList(IEnumerable<T> collection)
        {
            _list = new LinkedList<T>(collection);
        }

        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public T Next()
        {  
            if(Count == 0)
            {
                throw new InvalidOperationException("CircleList is empty");
            }

            if (_current == null)
            {
                _current = _list.First;
            }
            else
            {
                //circle
                _current = _current.Next ?? _list.First;
            }

            return _current.Value;
        }

        public void Add(T item)
        {
            _list.AddLast(item);
        }

        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (_current != null)
            {
                yield return Next();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
