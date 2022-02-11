#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat3 : Overridable<float3, OverridableFloat3>
    {
        public OverridableFloat3() : base(false, default)
        {
        }

        public OverridableFloat3(bool overriding, float3 value) : base(overriding, value)
        {
        }

        public OverridableFloat3(Overridable<float3, OverridableFloat3> value) : base(value)
        {
        }
    }
}
