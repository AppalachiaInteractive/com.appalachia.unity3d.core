#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat3x3 : Overridable<float3x3, OverridableFloat3x3>
    {
        public OverridableFloat3x3() : base(false, default)
        {
        }

        public OverridableFloat3x3(bool overrideEnabled, float3x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableFloat3x3(Overridable<float3x3, OverridableFloat3x3> value) : base(value)
        {
        }
    }
}
