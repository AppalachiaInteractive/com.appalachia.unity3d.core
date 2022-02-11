#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool2x4 : Overridable<bool2x4, OverridableBool2x4>
    {
        public OverridableBool2x4() : base(false, default)
        {
        }

        public OverridableBool2x4(bool overriding, bool2x4 value) : base(overriding, value)
        {
        }

        public OverridableBool2x4(Overridable<bool2x4, OverridableBool2x4> value) : base(value)
        {
        }
    }
}
