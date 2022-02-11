#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4x2 : Overridable<double4x2, OverridableDouble4x2>
    {
        public OverridableDouble4x2() : base(false, default)
        {
        }

        public OverridableDouble4x2(bool overriding, double4x2 value) : base(overriding, value)
        {
        }

        public OverridableDouble4x2(Overridable<double4x2, OverridableDouble4x2> value) : base(value)
        {
        }
    }
}
