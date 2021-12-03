#region

using System;
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

        public OverridableFloat4x3(bool overrideEnabled, float4x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableFloat4x3(Overridable<float4x3, OverridableFloat4x3> value) : base(value)
        {
        }
    }
}
