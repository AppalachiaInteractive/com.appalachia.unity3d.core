using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.Rendering;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableOpaqueSortMode : Overridable<OpaqueSortMode, OverridableOpaqueSortMode>
    {
        public OverridableOpaqueSortMode(OpaqueSortMode value) : base(false, value)
        {
        }

        public OverridableOpaqueSortMode(bool overriding, OpaqueSortMode value) : base(overriding, value)
        {
        }

        public OverridableOpaqueSortMode(Overridable<OpaqueSortMode, OverridableOpaqueSortMode> value) : base(
            value
        )
        {
        }

        public OverridableOpaqueSortMode() : base(false, default)
        {
        }
    }
}
