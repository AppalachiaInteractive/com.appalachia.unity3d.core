#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class Bounds_OVERRIDE : Overridable<Bounds, Bounds_OVERRIDE>
    { public Bounds_OVERRIDE() : base(false, default){}
        public Bounds_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Bounds value) : base(overrideEnabled, value)
        {
        }

        public Bounds_OVERRIDE(Overridable<Bounds, Bounds_OVERRIDE> value) : base(value)
        {
        }
    }
}
