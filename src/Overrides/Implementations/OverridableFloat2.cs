#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat2 : Overridable<float2, OverridableFloat2>
    {
        public OverridableFloat2() : base(false, default)
        {
        }

        public OverridableFloat2(bool overriding, float2 value) : base(overriding, value)
        {
        }

        public OverridableFloat2(Overridable<float2, OverridableFloat2> value) : base(value)
        {
        }
    }
}
