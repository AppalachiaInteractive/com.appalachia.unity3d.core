#region

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    public abstract class AppaQueue<T> : ISerializationCallbackReceiver,
                                         IEnumerable<T>,
                                         IEnumerable,
                                         ICollection,
                                         IReadOnlyCollection<T>
    {
        private const string _PRF_PFX = nameof(AppaQueue<T>) + ".";

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new(_PRF_PFX + nameof(OnAfterDeserialize));

        private static readonly ProfilerMarker _PRF_Initialize = new(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Clear = new(_PRF_PFX + nameof(Clear));

        private static readonly ProfilerMarker _PRF_CopyTo = new(_PRF_PFX + nameof(CopyTo));

        private static readonly ProfilerMarker _PRF_Enqueue = new(_PRF_PFX + nameof(Enqueue));

        private static readonly ProfilerMarker _PRF_Dequeue = new(_PRF_PFX + nameof(Dequeue));

        private static readonly ProfilerMarker _PRF_Peek = new(_PRF_PFX + nameof(Peek));

        private static readonly ProfilerMarker _PRF_Contains = new(_PRF_PFX + nameof(Contains));

        private static readonly ProfilerMarker _PRF_ToArray = new(_PRF_PFX + nameof(ToArray));

        private static readonly ProfilerMarker _PRF_TrimExcess = new(_PRF_PFX + nameof(TrimExcess));

        [SerializeField]
        [HideInInspector]
        private T[] _array;

        private Queue<T> _queue;

        public AppaQueue()
        {
            _queue = new Queue<T>();
        }

        public int Count => _queue.Count;

        public void CopyTo(Array array, int index)
        {
            using (_PRF_CopyTo.Auto())
            {
                ((ICollection) _queue).CopyTo(array, index);
            }
        }

        int ICollection.Count => _queue.Count;

        public bool IsSynchronized => ((ICollection) _queue).IsSynchronized;

        public object SyncRoot => ((ICollection) _queue).SyncRoot;

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _queue).GetEnumerator();
        }

        int IReadOnlyCollection<T>.Count => _queue.Count;

        public void OnBeforeSerialize()
        {
            using (_PRF_OnBeforeSerialize.Auto())
            {
                var length = _queue.Count;

                _array = new T[length];

                for (var i = 0; i < length; i++)
                {
                    _array[i] = _queue.Dequeue();
                }
            }
        }

        public void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                _queue = new Queue<T>();

                for (var i = 0; i < _array.Length; i++)
                {
                    _queue.Enqueue(_array[i]);
                }

                _array = null;
            }
        }

        private void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                if (_queue == null)
                {
                    _queue = new Queue<T>();
                }
            }
        }

        public virtual void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                Initialize();
                _queue.Clear();
            }
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            using (_PRF_CopyTo.Auto())
            {
                _queue.CopyTo(array, arrayIndex);
            }
        }

        public virtual void Enqueue(T item)
        {
            using (_PRF_Enqueue.Auto())
            {
                Initialize();
                _queue.Enqueue(item);
            }
        }

        public virtual T Dequeue()
        {
            using (_PRF_Dequeue.Auto())
            {
                Initialize();
                return _queue.Dequeue();
            }
        }

        public virtual T Peek()
        {
            using (_PRF_Peek.Auto())
            {
                Initialize();
                return _queue.Peek();
            }
        }

        public virtual bool Contains(T item)
        {
            using (_PRF_Contains.Auto())
            {
                Initialize();
                return _queue.Contains(item);
            }
        }

        public virtual T[] ToArray()
        {
            using (_PRF_ToArray.Auto())
            {
                return _queue.ToArray();
            }
        }

        public virtual void TrimExcess()
        {
            using (_PRF_TrimExcess.Auto())
            {
                _queue.TrimExcess();
            }
        }
    }
}
