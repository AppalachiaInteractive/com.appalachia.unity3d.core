#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableShort : Overridable<short, OverridableShort>
    {
        public OverridableShort() : base(false, default)
        {
        }

        public OverridableShort(bool overrideEnabled, short value) : base(overrideEnabled, value)
        {
        }

        public OverridableShort(Overridable<short, OverridableShort> value) : base(value)
        {
        }
    }
}
