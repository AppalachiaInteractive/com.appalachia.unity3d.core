#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf3 : Overridable<half3, OverridableHalf3>
    {
        public OverridableHalf3() : base(false, default)
        {
        }

        public OverridableHalf3(bool overriding, half3 value) : base(overriding, value)
        {
        }

        public OverridableHalf3(Overridable<half3, OverridableHalf3> value) : base(value)
        {
        }
    }
}
