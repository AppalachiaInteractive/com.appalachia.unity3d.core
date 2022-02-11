#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble3 : Overridable<double3, OverridableDouble3>
    {
        public OverridableDouble3() : base(false, default)
        {
        }

        public OverridableDouble3(bool overriding, double3 value) : base(overriding, value)
        {
        }

        public OverridableDouble3(Overridable<double3, OverridableDouble3> value) : base(value)
        {
        }
    }
}
