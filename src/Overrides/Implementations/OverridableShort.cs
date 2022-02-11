#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableShort : Overridable<short, OverridableShort>
    {
        public OverridableShort() : base(false, default)
        {
        }

        public OverridableShort(bool overriding, short value) : base(overriding, value)
        {
        }

        public OverridableShort(Overridable<short, OverridableShort> value) : base(value)
        {
        }
    }
}
