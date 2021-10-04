#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class float2_OVERRIDE : Overridable<float2, float2_OVERRIDE>
    {
        public float2_OVERRIDE() : base(false, default)
        {
        }

        public float2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, float2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public float2_OVERRIDE(Overridable<float2, float2_OVERRIDE> value) : base(value)
        {
        }
    }
}
