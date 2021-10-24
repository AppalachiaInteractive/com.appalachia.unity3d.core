#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class LayerMask_OVERRIDE : Overridable<LayerMask, LayerMask_OVERRIDE>
    {
        public LayerMask_OVERRIDE() : base(false, default)
        {
        }

        public LayerMask_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, LayerMask value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public LayerMask_OVERRIDE(Overridable<LayerMask, LayerMask_OVERRIDE> value) : base(value)
        {
        }
    }
}
