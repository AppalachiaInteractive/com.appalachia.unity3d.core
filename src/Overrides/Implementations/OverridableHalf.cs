#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf : Overridable<half, OverridableHalf>
    {
        public OverridableHalf() : base(false, default)
        {
        }

        public OverridableHalf(bool overrideEnabled, half value) : base(overrideEnabled, value)
        {
        }

        public OverridableHalf(Overridable<half, OverridableHalf> value) : base(value)
        {
        }
    }
}
