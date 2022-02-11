#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool2x2 : Overridable<bool2x2, OverridableBool2x2>
    {
        public OverridableBool2x2() : base(false, default)
        {
        }

        public OverridableBool2x2(bool overriding, bool2x2 value) : base(overriding, value)
        {
        }

        public OverridableBool2x2(Overridable<bool2x2, OverridableBool2x2> value) : base(value)
        {
        }
    }
}
