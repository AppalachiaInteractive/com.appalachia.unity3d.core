#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float3_OVERRIDE : Overridable<float3, float3_OVERRIDE>
    { public float3_OVERRIDE() : base(false, default){}
        public float3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float3 value) : base(overrideEnabled, value)
        {
        }

        public float3_OVERRIDE(Overridable<float3, float3_OVERRIDE> value) : base(value)
        {
        }
    }
}
