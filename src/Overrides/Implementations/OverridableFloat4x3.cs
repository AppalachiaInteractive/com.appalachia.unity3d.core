#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat4x3 : Overridable<float4x3, OverridableFloat4x3>
    {
        public OverridableFloat4x3() : base(false, default)
        {
        }

        public OverridableFloat4x3(bool overriding, float4x3 value) : base(overriding, value)
        {
        }

        public OverridableFloat4x3(Overridable<float4x3, OverridableFloat4x3> value) : base(value)
        {
        }
    }
}
