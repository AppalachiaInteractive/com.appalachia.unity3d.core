#region

using System;
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

        public OverridableBool3x4(bool overrideEnabled, bool3x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableBool3x4(Overridable<bool3x4, OverridableBool3x4> value) : base(value)
        {
        }
    }
}
