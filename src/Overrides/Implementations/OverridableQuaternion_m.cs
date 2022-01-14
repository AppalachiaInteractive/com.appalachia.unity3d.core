#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableQuaternion_m : Overridable<quaternion, OverridableQuaternion_m>
    {
        public OverridableQuaternion_m() : base(false, default)
        {
        }

        public OverridableQuaternion_m(bool overrideEnabled, quaternion value) : base(overrideEnabled, value)
        {
        }

        public OverridableQuaternion_m(Overridable<quaternion, OverridableQuaternion_m> value) : base(value)
        {
        }
    }
}
