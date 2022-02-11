#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool3x4 : Overridable<bool3x4, OverridableBool3x4>
    {
        public OverridableBool3x4() : base(false, default)
        {
        }

        public OverridableBool3x4(bool overriding, bool3x4 value) : base(overriding, value)
        {
        }

        public OverridableBool3x4(Overridable<bool3x4, OverridableBool3x4> value) : base(value)
        {
        }
    }
}
