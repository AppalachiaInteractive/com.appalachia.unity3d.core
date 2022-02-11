#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt4 : Overridable<int4, OverridableInt4>
    {
        public OverridableInt4() : base(false, default)
        {
        }

        public OverridableInt4(bool overriding, int4 value) : base(overriding, value)
        {
        }

        public OverridableInt4(Overridable<int4, OverridableInt4> value) : base(value)
        {
        }
    }
}
