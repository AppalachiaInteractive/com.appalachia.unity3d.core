#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Plane_OVERRIDE : Overridable<Plane, Plane_OVERRIDE>
    {
        public Plane_OVERRIDE() : base(false, default)
        {
        }

        public Plane_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Plane value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public Plane_OVERRIDE(Overridable<Plane, Plane_OVERRIDE> value) : base(value)
        {
        }
    }
}
