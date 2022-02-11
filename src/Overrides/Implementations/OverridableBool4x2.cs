#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBool4x2(bool overriding, bool4x2 value) : base(overriding, value)
        {
        }

        public OverridableBool4x2(Overridable<bool4x2, OverridableBool4x2> value) : base(value)
        {
        }
    }
}
