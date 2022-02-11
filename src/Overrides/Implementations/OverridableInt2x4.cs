#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2x4 : Overridable<int2x4, OverridableInt2x4>
    {
        public OverridableInt2x4() : base(false, default)
        {
        }

        public OverridableInt2x4(bool overriding, int2x4 value) : base(overriding, value)
        {
        }

        public OverridableInt2x4(Overridable<int2x4, OverridableInt2x4> value) : base(value)
        {
        }
    }
}
