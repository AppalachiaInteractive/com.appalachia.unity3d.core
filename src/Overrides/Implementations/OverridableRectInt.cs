#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableRectInt(bool overriding, RectInt value) : base(overriding, value)
        {
        }

        public OverridableRectInt(Overridable<RectInt, OverridableRectInt> value) : base(value)
        {
        }
    }
}
