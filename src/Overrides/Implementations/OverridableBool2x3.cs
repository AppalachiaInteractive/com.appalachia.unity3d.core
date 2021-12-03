#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool2x3 : Overridable<bool2x3, OverridableBool2x3>
    {
        public OverridableBool2x3() : base(false, default)
        {
        }

        public OverridableBool2x3(bool overrideEnabled, bool2x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableBool2x3(Overridable<bool2x3, OverridableBool2x3> value) : base(value)
        {
        }
    }
}
