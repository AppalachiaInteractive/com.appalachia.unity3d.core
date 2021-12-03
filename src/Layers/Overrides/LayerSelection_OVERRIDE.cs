#region

using System;
using Appalachia.Core.Overrides;

#endregion

namespace Appalachia.Core.Layers.Overrides
{
    [Serializable]
    public sealed class OverridableLayerSelection : Overridable<LayerSelection, OverridableLayerSelection>
    {
        public OverridableLayerSelection() : base(false, default)
        {
        }

        public OverridableLayerSelection(
            bool isOverridingAllowed,
            bool overrideEnabled,
            LayerSelection value) : base(overrideEnabled, value)
        {
        }

        public OverridableLayerSelection(Overridable<LayerSelection, OverridableLayerSelection> value) : base(
            value
        )
        {
        }
    }
}
