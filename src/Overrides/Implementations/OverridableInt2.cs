#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2 : Overridable<int2, OverridableInt2>
    {
        public OverridableInt2() : base(false, default)
        {
        }

        public OverridableInt2(bool overriding, int2 value) : base(overriding, value)
        {
        }

        public OverridableInt2(Overridable<int2, OverridableInt2> value) : base(value)
        {
        }
    }
}
