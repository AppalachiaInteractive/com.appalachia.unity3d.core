#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class float4x4_OVERRIDE : Overridable<float4x4, float4x4_OVERRIDE>
    { public float4x4_OVERRIDE() : base(false, default){}
        public float4x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float4x4 value) : base(overrideEnabled, value)
        {
        }

        public float4x4_OVERRIDE(Overridable<float4x4, float4x4_OVERRIDE> value) : base(value)
        {
        }
    }
}