#region

using System;
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

        public OverridableHalf3(bool overrideEnabled, half3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableHalf3(Overridable<half3, OverridableHalf3> value) : base(value)
        {
        }
    }
}
