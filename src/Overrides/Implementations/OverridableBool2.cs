#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool2 : Overridable<bool2, OverridableBool2>
    {
        public OverridableBool2() : base(false, default)
        {
        }

        public OverridableBool2(bool overriding, bool2 value) : base(overriding, value)
        {
        }

        public OverridableBool2(Overridable<bool2, OverridableBool2> value) : base(value)
        {
        }
    }
}
