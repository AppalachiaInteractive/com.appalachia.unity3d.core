#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableQuaternion(bool overriding, Quaternion value) : base(overriding, value)
        {
        }

        public OverridableQuaternion(Overridable<Quaternion, OverridableQuaternion> value) : base(value)
        {
        }
    }
}
