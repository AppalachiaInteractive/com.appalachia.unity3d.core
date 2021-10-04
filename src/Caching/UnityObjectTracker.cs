/*
#region

using Appalachia.Core.Base;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Caching
{
    public sealed class UnityObjectTracker : InternalBase<UnityObjectTracker>
    {
        private const string _PRF_PFX = nameof(UnityObjectTracker) + ".";
        private readonly System.Func<Object> _o;
        private readonly Cached<bool> _isAlive;
        private readonly Cached<bool> _isEnabled;

        private static readonly ProfilerMarker _PRF_UnityObjectTracker = new ProfilerMarker(_PRF_PFX + nameof(UnityObjectTracker));
        public UnityObjectTracker(System.Func<Object> o, double millisecondsToCache)
        {
            using (_PRF_UnityObjectTracker.Auto())
            {
                _o = o;
                _isAlive = Cached.InvalidateByTime(IsNativeObjectAlive,     millisecondsToCache);
                _isEnabled = Cached.InvalidateByTime(IsNativeObjectEnabled, millisecondsToCache);
            }
        }

        private static readonly ProfilerMarker _PRF_IsNull = new ProfilerMarker(_PRF_PFX + nameof(IsNull));
        public bool IsNull
        {
            get
            {
                using (_PRF_IsNull.Auto())
                {
                    return !IsAlive;
                }
            }
        }

        private static readonly ProfilerMarker _PRF_IsNotNull = new ProfilerMarker(_PRF_PFX + nameof(IsNotNull));
        public bool IsNotNull
        {
            get
            {
                using (_PRF_IsNotNull.Auto())
                {
                    return IsAlive;
                }
            }
        }

        private static readonly ProfilerMarker _PRF_IsAlive = new ProfilerMarker(_PRF_PFX + nameof(IsAlive));
        public bool IsAlive
        {
            get
            {
                using (_PRF_IsAlive.Auto())
                {
                    return _isAlive.Get();
                }
            }
        }

        private static readonly ProfilerMarker _PRF_IsEnabled = new ProfilerMarker(_PRF_PFX + nameof(IsEnabled));
        public bool IsEnabled
        {
            get
            {
                using (_PRF_IsEnabled.Auto())
                {
                    return _isEnabled.Get();
                }
            }
        }

        private static readonly ProfilerMarker _PRF_NotReadyToTrack = new ProfilerMarker(_PRF_PFX + nameof(NotReadyToTrack));
        public bool NotReadyToTrack
        {
            get
            {
                using (_PRF_NotReadyToTrack.Auto())
                {
                    return (_isAlive == null) || (_isEnabled == null);
                }
            }
        }

        private static readonly ProfilerMarker _PRF_IsNativeObjectAlive = new ProfilerMarker(_PRF_PFX + nameof(IsNativeObjectAlive));
        private bool IsNativeObjectAlive()
        {
            using (_PRF_IsNativeObjectAlive.Auto())
            {
                return _o() != null;
            }
        }

        private static readonly ProfilerMarker _PRF_IsNativeObjectEnabled = new ProfilerMarker(_PRF_PFX + nameof(IsNativeObjectEnabled));
        private bool IsNativeObjectEnabled()
        {
            using (_PRF_IsNativeObjectEnabled.Auto())
            {
                var o = _o();
                if (o == null)
                {
                    return false;
                }

                if (o is Behaviour b)
                {
                    return b.enabled;
                }

                if (o is Component c)
                {
                    if (o is Collider oc)
                    {
                        return oc.enabled;
                    }

                    if (o is LODGroup l)
                    {
                        return l.enabled;
                    }

                    if (o is Renderer r)
                    {
                        return r.enabled;
                    }
                }

                return false;
            }
        }

        private static readonly ProfilerMarker _PRF_Refresh = new ProfilerMarker(_PRF_PFX + nameof(Refresh));
        public void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                _isAlive.Invalidate();
                _isEnabled.Invalidate();
            }
        }
    }
}
*/


