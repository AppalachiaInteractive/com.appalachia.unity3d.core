#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float2x2_OVERRIDE : Overridable<float2x2, float2x2_OVERRIDE>
    { public float2x2_OVERRIDE() : base(false, default){}
        public float2x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float2x2 value) : base(overrideEnabled, value)
        {
        }

        public float2x2_OVERRIDE(Overridable<float2x2, float2x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
