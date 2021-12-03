#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat2 : Overridable<float2, OverridableFloat2>
    {
        public OverridableFloat2() : base(false, default)
        {
        }

        public OverridableFloat2(bool overrideEnabled, float2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableFloat2(Overridable<float2, OverridableFloat2> value) : base(value)
        {
        }
    }
}
