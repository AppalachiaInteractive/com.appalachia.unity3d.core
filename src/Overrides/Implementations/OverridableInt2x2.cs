#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2x2 : Overridable<int2x2, OverridableInt2x2>
    {
        public OverridableInt2x2() : base(false, default)
        {
        }

        public OverridableInt2x2(bool overriding, int2x2 value) : base(overriding, value)
        {
        }

        public OverridableInt2x2(Overridable<int2x2, OverridableInt2x2> value) : base(value)
        {
        }
    }
}
