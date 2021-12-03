#region

using System;
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

        public OverridableFloat4x4(bool overrideEnabled, float4x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableFloat4x4(Overridable<float4x4, OverridableFloat4x4> value) : base(value)
        {
        }
    }
}
