#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector3 : Overridable<Vector3, OverridableVector3>
    {
        public OverridableVector3() : base(false, default)
        {
        }

        public OverridableVector3(bool overrideEnabled, Vector3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableVector3(Overridable<Vector3, OverridableVector3> value) : base(value)
        {
        }
    }
}
