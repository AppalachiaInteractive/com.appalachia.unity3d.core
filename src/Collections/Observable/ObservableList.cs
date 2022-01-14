using System.Collections;
using System.Collections.Generic;

namespace Appalachia.Core.Collections.Observable
{
    public class ObservableList<T> : IList<T>
    {
        public event ListChangedEventHandler<T> ItemAdded;
        public event ListChangedEventHandler<T> ItemRemoved;

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

        #region Fields and Autoproperties

        private readonly IList<T> m_List;

        #endregion

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

        private void OnEvent(ListChangedEventHandler<T> e, int index, T item)
        {
            if (e != null)
            {
                e(this, new ListChangedEventArgs<T>(index, item));
            }
        }

        #region IList<T> Members

        public bool IsReadOnly => false;

        public int Count => m_List.Count;

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

        public bool Contains(T item)
        {
            return m_List.Contains(item);
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

        public void Add(T item)
        {
            m_List.Add(item);
            OnEvent(ItemAdded, m_List.IndexOf(item), item);
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

        public int IndexOf(T item)
        {
            return m_List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_List.Insert(index, item);
            OnEvent(ItemAdded, index, item);
        }

        public void RemoveAt(int index)
        {
            var item = m_List[index];
            m_List.RemoveAt(index);
            OnEvent(ItemRemoved, index, item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
