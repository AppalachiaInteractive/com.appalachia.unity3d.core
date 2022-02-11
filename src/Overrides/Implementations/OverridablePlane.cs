#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridablePlane : Overridable<Plane, OverridablePlane>
    {
        public OverridablePlane() : base(false, default)
        {
        }

        public OverridablePlane(bool overriding, Plane value) : base(overriding, value)
        {
        }

        public OverridablePlane(Overridable<Plane, OverridablePlane> value) : base(value)
        {
        }
    }
}
