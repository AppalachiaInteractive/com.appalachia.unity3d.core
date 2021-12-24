using System;
using Appalachia.Utility.Execution;
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

        [NonSerialized] private Bounds ___renderingBounds;
        [NonSerialized] private Transform ___transform;

        #endregion

        public Bounds renderingBounds
        {
            get
            {
                if (___renderingBounds == default)
                {
                    ___renderingBounds = gameObject.GetRenderingBounds(out _);
                }

                return ___renderingBounds;
            }
        }

        protected Transform _transform
        {
            get
            {
                if (___transform == null)
                {
                    ___transform = transform;
                }

                return ___transform;
            }
        }

        protected void DontDestroyOnLoadSafe()
        {
            using (_PRF_DontDestroyOnLoadSafe.Auto())
            {
                if (AppalachiaApplication.IsPlaying && (transform.parent == null))
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        protected float3 LocalToWorldDirection(float3 direction)
        {
            using (_PRF_LocalToWorldDirection.Auto())
            {
                return _transform.TransformDirection(direction);
            }
        }

        protected float3 LocalToWorldPoint(float3 point)
        {
            using (_PRF_LocalToWorldPoint.Auto())
            {
                return _transform.TransformPoint(point);
            }
        }

        protected float3 LocalToWorldVector(float3 vector)
        {
            using (_PRF_LocalToWorldVector.Auto())
            {
                return _transform.TransformVector(vector);
            }
        }

        protected void RecalculateBounds()
        {
            using (_PRF_RecalculateBounds.Auto())
            {
                ___renderingBounds = default;
            }
        }

        protected float3 WorldToLocalDirection(float3 direction)
        {
            using (_PRF_WorldToLocalDirection.Auto())
            {
                return _transform.InverseTransformDirection(direction);
            }
        }

        protected float3 WorldToLocalPoint(float3 point)
        {
            using (_PRF_WorldToLocalPoint.Auto())
            {
                return _transform.InverseTransformPoint(point);
            }
        }

        protected float3 WorldToLocalVector(float3 vector)
        {
            using (_PRF_WorldToLocalVector.Auto())
            {
                return _transform.InverseTransformVector(vector);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaBehaviour) + ".";

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

        private const string _PRF_PFX = nameof(AppalachiaBehaviour<T>) + ".";

        #endregion
    }
}
