#region

using System;
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

        public OverridableBool2(bool overrideEnabled, bool2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool2(Overridable<bool2, OverridableBool2> value) : base(value)
        {
        }
    }
}
