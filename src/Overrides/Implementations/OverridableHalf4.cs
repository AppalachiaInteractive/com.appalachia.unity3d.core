#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf4 : Overridable<half4, OverridableHalf4>
    {
        public OverridableHalf4() : base(false, default)
        {
        }

        public OverridableHalf4(bool overriding, half4 value) : base(overriding, value)
        {
        }

        public OverridableHalf4(Overridable<half4, OverridableHalf4> value) : base(value)
        {
        }
    }
}
