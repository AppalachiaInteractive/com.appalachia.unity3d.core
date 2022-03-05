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

        [NonSerialized] protected Bounds _renderingBounds;

        #endregion

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
}
