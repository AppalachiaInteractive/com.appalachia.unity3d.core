#region

using System;
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

        public OverridableBool3(bool overrideEnabled, bool3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool3(Overridable<bool3, OverridableBool3> value) : base(value)
        {
        }
    }
}
