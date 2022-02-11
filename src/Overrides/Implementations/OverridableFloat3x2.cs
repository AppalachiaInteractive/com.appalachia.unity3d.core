#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat3x2 : Overridable<float3x2, OverridableFloat3x2>
    {
        public OverridableFloat3x2() : base(false, default)
        {
        }

        public OverridableFloat3x2(bool overriding, float3x2 value) : base(overriding, value)
        {
        }

        public OverridableFloat3x2(Overridable<float3x2, OverridableFloat3x2> value) : base(value)
        {
        }
    }
}
