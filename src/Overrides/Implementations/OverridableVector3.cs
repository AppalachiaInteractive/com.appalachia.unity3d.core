#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableVector3(bool overriding, Vector3 value) : base(overriding, value)
        {
        }

        public OverridableVector3(Overridable<Vector3, OverridableVector3> value) : base(value)
        {
        }
    }
}
