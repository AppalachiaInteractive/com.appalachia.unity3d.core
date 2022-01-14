#region

using System;
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

        public OverridableFloat4(bool overrideEnabled, float4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableFloat4(Overridable<float4, OverridableFloat4> value) : base(value)
        {
        }
    }
}
