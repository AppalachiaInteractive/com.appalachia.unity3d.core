#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf4 : Overridable<half4, OverridableHalf4>
    {
        public OverridableHalf4() : base(false, default)
        {
        }

        public OverridableHalf4(bool overrideEnabled, half4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableHalf4(Overridable<half4, OverridableHalf4> value) : base(value)
        {
        }
    }
}
