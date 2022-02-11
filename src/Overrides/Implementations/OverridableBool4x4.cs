#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBool4x4(bool overriding, bool4x4 value) : base(overriding, value)
        {
        }

        public OverridableBool4x4(Overridable<bool4x4, OverridableBool4x4> value) : base(value)
        {
        }
    }
}
