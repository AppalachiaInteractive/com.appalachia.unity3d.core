#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableQuaternion : Overridable<Quaternion, OverridableQuaternion>
    {
        public OverridableQuaternion() : base(false, default)
        {
        }

        public OverridableQuaternion(bool overrideEnabled, Quaternion value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableQuaternion(Overridable<Quaternion, OverridableQuaternion> value) : base(value)
        {
        }
    }
}
