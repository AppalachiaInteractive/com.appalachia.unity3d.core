#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool3x2 : Overridable<bool3x2, OverridableBool3x2>
    {
        public OverridableBool3x2() : base(false, default)
        {
        }

        public OverridableBool3x2(bool overrideEnabled, bool3x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableBool3x2(Overridable<bool3x2, OverridableBool3x2> value) : base(value)
        {
        }
    }
}
