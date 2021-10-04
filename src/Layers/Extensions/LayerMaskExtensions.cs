#region

using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Layers.Extensions
{
    public static class LayerMaskExtensions
    {
        private static readonly ProfilerMarker _PRF_LayerMaskExtensions_IsLayerInMask = new ProfilerMarker("LayerMaskExtensions.IsLayerInMask");

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLayerInMask(this LayerMask layerMask, int layer)
        {
            using (_PRF_LayerMaskExtensions_IsLayerInMask.Auto())
            {
                return layerMask == (layerMask | (1 << layer));

            }
        }
    }
}
