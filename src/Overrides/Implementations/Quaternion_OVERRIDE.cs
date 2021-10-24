#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Quaternion_OVERRIDE : Overridable<Quaternion, Quaternion_OVERRIDE>
    {
        public Quaternion_OVERRIDE() : base(false, default)
        {
        }

        public Quaternion_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Quaternion value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public Quaternion_OVERRIDE(Overridable<Quaternion, Quaternion_OVERRIDE> value) : base(value)
        {
        }
    }
}
