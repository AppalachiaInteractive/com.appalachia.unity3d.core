#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat2x4 : Overridable<float2x4, OverridableFloat2x4>
    {
        public OverridableFloat2x4() : base(false, default)
        {
        }

        public OverridableFloat2x4(bool overriding, float2x4 value) : base(overriding, value)
        {
        }

        public OverridableFloat2x4(Overridable<float2x4, OverridableFloat2x4> value) : base(value)
        {
        }
    }
}
