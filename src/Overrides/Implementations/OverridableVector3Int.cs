#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableVector3Int : Overridable<Vector3Int, OverridableVector3Int>
    {
        public OverridableVector3Int() : base(false, default)
        {
        }

        public OverridableVector3Int(bool overriding, Vector3Int value) : base(overriding, value)
        {
        }

        public OverridableVector3Int(Overridable<Vector3Int, OverridableVector3Int> value) : base(value)
        {
        }
    }
}
