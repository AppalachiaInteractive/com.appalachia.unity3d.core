#region

using System;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    public class AppaTemporalQueue<T> : AppaQueue<T>
    {
        #region Fields and Autoproperties

        [SerializeField] private T _current;

        #endregion

        public bool HasCurrent => _current?.Equals(default) ?? false;

        public T Current => _current;

        /// <inheritdoc />
        public override T Dequeue()
        {
            using (_PRF_Dequeue.Auto())
            {
                _current = base.Dequeue();

                return _current;
            }
        }

        public T CurrentOrNext()
        {
            using (_PRF_CurrentOrNext.Auto())
            {
                if (_current != null)
                {
                    return _current;
                }

                if (Count == 0)
                {
                    return default;
                }

                return Dequeue();
            }
        }

        public void ResetCurrent()
        {
            using (_PRF_ResetCurrent.Auto())
            {
                _current = default;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppaTemporalQueue<T>) + ".";
        private static readonly ProfilerMarker _PRF_Dequeue = new(_PRF_PFX + nameof(Dequeue));
        private static readonly ProfilerMarker _PRF_ResetCurrent = new(_PRF_PFX + nameof(ResetCurrent));
        private static readonly ProfilerMarker _PRF_CurrentOrNext = new(_PRF_PFX + nameof(CurrentOrNext));

        #endregion
    }
}
