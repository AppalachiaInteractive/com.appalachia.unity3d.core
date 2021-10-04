#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float2x4_OVERRIDE : Overridable<float2x4, float2x4_OVERRIDE>
    { public float2x4_OVERRIDE() : base(false, default){}
        public float2x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float2x4 value) : base(overrideEnabled, value)
        {
        }

        public float2x4_OVERRIDE(Overridable<float2x4, float2x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
