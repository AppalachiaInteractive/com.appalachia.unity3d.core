#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat3x4 : Overridable<float3x4, OverridableFloat3x4>
    {
        public OverridableFloat3x4() : base(false, default)
        {
        }

        public OverridableFloat3x4(bool overriding, float3x4 value) : base(overriding, value)
        {
        }

        public OverridableFloat3x4(Overridable<float3x4, OverridableFloat3x4> value) : base(value)
        {
        }
    }
}
