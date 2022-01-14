#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat2x2 : Overridable<float2x2, OverridableFloat2x2>
    {
        public OverridableFloat2x2() : base(false, default)
        {
        }

        public OverridableFloat2x2(bool overrideEnabled, float2x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableFloat2x2(Overridable<float2x2, OverridableFloat2x2> value) : base(value)
        {
        }
    }
}
