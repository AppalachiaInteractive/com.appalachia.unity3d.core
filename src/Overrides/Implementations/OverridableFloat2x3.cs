#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat2x3 : Overridable<float2x3, OverridableFloat2x3>
    {
        public OverridableFloat2x3() : base(false, default)
        {
        }

        public OverridableFloat2x3(bool overriding, float2x3 value) : base(overriding, value)
        {
        }

        public OverridableFloat2x3(Overridable<float2x3, OverridableFloat2x3> value) : base(value)
        {
        }
    }
}
