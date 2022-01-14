#region

using System;
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

        public OverridableBool2x4(bool overrideEnabled, bool2x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool2x4(Overridable<bool2x4, OverridableBool2x4> value) : base(value)
        {
        }
    }
}
