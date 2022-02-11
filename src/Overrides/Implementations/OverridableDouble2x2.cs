#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble2x2 : Overridable<double2x2, OverridableDouble2x2>
    {
        public OverridableDouble2x2() : base(false, default)
        {
        }

        public OverridableDouble2x2(bool overriding, double2x2 value) : base(overriding, value)
        {
        }

        public OverridableDouble2x2(Overridable<double2x2, OverridableDouble2x2> value) : base(value)
        {
        }
    }
}
