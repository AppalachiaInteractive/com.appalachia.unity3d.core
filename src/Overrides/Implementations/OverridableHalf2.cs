#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf2 : Overridable<half2, OverridableHalf2>
    {
        public OverridableHalf2() : base(false, default)
        {
        }

        public OverridableHalf2(bool overrideEnabled, half2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableHalf2(Overridable<half2, OverridableHalf2> value) : base(value)
        {
        }
    }
}
