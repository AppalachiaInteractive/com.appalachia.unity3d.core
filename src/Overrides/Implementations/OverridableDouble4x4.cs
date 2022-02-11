#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4x4 : Overridable<double4x4, OverridableDouble4x4>
    {
        public OverridableDouble4x4() : base(false, default)
        {
        }

        public OverridableDouble4x4(bool overriding, double4x4 value) : base(overriding, value)
        {
        }

        public OverridableDouble4x4(Overridable<double4x4, OverridableDouble4x4> value) : base(value)
        {
        }
    }
}
