#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat4 : Overridable<float4, OverridableFloat4>
    {
        public OverridableFloat4() : base(false, default)
        {
        }

        public OverridableFloat4(bool overriding, float4 value) : base(overriding, value)
        {
        }

        public OverridableFloat4(Overridable<float4, OverridableFloat4> value) : base(value)
        {
        }
    }
}
