#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float3x2_OVERRIDE : Overridable<float3x2, float3x2_OVERRIDE>
    {
        public float3x2_OVERRIDE() : base(false, default)
        {
        }

        public float3x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float3x2 value) :
            base(overrideEnabled, value)
        {
        }

        public float3x2_OVERRIDE(Overridable<float3x2, float3x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
