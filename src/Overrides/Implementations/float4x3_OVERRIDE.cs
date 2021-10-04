#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float4x3_OVERRIDE : Overridable<float4x3, float4x3_OVERRIDE>
    {
        public float4x3_OVERRIDE() : base(false, default)
        {
        }

        public float4x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float4x3 value) :
            base(overrideEnabled, value)
        {
        }

        public float4x3_OVERRIDE(Overridable<float4x3, float4x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
