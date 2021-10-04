#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float4_OVERRIDE : Overridable<float4, float4_OVERRIDE>
    {
        public float4_OVERRIDE() : base(false, default)
        {
        }

        public float4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public float4_OVERRIDE(Overridable<float4, float4_OVERRIDE> value) : base(value)
        {
        }
    }
}
