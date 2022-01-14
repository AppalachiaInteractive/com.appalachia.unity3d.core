#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat4x2 : Overridable<float4x2, OverridableFloat4x2>
    {
        public OverridableFloat4x2() : base(false, default)
        {
        }

        public OverridableFloat4x2(bool overrideEnabled, float4x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableFloat4x2(Overridable<float4x2, OverridableFloat4x2> value) : base(value)
        {
        }
    }
}
