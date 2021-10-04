#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float4x2_OVERRIDE : Overridable<float4x2, float4x2_OVERRIDE>
    {
        public float4x2_OVERRIDE() : base(false, default)
        {
        }

        public float4x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float4x2 value) :
            base(overrideEnabled, value)
        {
        }

        public float4x2_OVERRIDE(Overridable<float4x2, float4x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
