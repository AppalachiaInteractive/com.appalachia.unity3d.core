#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble3x3 : Overridable<double3x3, OverridableDouble3x3>
    {
        public OverridableDouble3x3() : base(false, default)
        {
        }

        public OverridableDouble3x3(bool overriding, double3x3 value) : base(overriding, value)
        {
        }

        public OverridableDouble3x3(Overridable<double3x3, OverridableDouble3x3> value) : base(value)
        {
        }
    }
}
