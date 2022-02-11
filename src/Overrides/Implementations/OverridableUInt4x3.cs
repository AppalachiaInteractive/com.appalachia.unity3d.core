#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4x3 : Overridable<uint4x3, OverridableUInt4x3>
    {
        public OverridableUInt4x3() : base(false, default)
        {
        }

        public OverridableUInt4x3(bool overriding, uint4x3 value) : base(overriding, value)
        {
        }

        public OverridableUInt4x3(Overridable<uint4x3, OverridableUInt4x3> value) : base(value)
        {
        }
    }
}
