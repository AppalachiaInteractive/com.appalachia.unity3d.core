#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4 : Overridable<double4, OverridableDouble4>
    {
        public OverridableDouble4() : base(false, default)
        {
        }

        public OverridableDouble4(bool overriding, double4 value) : base(overriding, value)
        {
        }

        public OverridableDouble4(Overridable<double4, OverridableDouble4> value) : base(value)
        {
        }
    }
}
