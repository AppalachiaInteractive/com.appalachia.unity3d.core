#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4x2 : Overridable<uint4x2, OverridableUInt4x2>
    {
        public OverridableUInt4x2() : base(false, default)
        {
        }

        public OverridableUInt4x2(bool overriding, uint4x2 value) : base(overriding, value)
        {
        }

        public OverridableUInt4x2(Overridable<uint4x2, OverridableUInt4x2> value) : base(value)
        {
        }
    }
}
