#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float3x4_OVERRIDE : Overridable<float3x4, float3x4_OVERRIDE>
    {
        public float3x4_OVERRIDE() : base(false, default)
        {
        }

        public float3x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float3x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public float3x4_OVERRIDE(Overridable<float3x4, float3x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
