#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool4 : Overridable<bool4, OverridableBool4>
    {
        public OverridableBool4() : base(false, default)
        {
        }

        public OverridableBool4(bool overrideEnabled, bool4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableBool4(Overridable<bool4, OverridableBool4> value) : base(value)
        {
        }
    }
}
