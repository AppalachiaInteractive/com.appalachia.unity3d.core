#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector2Int : Overridable<Vector2Int, OverridableVector2Int>
    {
        public OverridableVector2Int() : base(false, default)
        {
        }

        public OverridableVector2Int(bool overriding, Vector2Int value) : base(overriding, value)
        {
        }

        public OverridableVector2Int(Overridable<Vector2Int, OverridableVector2Int> value) : base(value)
        {
        }
    }
}
