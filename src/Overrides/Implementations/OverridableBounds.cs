#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBounds(bool overriding, Bounds value) : base(overriding, value)
        {
        }

        public OverridableBounds(Overridable<Bounds, OverridableBounds> value) : base(value)
        {
        }
    }
}
