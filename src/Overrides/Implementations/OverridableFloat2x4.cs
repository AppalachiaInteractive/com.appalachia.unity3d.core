#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat2x4 : Overridable<float2x4, OverridableFloat2x4>
    {
        public OverridableFloat2x4() : base(false, default)
        {
        }

        public OverridableFloat2x4(bool overrideEnabled, float2x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableFloat2x4(Overridable<float2x4, OverridableFloat2x4> value) : base(value)
        {
        }
    }
}
