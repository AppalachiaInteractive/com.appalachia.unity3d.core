#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class float2x3_OVERRIDE : Overridable<float2x3, float2x3_OVERRIDE>
    { public float2x3_OVERRIDE() : base(false, default){}
        public float2x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float2x3 value) : base(overrideEnabled, value)
        {
        }

        public float2x3_OVERRIDE(Overridable<float2x3, float2x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
