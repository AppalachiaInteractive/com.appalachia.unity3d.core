#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble2x4 : Overridable<double2x4, OverridableDouble2x4>
    {
        public OverridableDouble2x4() : base(false, default)
        {
        }

        public OverridableDouble2x4(bool overriding, double2x4 value) : base(overriding, value)
        {
        }

        public OverridableDouble2x4(Overridable<double2x4, OverridableDouble2x4> value) : base(value)
        {
        }
    }
}
