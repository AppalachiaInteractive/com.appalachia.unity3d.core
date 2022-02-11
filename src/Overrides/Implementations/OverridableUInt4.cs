#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4 : Overridable<uint4, OverridableUInt4>
    {
        public OverridableUInt4() : base(false, default)
        {
        }

        public OverridableUInt4(bool overriding, uint4 value) : base(overriding, value)
        {
        }

        public OverridableUInt4(Overridable<uint4, OverridableUInt4> value) : base(value)
        {
        }
    }
}
