#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableLayerMask : Overridable<LayerMask, OverridableLayerMask>
    {
        public OverridableLayerMask() : base(false, default)
        {
        }

        public OverridableLayerMask(bool overriding, LayerMask value) : base(overriding, value)
        {
        }

        public OverridableLayerMask(Overridable<LayerMask, OverridableLayerMask> value) : base(value)
        {
        }
    }
}
