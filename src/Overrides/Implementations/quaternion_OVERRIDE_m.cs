#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class quaternion_OVERRIDE_m : Overridable<quaternion, quaternion_OVERRIDE_m>
    {
        public quaternion_OVERRIDE_m() : base(false, default)
        {
        }

        public quaternion_OVERRIDE_m(
            bool isOverridingAllowed,
            bool overrideEnabled,
            quaternion value) : base(overrideEnabled, value)
        {
        }

        public quaternion_OVERRIDE_m(Overridable<quaternion, quaternion_OVERRIDE_m> value) : base(
            value
        )
        {
        }
    }
}
