#region

using System;
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

        public OverridableVector3Int(bool overrideEnabled, Vector3Int value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableVector3Int(Overridable<Vector3Int, OverridableVector3Int> value) : base(value)
        {
        }
    }
}
