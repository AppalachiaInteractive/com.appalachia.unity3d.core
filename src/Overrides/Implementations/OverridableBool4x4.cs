#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool4x4 : Overridable<bool4x4, OverridableBool4x4>
    {
        public OverridableBool4x4() : base(false, default)
        {
        }

        public OverridableBool4x4(bool overrideEnabled, bool4x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool4x4(Overridable<bool4x4, OverridableBool4x4> value) : base(value)
        {
        }
    }
}
