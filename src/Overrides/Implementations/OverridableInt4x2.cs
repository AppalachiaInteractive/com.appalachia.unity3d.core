#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt4x2 : Overridable<int4x2, OverridableInt4x2>
    {
        public OverridableInt4x2() : base(false, default)
        {
        }

        public OverridableInt4x2(bool overriding, int4x2 value) : base(overriding, value)
        {
        }

        public OverridableInt4x2(Overridable<int4x2, OverridableInt4x2> value) : base(value)
        {
        }
    }
}
