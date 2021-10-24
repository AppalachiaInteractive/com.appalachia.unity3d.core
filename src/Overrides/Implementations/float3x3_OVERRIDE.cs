#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float3x3_OVERRIDE : Overridable<float3x3, float3x3_OVERRIDE>
    {
        public float3x3_OVERRIDE() : base(false, default)
        {
        }

        public float3x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float3x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public float3x3_OVERRIDE(Overridable<float3x3, float3x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
