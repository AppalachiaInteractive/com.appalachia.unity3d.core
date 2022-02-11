#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble2 : Overridable<double2, OverridableDouble2>
    {
        public OverridableDouble2() : base(false, default)
        {
        }

        public OverridableDouble2(bool overriding, double2 value) : base(overriding, value)
        {
        }

        public OverridableDouble2(Overridable<double2, OverridableDouble2> value) : base(value)
        {
        }
    }
}
