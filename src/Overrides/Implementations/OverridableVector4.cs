#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector4 : Overridable<Vector4, OverridableVector4>
    {
        public OverridableVector4() : base(false, default)
        {
        }

        public OverridableVector4(bool overriding, Vector4 value) : base(overriding, value)
        {
        }

        public OverridableVector4(Overridable<Vector4, OverridableVector4> value) : base(value)
        {
        }
    }
}
