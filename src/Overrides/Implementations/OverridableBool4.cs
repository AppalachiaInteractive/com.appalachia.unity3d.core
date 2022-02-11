#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBool4(bool overriding, bool4 value) : base(overriding, value)
        {
        }

        public OverridableBool4(Overridable<bool4, OverridableBool4> value) : base(value)
        {
        }
    }
}
