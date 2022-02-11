using System;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Framing;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBehaviour : MonoBehaviour
    {
        #region Fields and Autoproperties

        [NonSerialized] protected Bounds _renderingBounds;
        [NonSerialized] protected Transform _cachedTransform;
        [NonSerialized] protected RectTransform _cachedRectTransform;

        [NonSerialized] private bool _hasCachedTransform;
        [NonSerialized] private bool _hasCachedRectTransform;

        #endregion

        public bool HasRectTransform
        {
            get
            {
                using (_PRF_HasRectTransform.Auto())
                {
                    return Transform is RectTransform;
                }
            }
        }

        public Bounds RenderingBounds
        {
            get
            {
                using (_PRF_RenderingBounds.Auto())
                {
                    if (_renderingBounds == default)
                    {
                        _renderingBounds = gameObject.GetRenderingBounds(out _);
                    }

                    return _renderingBounds;
                }
            }
        }

        public RectTransform RectTransform
        {
            get
            {
                using (_PRF_RectTransform.Auto())
                {
                    if (_hasCachedRectTransform)
                    {
                        return _cachedRectTransform;
                    }

                    if (_cachedRectTransform == null)
                    {
                        if (Transform is RectTransform transformCast)
                        {
                            _cachedRectTransform = transformCast;
                            _hasCachedRectTransform = true;
                        }
                        else
                        {
                            gameObject.GetOrAddComponent(ref _cachedRectTransform);
                            _hasCachedRectTransform = true;
                        }
                    }

                    return _cachedRectTransform;
                }
            }
        }

        public Transform Transform
        {
            get
            {
                using (_PRF_Transform.Auto())
                {
                    if (_hasCachedTransform)
                    {
                        return _cachedTransform;
                    }

                    if (_cachedTransform == null)
                    {
                        _cachedTransform = transform;
                        _hasCachedTransform = true;
                    }

                    return _cachedTransform;
                }
            }
        }

        protected void DontDestroyOnLoadSafe()
        {
            using (_PRF_DontDestroyOnLoadSafe.Auto())
            {
                if (AppalachiaApplication.IsPlaying)
                {
                    DontDestroyOnLoad(gameObject.transform.root);
                }
            }
        }

        protected float3 LocalToWorldDirection(float3 direction)
        {
            using (_PRF_LocalToWorldDirection.Auto())
            {
                return Transform.TransformDirection(direction);
            }
        }

        protected float3 LocalToWorldPoint(float3 point)
        {
            using (_PRF_LocalToWorldPoint.Auto())
            {
                return Transform.TransformPoint(point);
            }
        }

        protected float3 LocalToWorldVector(float3 vector)
        {
            using (_PRF_LocalToWorldVector.Auto())
            {
                return Transform.TransformVector(vector);
            }
        }

        protected void RecalculateBounds()
        {
            using (_PRF_RecalculateBounds.Auto())
            {
                _renderingBounds = default;
            }
        }

        protected float3 WorldToLocalDirection(float3 direction)
        {
            using (_PRF_WorldToLocalDirection.Auto())
            {
                return Transform.InverseTransformDirection(direction);
            }
        }

        protected float3 WorldToLocalPoint(float3 point)
        {
            using (_PRF_WorldToLocalPoint.Auto())
            {
                return Transform.InverseTransformPoint(point);
            }
        }

        protected float3 WorldToLocalVector(float3 vector)
        {
            using (_PRF_WorldToLocalVector.Auto())
            {
                return Transform.InverseTransformVector(vector);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaBehaviour) + ".";

        private static readonly ProfilerMarker _PRF_RenderingBounds =
            new ProfilerMarker(_PRF_PFX + nameof(RenderingBounds));

        private static readonly ProfilerMarker _PRF_HasRectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(HasRectTransform));

        private static readonly ProfilerMarker _PRF_Transform =
            new ProfilerMarker(_PRF_PFX + nameof(Transform));

        private static readonly ProfilerMarker _PRF_RectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(RectTransform));

        private static readonly ProfilerMarker _PRF_RecalculateBounds =
            new ProfilerMarker(_PRF_PFX + nameof(RecalculateBounds));

        private static readonly ProfilerMarker _PRF_DontDestroyOnLoadSafe =
            new ProfilerMarker(_PRF_PFX + nameof(DontDestroyOnLoadSafe));

        private static readonly ProfilerMarker _PRF_LocalToWorldDirection =
            new ProfilerMarker(_PRF_PFX + nameof(LocalToWorldDirection));

        private static readonly ProfilerMarker _PRF_LocalToWorldPoint =
            new ProfilerMarker(_PRF_PFX + nameof(LocalToWorldPoint));

        private static readonly ProfilerMarker _PRF_LocalToWorldVector =
            new ProfilerMarker(_PRF_PFX + nameof(LocalToWorldVector));

        private static readonly ProfilerMarker _PRF_WorldToLocalDirection =
            new ProfilerMarker(_PRF_PFX + nameof(WorldToLocalDirection));

        private static readonly ProfilerMarker _PRF_WorldToLocalPoint =
            new ProfilerMarker(_PRF_PFX + nameof(WorldToLocalPoint));

        private static readonly ProfilerMarker _PRF_WorldToLocalVector =
            new ProfilerMarker(_PRF_PFX + nameof(WorldToLocalVector));

        #endregion
    }

    public abstract partial class AppalachiaBehaviour<T> : AppalachiaBehaviour
        where T : AppalachiaBehaviour<T>
    {
        #region Profiling

        protected static readonly string _PRF_PFX = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_AwakeActual =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeActual));

        protected static readonly ProfilerMarker _PRF_FixedUpdate =
            new ProfilerMarker(_PRF_PFX + "FixedUpdate");

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker
            _PRF_LateUpdate = new ProfilerMarker(_PRF_PFX + "LateUpdate");

        protected static readonly ProfilerMarker _PRF_OnDestroyActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnDestroyActual));

        protected static readonly ProfilerMarker _PRF_OnDisableActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisableActual));

        protected static readonly ProfilerMarker _PRF_OnDrawGizmos =
            new ProfilerMarker(_PRF_PFX + "OnDrawGizmos");

        protected static readonly ProfilerMarker _PRF_OnDrawGizmosSelected =
            new ProfilerMarker(_PRF_PFX + "OnDrawGizmosSelected");

        protected static readonly ProfilerMarker _PRF_OnEnableActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnEnableActual));

        protected static readonly ProfilerMarker _PRF_ResetActual =
            new ProfilerMarker(_PRF_PFX + nameof(ResetActual));

        protected static readonly ProfilerMarker _PRF_StartActual =
            new ProfilerMarker(_PRF_PFX + nameof(StartActual));

        protected static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + "Update");

        protected static readonly ProfilerMarker _PRF_WhenDestroyed =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDestroyed));

        protected static readonly ProfilerMarker _PRF_WhenDisabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDisabled));

        protected static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        #endregion
    }
}
