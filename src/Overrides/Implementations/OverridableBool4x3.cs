#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool4x3 : Overridable<bool4x3, OverridableBool4x3>
    {
        public OverridableBool4x3() : base(false, default)
        {
        }

        public OverridableBool4x3(bool overrideEnabled, bool4x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool4x3(Overridable<bool4x3, OverridableBool4x3> value) : base(value)
        {
        }
    }
}
