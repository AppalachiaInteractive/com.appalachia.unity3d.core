#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableQuaternion_m(bool overriding, quaternion value) : base(overriding, value)
        {
        }

        public OverridableQuaternion_m(Overridable<quaternion, OverridableQuaternion_m> value) : base(value)
        {
        }
    }
}
