#region

using System;
using Appalachia.Core.Editing;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class LayerSelection_OVERRIDE : Overridable<LayerSelection, LayerSelection_OVERRIDE>
    { public LayerSelection_OVERRIDE() : base(false, default){}
        public LayerSelection_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, LayerSelection value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public LayerSelection_OVERRIDE(Overridable<LayerSelection, LayerSelection_OVERRIDE> value) : base(value)
        {
        }
    }
}
