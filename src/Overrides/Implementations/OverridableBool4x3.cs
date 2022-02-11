#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBool4x3(bool overriding, bool4x3 value) : base(overriding, value)
        {
        }

        public OverridableBool4x3(Overridable<bool4x3, OverridableBool4x3> value) : base(value)
        {
        }
    }
}
