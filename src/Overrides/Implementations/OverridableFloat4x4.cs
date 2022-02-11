#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat4x4 : Overridable<float4x4, OverridableFloat4x4>
    {
        public OverridableFloat4x4() : base(false, default)
        {
        }

        public OverridableFloat4x4(bool overriding, float4x4 value) : base(overriding, value)
        {
        }

        public OverridableFloat4x4(Overridable<float4x4, OverridableFloat4x4> value) : base(value)
        {
        }
    }
}
