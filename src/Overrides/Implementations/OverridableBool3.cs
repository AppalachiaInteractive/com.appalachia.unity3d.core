#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool3 : Overridable<bool3, OverridableBool3>
    {
        public OverridableBool3() : base(false, default)
        {
        }

        public OverridableBool3(bool overriding, bool3 value) : base(overriding, value)
        {
        }

        public OverridableBool3(Overridable<bool3, OverridableBool3> value) : base(value)
        {
        }
    }
}
