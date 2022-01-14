#region

using System;
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

        public OverridablePlane(bool overrideEnabled, Plane value) : base(overrideEnabled, value)
        {
        }

        public OverridablePlane(Overridable<Plane, OverridablePlane> value) : base(value)
        {
        }
    }
}
