#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector2 : Overridable<Vector2, OverridableVector2>
    {
        public OverridableVector2() : base(false, default)
        {
        }

        public OverridableVector2(bool overriding, Vector2 value) : base(overriding, value)
        {
        }

        public OverridableVector2(Overridable<Vector2, OverridableVector2> value) : base(value)
        {
        }
    }
}
