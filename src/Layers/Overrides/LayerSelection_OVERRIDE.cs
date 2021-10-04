#region

using System;
using Appalachia.Core.Overrides;

#endregion

namespace Appalachia.Core.Layers.Overrides
{
    [Serializable]
    public sealed class
        LayerSelection_OVERRIDE : Overridable<LayerSelection, LayerSelection_OVERRIDE>
    {
        public LayerSelection_OVERRIDE() : base(false, default)
        {
        }

        public LayerSelection_OVERRIDE(
            bool isOverridingAllowed,
            bool overrideEnabled,
            LayerSelection value) : base(overrideEnabled, value)
        {
        }

        public LayerSelection_OVERRIDE(
            Overridable<LayerSelection, LayerSelection_OVERRIDE> value) : base(value)
        {
        }
    }
}
