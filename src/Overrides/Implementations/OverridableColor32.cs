#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableColor32 : Overridable<Color32, OverridableColor32>
    {
        public OverridableColor32() : base(false, default)
        {
        }

        public OverridableColor32(bool overriding, Color32 value) : base(overriding, value)
        {
        }

        public OverridableColor32(Overridable<Color32, OverridableColor32> value) : base(value)
        {
        }
    }
}
