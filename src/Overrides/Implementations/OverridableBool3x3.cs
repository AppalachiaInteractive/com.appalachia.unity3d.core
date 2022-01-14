#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool3x3 : Overridable<bool3x3, OverridableBool3x3>
    {
        public OverridableBool3x3() : base(false, default)
        {
        }

        public OverridableBool3x3(bool overrideEnabled, bool3x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool3x3(Overridable<bool3x3, OverridableBool3x3> value) : base(value)
        {
        }
    }
}
