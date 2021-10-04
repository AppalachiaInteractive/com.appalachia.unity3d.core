using System.Collections;
using System.Collections.Generic;

namespace Appalachia.Core.Collections.Observable
{
    public class ObservableList<T> : IList<T>
    {
        private readonly IList<T> m_List;

        public ObservableList() : this(0)
        {
        }

        public ObservableList(int capacity)
        {
            m_List = new List<T>(capacity);
        }

        public ObservableList(IEnumerable<T> collection)
        {
            m_List = new List<T>(collection);
        }

        public T this[int index]
        {
            get => m_List[index];
            set
            {
                OnEvent(ItemRemoved, index, m_List[index]);
                m_List[index] = value;
                OnEvent(ItemAdded, index, value);
            }
        }

        public int Count => m_List.Count;

        public bool IsReadOnly => false;

        public bool Contains(T item)
        {
            return m_List.Contains(item);
        }

        public int IndexOf(T item)
        {
            return m_List.IndexOf(item);
        }

        public void Add(T item)
        {
            m_List.Add(item);
            OnEvent(ItemAdded, m_List.IndexOf(item), item);
        }

        public void Insert(int index, T item)
        {
            m_List.Insert(index, item);
            OnEvent(ItemAdded, index, item);
        }

        public bool Remove(T item)
        {
            var index = m_List.IndexOf(item);
            var ret = m_List.Remove(item);
            if (ret)
            {
                OnEvent(ItemRemoved, index, item);
            }

            return ret;
        }

        public void RemoveAt(int index)
        {
            var item = m_List[index];
            m_List.RemoveAt(index);
            OnEvent(ItemRemoved, index, item);
        }

        public void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                RemoveAt(i);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_List.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event ListChangedEventHandler<T> ItemAdded;
        public event ListChangedEventHandler<T> ItemRemoved;

        private void OnEvent(ListChangedEventHandler<T> e, int index, T item)
        {
            if (e != null)
            {
                e(this, new ListChangedEventArgs<T>(index, item));
            }
        }

        public void Add(params T[] items)
        {
            for (var index = 0; index < items.Length; index++)
            {
                var i = items[index];
                Add(i);
            }
        }

        public int Remove(params T[] items)
        {
            if (items == null)
            {
                return 0;
            }

            var count = 0;

            for (var index = 0; index < items.Length; index++)
            {
                var i = items[index];
                count += Remove(i) ? 1 : 0;
            }

            return count;
        }
    }
}
