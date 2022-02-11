#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf2 : Overridable<half2, OverridableHalf2>
    {
        public OverridableHalf2() : base(false, default)
        {
        }

        public OverridableHalf2(bool overriding, half2 value) : base(overriding, value)
        {
        }

        public OverridableHalf2(Overridable<half2, OverridableHalf2> value) : base(value)
        {
        }
    }
}
