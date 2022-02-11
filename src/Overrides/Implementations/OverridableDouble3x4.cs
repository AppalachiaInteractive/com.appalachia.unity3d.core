#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble3x4 : Overridable<double3x4, OverridableDouble3x4>
    {
        public OverridableDouble3x4() : base(false, default)
        {
        }

        public OverridableDouble3x4(bool overriding, double3x4 value) : base(overriding, value)
        {
        }

        public OverridableDouble3x4(Overridable<double3x4, OverridableDouble3x4> value) : base(value)
        {
        }
    }
}
