#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt4x3 : Overridable<int4x3, OverridableInt4x3>
    {
        public OverridableInt4x3() : base(false, default)
        {
        }

        public OverridableInt4x3(bool overriding, int4x3 value) : base(overriding, value)
        {
        }

        public OverridableInt4x3(Overridable<int4x3, OverridableInt4x3> value) : base(value)
        {
        }
    }
}
