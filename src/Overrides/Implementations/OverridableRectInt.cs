#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableRectInt : Overridable<RectInt, OverridableRectInt>
    {
        public OverridableRectInt() : base(false, default)
        {
        }

        public OverridableRectInt(bool overrideEnabled, RectInt value) : base(overrideEnabled, value)
        {
        }

        public OverridableRectInt(Overridable<RectInt, OverridableRectInt> value) : base(value)
        {
        }
    }
}
