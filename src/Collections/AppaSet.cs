#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Appalachia.Core.Collections.Interfaces;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    public abstract class AppaSet<TValue, TValueList> : ISerializationCallbackReceiver,
                                                        IAppaSetState<TValue>,
                                                        IAppaSet<TValue>,
                                                        ICollection<TValue>,
                                                        IReadOnlyList<TValue>
        where TValueList : AppaList<TValue>, new()
    {
        private const string _PRF_PFX = nameof(AppaSet<TValue, TValueList>) + ".";

        private static readonly ProfilerMarker _PRF_Remove = new(_PRF_PFX + nameof(Remove));
        private static readonly ProfilerMarker _PRF_CopyTo = new(_PRF_PFX + nameof(CopyTo));
        private static readonly ProfilerMarker _PRF_Count = new(_PRF_PFX + nameof(Count));
        private static readonly ProfilerMarker _PRF_Add = new(_PRF_PFX + nameof(Add));

        private static readonly ProfilerMarker _PRF_Indexer = new(_PRF_PFX + "Indexer");
        private static readonly ProfilerMarker _PRF_RemoveAt = new(_PRF_PFX + nameof(RemoveAt));
        private static readonly ProfilerMarker _PRF_AddRange = new(_PRF_PFX + nameof(AddRange));

        private static readonly ProfilerMarker _PRF_Clear = new(_PRF_PFX + nameof(Clear));

        private static readonly ProfilerMarker _PRF_Contains = new(_PRF_PFX + nameof(Contains));

        private static readonly ProfilerMarker _PRF_GetByIndex = new(_PRF_PFX + nameof(GetByIndex));

        private static readonly ProfilerMarker _PRF_IfPresent = new(_PRF_PFX + nameof(IfPresent));

        private static readonly ProfilerMarker _PRF_SumCounts = new(_PRF_PFX + nameof(SumCounts));

        private static readonly ProfilerMarker _PRF_INTERNAL_CLEAR = new(_PRF_PFX + nameof(INTERNAL_CLEAR));

        private static readonly ProfilerMarker _PRF_INTERNAL_GET_VALUE_BY_INDEX =
            new(_PRF_PFX + nameof(INTERNAL_GET_VALUE_BY_INDEX));

        private static readonly ProfilerMarker _PRF_INTERNAL_CONTAINS =
            new(_PRF_PFX + nameof(INTERNAL_CONTAINS));

        private static readonly ProfilerMarker _PRF_INTERNAL_REMOVE = new(_PRF_PFX + nameof(INTERNAL_REMOVE));

        private static readonly ProfilerMarker _PRF_INTERNAL_ADD = new(_PRF_PFX + nameof(INTERNAL_ADD));

        private static readonly ProfilerMarker _PRF_INTERNAL_INITIALIZE =
            new(_PRF_PFX + nameof(INTERNAL_INITIALIZE));

        private static readonly ProfilerMarker _PRF_INTERNAL_CHECK_FATAL =
            new(_PRF_PFX + nameof(INTERNAL_CHECK_FATAL));

        private static readonly ProfilerMarker _PRF_INTERNAL_REQUIRES_REBUILD =
            new(_PRF_PFX + nameof(INTERNAL_REQUIRES_REBUILD));

        private static readonly ProfilerMarker _PRF_INTERNAL_REBUILD =
            new(_PRF_PFX + nameof(INTERNAL_REBUILD));

        protected AppaSet()
        {
            INTERNAL_INITIALIZE(false);
        }

        [NonSerialized] private Action _setDirtyAction;

        [NonSerialized] private bool _isValueUnityObjectChecked;

        [NonSerialized] private bool isValueUnityObject;

        [NonSerialized] private Dictionary<TValue, int> _indices;

        [SerializeField]
        [HideInInspector]
        private int initializerCount = 64;

        [SerializeField]
        [HideInInspector]
        private TValueList values;

        public bool NoTracking { get; set; } = false;

        [field: NonSerialized] public int LastFrameCheck { get; private set; } = -1;

        public int Count
        {
            get
            {
                using (_PRF_Count.Auto())
                {
                    INTERNAL_INITIALIZE();

                    return values.Count;
                }
            }
        }

        public IReadOnlyList<TValue> Values => values;

        public int InitializerCount
        {
            get => initializerCount;
            set => initializerCount = value;
        }

        public bool IsReadOnly => false;

        public TValue this[int index]
        {
            get
            {
                using (_PRF_Indexer.Auto())
                {
                    INTERNAL_INITIALIZE();
                    return INTERNAL_GET_VALUE_BY_INDEX(index);
                }
            }
        }

        public void RemoveNulls()
        {
            if (values.RemoveNulls())
            {
                INTERNAL_REBUILD();
            }
        }

        public bool Remove(TValue key)
        {
            using (_PRF_Remove.Auto())
            {
                INTERNAL_INITIALIZE();

                if (INTERNAL_CONTAINS(key))
                {
                    var targetIndex = _indices[key];

                    INTERNAL_REMOVE(targetIndex);
                    return true;
                }

                return false;
            }
        }

        public TValue RemoveAt(int targetIndex)
        {
            using (_PRF_RemoveAt.Auto())
            {
                INTERNAL_INITIALIZE();

                return INTERNAL_REMOVE(targetIndex);
            }
        }

        public void Add(TValue value)
        {
            using (_PRF_Add.Auto())
            {
                INTERNAL_INITIALIZE();
                INTERNAL_ADD(value);
            }
        }

        public void AddRange(IList<TValue> vals)
        {
            using (_PRF_AddRange.Auto())
            {
                INTERNAL_INITIALIZE();

                for (var i = 0; i < vals.Count; i++)
                {
                    var value = INTERNAL_GET_VALUE_BY_INDEX(i);

                    INTERNAL_ADD(value);
                }
            }
        }

        public void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                INTERNAL_INITIALIZE();
                INTERNAL_CLEAR();
            }
        }

        public bool Contains(TValue item)
        {
            using (_PRF_Contains.Auto())
            {
                INTERNAL_INITIALIZE();
                return (item != null) && INTERNAL_CONTAINS(item);
            }
        }

        public int SumCounts(Func<TValue, int> counter)
        {
            using (_PRF_SumCounts.Auto())
            {
                INTERNAL_INITIALIZE();

                var sum = 0;

                var count = Count;

                for (var i = 0; i < count; i++)
                {
                    sum += counter(INTERNAL_GET_VALUE_BY_INDEX(i));
                }

                return sum;
            }
        }

        public TValue GetByIndex(int i)
        {
            using (_PRF_GetByIndex.Auto())
            {
                INTERNAL_INITIALIZE();

                return INTERNAL_GET_VALUE_BY_INDEX(i);
            }
        }

        public void IfPresent(TValue value, Action present, Action notPresent)
        {
            using (_PRF_IfPresent.Auto())
            {
                INTERNAL_INITIALIZE();

                if (INTERNAL_CONTAINS(value))
                {
                    present();
                }
                else
                {
                    notPresent();
                }
            }
        }

        public void SetDirtyAction(Action a)
        {
            _setDirtyAction = a;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            using (_PRF_CopyTo.Auto())
            {
                INTERNAL_INITIALIZE();

                for (var i = arrayIndex; i < array.Length; i++)
                {
                    array[i] = values[i - arrayIndex];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool INTERNAL_CONTAINS(TValue value)
        {
            using (_PRF_INTERNAL_CONTAINS.Auto())
            {
                return _indices.ContainsKey(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool INTERNAL_REQUIRES_REBUILD()
        {
            using (_PRF_INTERNAL_REQUIRES_REBUILD.Auto())
            {
                var valueCount = values.Count;

                if (valueCount == 0)
                {
                    _indices.Clear();
                    _setDirtyAction?.Invoke();
                    return false;
                }

                if (isValueUnityObject)
                {
                    if (values.RemoveNulls())
                    {
                        return true;
                    }
                }

                var indexCount = _indices.Count;

                if (indexCount != valueCount)
                {
                    return true;
                }

                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue INTERNAL_GET_VALUE_BY_INDEX(int index)
        {
            using (_PRF_INTERNAL_GET_VALUE_BY_INDEX.Auto())
            {
                return values[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue INTERNAL_REMOVE(int targetIndex)
        {
            using (_PRF_INTERNAL_REMOVE.Auto())
            {
                if ((targetIndex >= values.Count) || (targetIndex < 0))
                {
                    throw new IndexOutOfRangeException(
                        $"Index [{targetIndex}] is out of range for collection of length [{values.Count}]."
                    );
                }

                var deletingValue = values[targetIndex];

                _indices.Remove(deletingValue);

                var lastIndex = values.Count - 1;

                if (lastIndex != targetIndex)
                {
                    var lastValue = values[lastIndex];

                    values[targetIndex] = lastValue;

                    _indices[lastValue] = targetIndex;
                }

                values.RemoveAt(lastIndex);

                _setDirtyAction?.Invoke();
                return deletingValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_ADD(TValue value)
        {
            using (_PRF_INTERNAL_ADD.Auto())
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _indices.Add(value, _indices.Count);
                values.Add(value);
                _setDirtyAction?.Invoke();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_CHECK_FATAL()
        {
            using (_PRF_INTERNAL_CHECK_FATAL.Auto())
            {
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_CLEAR()
        {
            using (_PRF_INTERNAL_CLEAR.Auto())
            {
                values.Clear();
                _indices.Clear();
                _setDirtyAction?.Invoke();
            }
        }

        private void INTERNAL_INITIALIZE(bool getFrameCount = true)
        {
            using (_PRF_INTERNAL_INITIALIZE.Auto())
            {
                if (getFrameCount)
                {
                    var frameCount = Time.frameCount;

                    if (LastFrameCheck == frameCount)
                    {
                        return;
                    }

                    LastFrameCheck = frameCount;
                }

                if (values == null)
                {
                    values = new TValueList {Capacity = initializerCount};

                    ;
                    _setDirtyAction?.Invoke();
                }

                if (_indices == null)
                {
                    _indices = new Dictionary<TValue, int>(initializerCount);
                    _setDirtyAction?.Invoke();
                }

                if (!_isValueUnityObjectChecked)
                {
                    isValueUnityObject = typeof(Object).IsAssignableFrom(typeof(TValue));
                    _isValueUnityObjectChecked = true;
                }

                //INTERNAL_CHECK_FATAL();

                if (INTERNAL_REQUIRES_REBUILD())
                {
                    INTERNAL_REBUILD();
                }

                //INTERNAL_CHECK_FATAL();

                if (values.Count > initializerCount)
                {
                    initializerCount = values.Count;
                    _setDirtyAction?.Invoke();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_REBUILD()
        {
            using (_PRF_INTERNAL_REBUILD.Auto())
            {
                _indices.Clear();

                var newIndex = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    var value = values[i];

                    _indices.Add(value, newIndex);

                    newIndex += 1;
                }

                _setDirtyAction?.Invoke();
            }
        }

        bool ICollection<TValue>.Remove(TValue item)
        {
            using (_PRF_Remove.Auto())
            {
                INTERNAL_INITIALIZE();

                return Remove(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            INTERNAL_INITIALIZE();
            return values.GetEnumerator();
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            INTERNAL_INITIALIZE();
            return values.GetEnumerator();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            INTERNAL_INITIALIZE(false);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
    }
}
