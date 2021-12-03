#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBounds : Overridable<Bounds, OverridableBounds>
    {
        public OverridableBounds() : base(false, default)
        {
        }

        public OverridableBounds(bool overrideEnabled, Bounds value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableBounds(Overridable<Bounds, OverridableBounds> value) : base(value)
        {
        }
    }
}
