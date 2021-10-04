#region

using System;
using System.Collections;
using System.Collections.Generic;
using Appalachia.Core.Comparisons;
using Appalachia.Pooling.Arrays;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    public abstract class AppaList<T> : IReadOnlyList<T>, ISerializationCallbackReceiver, IList<T>
    {
        protected const int _DEFAULT_CAPACITY = 4;

        // ReSharper disable once StaticMemberInGenericType
        private static object __itemPoolLock = new();

        private static DefaultArrayPool<T> __itemPool;

        private static readonly ProfilerMarker _PRF_Reverse = new(_PRF_PFX + nameof(Reverse));

        [SerializeField] private bool noTracking;

        [SerializeField] protected T[] _values;

        [SerializeField] protected float _capacityIncreaseMultiplier = 2.0f;

        [SerializeField] private int _count;

        private static DefaultArrayPool<T> _itemPool
        {
            get
            {
                if (__itemPoolLock == null)
                {
                    __itemPoolLock = new object();
                }

                lock (__itemPoolLock)
                {
                    if (__itemPool == null)
                    {
                        lock (__itemPoolLock)
                        {
                            __itemPool = new DefaultArrayPool<T>();
                        }
                    }
                }

                return __itemPool;
            }
        }

        public int Capacity
        {
            get => _values.Length;
            set => SetCapacity(value);
        }

        public float CapacityIncreaseMultiplier
        {
            get => _capacityIncreaseMultiplier;
            set => _capacityIncreaseMultiplier = value;
        }

        public bool IsReadOnly => false;

        public bool Contains(T item)
        {
            using (_PRF_Contains.Auto())
            {
                return Array.IndexOf(_values, item, 0, Count) != -1;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            using (_PRF_CopyTo.Auto())
            {
                for (var i = arrayIndex; i < array.Length; i++)
                {
                    array[i] = _values[i - arrayIndex];
                }
            }
        }

        // Slow
        public virtual bool Remove(T item)
        {
            using (_PRF_Remove.Auto())
            {
                var index = Array.IndexOf(_values, item, 0, Count);

                if (index >= 0)
                {
                    _values[index] = _values[--Count];
                    _values[Count] = default;

                    return true;
                }

                return false;
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public virtual void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                Array.Clear(_values, 0, Count);
                _count = 0;
            }
        }

        public int IndexOf(T item)
        {
            using (_PRF_IndexOf.Auto())
            {
                return Array.IndexOf(_values, item, 0, Count);
            }
        }

        public virtual void Insert(int index, T item)
        {
            using (_PRF_Insert.Auto())
            {
                if (index > Count)
                {
                    Debug.LogError("Index " + index + " is out of range " + Count);
                }

                if (Count == _values.Length)
                {
                    IncreaseCapacity();
                }

                if (index < Count)
                {
                    Array.Copy(_values, index, _values, index + 1, Count - index);
                }

                _values[index] = item;
                Count = ++Count;
            }
        }

        public virtual void RemoveAt(int index)
        {
            using (_PRF_RemoveAt.Auto())
            {
                if (index >= Count)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"Index {index} is out of range. List count is {Count}."
                    );
                }

                _values[index] = _values[--Count];
                _values[Count] = default;
            }
        }

        public T this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public int Count
        {
            get => _count;
            set => _count = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _values).GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                if (__itemPoolLock == null)
                {
                    __itemPoolLock = new object();
                }
            }
        }

        ~AppaList()
        {
            if (_values != null)
            {
                _itemPool.Return(_values);
            }
        }

        public void Dispose()
        {
            if (_values != null)
            {
                _itemPool.Return(_values);
                _values = null;
            }
        }

        protected void IncreaseCapacity()
        {
            using (_PRF_IncreaseCapacity.Auto())
            {
                var baseSize = Mathf.Max(1.0f, _values.Length);

                var arraySize = (int) (baseSize * _capacityIncreaseMultiplier);

                var tempItemArray = _itemPool.Rent(arraySize);

                Array.Copy(_values, tempItemArray, Count);

                _itemPool.Return(_values);

                _values = tempItemArray;
            }
        }

        protected void SetCapacity(int capacity)
        {
            using (_PRF_SetCapacity.Auto())
            {
                var newItems = _itemPool.Rent(capacity);

                if (Count > 0)
                {
                    Array.Copy(_values, newItems, Count);
                }

                _itemPool.Return(_values);
                _values = newItems;
            }
        }

        public void SetCount(int count)
        {
            using (_PRF_SetCount.Auto())
            {
                if (count > _values.Length)
                {
                    SetCapacity(count);
                }

                Count = count;
            }
        }

        public void EnsureCount(int count)
        {
            using (_PRF_EnsureCount.Auto())
            {
                if (count <= Count)
                {
                    return;
                }

                if (count > _values.Length)
                {
                    SetCapacity(count);
                }

                Count = count;
            }
        }

        public virtual void SetArray(T[] items)
        {
            using (_PRF_SetArray.Auto())
            {
                if (_values != null)
                {
                    _itemPool.Return(_values);
                }

                _values = items;
                Count = _values.Length;
            }
        }

        public int AddUnique(T item)
        {
            using (_PRF_AddUnique.Auto())
            {
                if (!Contains(item))
                {
                    return Add(item);
                }

                return -1;
            }
        }

        public T GetIndex(T item)
        {
            using (_PRF_GetIndex.Auto())
            {
                var index = Array.IndexOf(_values, item, 0, Count);
                if (index == -1)
                {
                    return default;
                }

                return _values[index];
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int Add(T item)
        {
            using (_PRF_Add.Auto())
            {
                if (Count == _values.Length)
                {
                    IncreaseCapacity();
                }

                _values[Count] = item;
                Count = ++Count;
                return Count - 1;
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int AddThreadSafe(T item)
        {
            using (_PRF_AddThreadSafe.Auto())
            {
                lock (this)
                {
                    if (Count == _values.Length)
                    {
                        IncreaseCapacity();
                    }

                    _values[Count] = item;
                    Count = ++Count;
                    return Count - 1;
                }
            }
        }

        public virtual void Add(T item, T item2)
        {
            using (_PRF_Add.Auto())
            {
                if ((Count + 1) >= _values.Length)
                {
                    IncreaseCapacity();
                }

                _values[Count++] = item;
                _values[Count++] = item2;
            }
        }

        public virtual void Add(T item, T item2, T item3)
        {
            using (_PRF_Add.Auto())
            {
                if ((Count + 2) >= _values.Length)
                {
                    IncreaseCapacity();
                }

                _values[Count++] = item;
                _values[Count++] = item2;
                _values[Count++] = item3;
            }
        }

        public virtual void Add(T item, T item2, T item3, T item4)
        {
            using (_PRF_Add.Auto())
            {
                if ((Count + 3) >= _values.Length)
                {
                    IncreaseCapacity();
                }

                _values[Count++] = item;
                _values[Count++] = item2;
                _values[Count++] = item3;
                _values[Count++] = item4;
            }
        }

        public virtual void Add(T item, T item2, T item3, T item4, T item5)
        {
            using (_PRF_Add.Auto())
            {
                if ((Count + 4) >= _values.Length)
                {
                    IncreaseCapacity();
                }

                _values[Count++] = item;
                _values[Count++] = item2;
                _values[Count++] = item3;
                _values[Count++] = item4;
                _values[Count++] = item5;
            }
        }

        public virtual void AddRange(T[] arrayItems)
        {
            using (_PRF_AddRange.Auto())
            {
                if (arrayItems == null)
                {
                    return;
                }

                var length = arrayItems.Length;
                var newCount = Count + length;
                if (newCount >= _values.Length)
                {
                    SetCapacity(newCount * 2);
                }

                Array.Copy(arrayItems, 0, _values, Count, length);
                Count = newCount;
            }
        }

        public virtual void AddRange(T[] arrayItems, int startIndex, int length)
        {
            using (_PRF_AddRange.Auto())
            {
                var newCount = Count + length;
                if (newCount >= _values.Length)
                {
                    SetCapacity(newCount * 2);
                }

                Array.Copy(arrayItems, startIndex, _values, Count, length);
                Count = newCount;
            }
        }

        public virtual void AddRange(AppaList<T> list)
        {
            using (_PRF_AddRange.Auto())
            {
                if (list.Count == 0)
                {
                    return;
                }

                var newCount = Count + list.Count;
                if (newCount >= _values.Length)
                {
                    SetCapacity(newCount * 2);
                }

                Array.Copy(list._values, 0, _values, Count, list.Count);
                Count = newCount;
            }
        }

        public virtual int GrabListThreadSafe(AppaList<T> threadList, bool fastClear = false)
        {
            using (_PRF_GrabListThreadSafe.Auto())
            {
                lock (threadList)
                {
                    var count = Count;
                    AddRange(threadList);
                    if (fastClear)
                    {
                        threadList.ClearFast();
                    }
                    else
                    {
                        threadList.Clear();
                    }

                    return count;
                }
            }
        }

        public virtual void ChangeRange(int startIndex, T[] arrayItems)
        {
            using (_PRF_ChangeRange.Auto())
            {
                for (var i = 0; i < arrayItems.Length; i++)
                {
                    _values[startIndex + i] = arrayItems[i];
                }
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void RemoveLast()
        {
            using (_PRF_RemoveLast.Auto())
            {
                if (Count == 0)
                {
                    return;
                }

                --Count;

                _values[Count] = default;
            }
        }

        public virtual void RemoveRange(int index, int length)
        {
            using (_PRF_RemoveRange.Auto())
            {
                if ((Count - index) < length)
                {
                    Debug.LogError("Invalid length!");
                }

                if (length > 0)
                {
                    Count -= length;
                    if (index < Count)
                    {
                        Array.Copy(_values, index + length, _values, index, Count - index);
                    }

                    Array.Clear(_values, Count, length);
                }
            }
        }

        public virtual T Dequeue()
        {
            using (_PRF_Dequeue.Auto())
            {
                if (Count == 0)
                {
                    throw new NotSupportedException("List is empty!");
                }

                var item = _values[--Count];

                _values[Count] = default;

                return item;
            }
        }

        public virtual T Dequeue(int index)
        {
            using (_PRF_Dequeue.Auto())
            {
                var item = _values[index];

                _values[index] = _values[--Count];
                _values[Count] = default;

                return item;
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void ClearThreadSafe()
        {
            using (_PRF_ClearThreadSafe.Auto())
            {
                lock (this)
                {
                    Array.Clear(_values, 0, Count);
                    _count = 0;
                }
            }
        }

        public virtual void ClearRange(int startIndex)
        {
            using (_PRF_ClearRange.Auto())
            {
                Array.Clear(_values, startIndex, Count - startIndex);
                Count = startIndex;
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void ClearFast()
        {
            using (_PRF_ClearFast.Auto())
            {
                Count = 0;
            }
        }

        public virtual void ClearFast(int newCount)
        {
            using (_PRF_ClearFast.Auto())
            {
                if (newCount < Count)
                {
                    Count = newCount;
                }
            }
        }

        public virtual T[] ToArray()
        {
            using (_PRF_ToArray.Auto())
            {
                var array = new T[Count];

                var start = 0;
                var length = Count;

                Array.Copy(_values, start, array, 0, length);
                return array;
            }
        }

        public virtual T[] ToArray(int length)
        {
            using (_PRF_ToArray.Auto())
            {
                var array = new T[Count];

                var start = 0;

                Array.Copy(_values, start, array, 0, length);
                return array;
            }
        }

        public virtual T[] ToArray(int start, int length)
        {
            using (_PRF_ToArray.Auto())
            {
                var array = new T[Count];

                Array.Copy(_values, start, array, 0, length);
                return array;
            }
        }

        public void Sort()
        {
            using (_PRF_Sort.Auto())
            {
                Array.Sort(_values);
            }
        }

        public void Sort(IComparer<T> comparer)
        {
            using (_PRF_Sort.Auto())
            {
                Sort(0, Count, comparer);
            }
        }

        public void Sort(int index, int length, IComparer<T> comparer)
        {
            using (_PRF_Sort.Auto())
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"Index {index} is out of range. List count is {Count}."
                    );
                }

                if ((length < 0) || ((Count - index) < length))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(length),
                        $"Length {length} is out of range. List count is {Count} and index is {index}."
                    );
                }

                Array.Sort(_values, index, length, comparer);
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            using (_PRF_Sort.Auto())
            {
                if (comparison == null)
                {
                    throw new ArgumentNullException(nameof(comparison));
                }

                if (Count <= 0)
                {
                    return;
                }

                Array.Sort(_values, comparison);
            }
        }

        public void Sort<T2>(AppaList<T2> paired, IComparer<T> comparer)
        {
            using (_PRF_Sort.Auto())
            {
                Sort(paired, 0, Count, comparer);
            }
        }

        public void Sort<T2>(AppaList<T2> paired, int index, int length, IComparer<T> comparer)
        {
            using (_PRF_Sort.Auto())
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"Index {index} is out of range. List count is {Count}."
                    );
                }

                if ((length < 0) || ((Count - index) < length))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(length),
                        $"Length {length} is out of range. List count is {Count} and index is {index}."
                    );
                }

                var others = paired._values;
                Array.Sort(_values, others, index, length, comparer);
                paired._values = others;
            }
        }

        public void Sort<T2>(AppaList<T2> paired, Comparison<T> comparison)
        {
            using (_PRF_Sort.Auto())
            {
                Sort(paired, 0, Count, comparison);
            }
        }

        public void Sort<T2>(AppaList<T2> paired, int index, int length, Comparison<T> comparison)
        {
            using (_PRF_Sort.Auto())
            {
                if (comparison == null)
                {
                    throw new ArgumentNullException(nameof(comparison));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"Index {index} is out of range. List count is {Count}."
                    );
                }

                if ((length < 0) || ((Count - index) < length))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(length),
                        $"Length {length} is out of range. List count is {Count} and index is {index}."
                    );
                }

                var others = paired._values;
                Array.Sort(_values, others, index, length, new ComparisonWrapper<T>(comparison));
                paired._values = others;
            }
        }

        public void Reverse()
        {
            using (_PRF_Reverse.Auto())
            {
                Array.Reverse(_values, 0, Count);
            }
        }

        public void Reverse(int index, int length)
        {
            using (_PRF_Reverse.Auto())
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"Index {index} is out of range. List count is {Count}."
                    );
                }

                if ((length < 0) || ((Count - index) < length))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(length),
                        $"Length {length} is out of range. List count is {Count} and index is {index}."
                    );
                }

                Array.Reverse(_values, index, length);
            }
        }

        public bool RemoveNulls(AppaList<int> removedIndicesSafeOrdered)
        {
            using (_PRF_RemoveNulls.Auto())
            {
                removedIndicesSafeOrdered.ClearFast();

                var hadNulls = false;

                for (var i = _count - 1; i >= 0; i--)
                {
                    var obj = _values[i];

                    if (obj == null)
                    {
                        hadNulls = true;
                        RemoveAt(i);
                        removedIndicesSafeOrdered.Add(i);
                    }
                }

                return hadNulls;
            }
        }

        public bool RemoveNulls()
        {
            using (_PRF_RemoveNulls.Auto())
            {
                var hadNulls = false;

                for (var i = _count - 1; i >= 0; i--)
                {
                    var obj = _values[i];

                    if (obj == null)
                    {
                        hadNulls = true;
                        RemoveAt(i);
                    }
                }

                return hadNulls;
            }
        }

        public T[] GetInternalArray()
        {
            return _values;
        }

        public void TrimExcess()
        {
            using (_PRF_TrimExcess.Auto())
            {
                var count = _count;
                var length = _values.Length;

                if (count == length)
                {
                    return;
                }

                var temp = new T[count];
                Array.Copy(_values, 0, temp, 0, count);
                _values = temp;
            }
        }

#region Performance Markers

        private const string _PRF_PFX = nameof(AppaList<T>) + ".";
        private static readonly ProfilerMarker _PRF_Add = new(_PRF_PFX + nameof(Add));
        private static readonly ProfilerMarker _PRF_AddRange = new(_PRF_PFX + nameof(AddRange));

        private static readonly ProfilerMarker _PRF_AddThreadSafe =
            new(_PRF_PFX + nameof(AddThreadSafe));

        private static readonly ProfilerMarker _PRF_AddUnique = new(_PRF_PFX + nameof(AddUnique));

        private static readonly ProfilerMarker _PRF_ChangeRange =
            new(_PRF_PFX + nameof(ChangeRange));

        private static readonly ProfilerMarker _PRF_Clear = new(_PRF_PFX + nameof(Clear));
        private static readonly ProfilerMarker _PRF_ClearFast = new(_PRF_PFX + nameof(ClearFast));
        private static readonly ProfilerMarker _PRF_ClearRange = new(_PRF_PFX + nameof(ClearRange));

        private static readonly ProfilerMarker _PRF_ClearThreadSafe =
            new(_PRF_PFX + nameof(ClearThreadSafe));

        private static readonly ProfilerMarker _PRF_Contains = new(_PRF_PFX + nameof(Contains));
        private static readonly ProfilerMarker _PRF_CopyTo = new(_PRF_PFX + nameof(CopyTo));
        private static readonly ProfilerMarker _PRF_Dequeue = new(_PRF_PFX + nameof(Dequeue));

        private static readonly ProfilerMarker _PRF_EnsureCount =
            new(_PRF_PFX + nameof(EnsureCount));

        private static readonly ProfilerMarker _PRF_GetIndex = new(_PRF_PFX + nameof(GetIndex));

        private static readonly ProfilerMarker _PRF_GrabListThreadSafe =
            new(_PRF_PFX + nameof(GrabListThreadSafe));

        private static readonly ProfilerMarker _PRF_IncreaseCapacity =
            new(_PRF_PFX + nameof(IncreaseCapacity));

        private static readonly ProfilerMarker _PRF_IndexOf = new(_PRF_PFX + nameof(IndexOf));
        private static readonly ProfilerMarker _PRF_Insert = new(_PRF_PFX + nameof(Insert));
        private static readonly ProfilerMarker _PRF_AppaList = new(_PRF_PFX + nameof(AppaList<T>));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new(_PRF_PFX + nameof(OnAfterDeserialize));

        private static readonly ProfilerMarker _PRF_Remove = new(_PRF_PFX + nameof(Remove));
        private static readonly ProfilerMarker _PRF_RemoveAt = new(_PRF_PFX + nameof(RemoveAt));
        private static readonly ProfilerMarker _PRF_RemoveLast = new(_PRF_PFX + nameof(RemoveLast));

        private static readonly ProfilerMarker _PRF_RemoveNulls =
            new(_PRF_PFX + nameof(RemoveNulls));

        private static readonly ProfilerMarker _PRF_RemoveRange =
            new(_PRF_PFX + nameof(RemoveRange));

        private static readonly ProfilerMarker _PRF_SetArray = new(_PRF_PFX + nameof(SetArray));

        private static readonly ProfilerMarker _PRF_SetCapacity =
            new(_PRF_PFX + nameof(SetCapacity));

        private static readonly ProfilerMarker _PRF_SetCount = new(_PRF_PFX + nameof(SetCount));
        private static readonly ProfilerMarker _PRF_Sort = new(_PRF_PFX + nameof(Sort));
        private static readonly ProfilerMarker _PRF_ToArray = new(_PRF_PFX + nameof(ToArray));
        private static readonly ProfilerMarker _PRF_TrimExcess = new(_PRF_PFX + nameof(TrimExcess));

#endregion

#region Constructors

        protected AppaList() : this(4)
        {
        }

        protected AppaList(
            int capacity,
            float capacityIncreaseMultiplier = 2.0f,
            bool noTracking = false)
        {
            using (_PRF_AppaList.Auto())
            {
                if (capacity < 1)
                {
                    capacity = 1;
                }

                if (capacityIncreaseMultiplier <= 1.0f)
                {
                    capacityIncreaseMultiplier = 1.1f;
                }

                _values = _itemPool.Rent(capacity);

                //_items = new T[capacity];

                _capacityIncreaseMultiplier = capacityIncreaseMultiplier;
                this.noTracking = noTracking;

                // Debug.Log(items.Length);
            }
        }

        protected AppaList(AppaList<T> list)
        {
            using (_PRF_AppaList.Auto())
            {
                if (list == null)
                {
                    //_items = new T[_DEFAULT_CAPACITY];
                    _values = _itemPool.Rent(_DEFAULT_CAPACITY);
                }
                else
                {
                    //_items = new T[list.Count];
                    _values = _itemPool.Rent(list.Count);
                    Array.Copy(list._values, _values, list.Count);
                    Count = _values.Length;
                }
            }
        }

        protected AppaList(T[] values)
        {
            using (_PRF_AppaList.Auto())
            {
                _values = values;
                Count = values.Length;
            }
        }

#endregion
    }
}
