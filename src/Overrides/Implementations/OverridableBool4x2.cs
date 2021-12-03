#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool4x2 : Overridable<bool4x2, OverridableBool4x2>
    {
        public OverridableBool4x2() : base(false, default)
        {
        }

        public OverridableBool4x2(bool overrideEnabled, bool4x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableBool4x2(Overridable<bool4x2, OverridableBool4x2> value) : base(value)
        {
        }
    }
}
