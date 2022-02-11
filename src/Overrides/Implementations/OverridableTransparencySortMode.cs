using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class
        OverridableTransparencySortMode : Overridable<TransparencySortMode, OverridableTransparencySortMode>
    {
        public OverridableTransparencySortMode(TransparencySortMode value) : base(false, value)
        {
        }

        public OverridableTransparencySortMode(bool overriding, TransparencySortMode value) : base(
            overriding,
            value
        )
        {
        }

        public OverridableTransparencySortMode(
            Overridable<TransparencySortMode, OverridableTransparencySortMode> value) : base(value)
        {
        }

        public OverridableTransparencySortMode() : base(false, default)
        {
        }
    }
}
