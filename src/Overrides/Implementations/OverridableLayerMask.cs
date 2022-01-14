#region

using System;
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

        public OverridableLayerMask(bool overrideEnabled, LayerMask value) : base(overrideEnabled, value)
        {
        }

        public OverridableLayerMask(Overridable<LayerMask, OverridableLayerMask> value) : base(value)
        {
        }
    }
}
