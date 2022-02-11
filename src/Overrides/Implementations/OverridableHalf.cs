#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableHalf : Overridable<half, OverridableHalf>
    {
        public OverridableHalf() : base(false, default)
        {
        }

        public OverridableHalf(bool overriding, half value) : base(overriding, value)
        {
        }

        public OverridableHalf(Overridable<half, OverridableHalf> value) : base(value)
        {
        }
    }
}
