#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableString : Overridable<string, OverridableString>
    {
        public OverridableString() : base(false, default)
        {
        }

        public OverridableString(bool overrideEnabled, string value) : base(overrideEnabled, value)
        {
        }

        public OverridableString(Overridable<string, OverridableString> value) : base(value)
        {
        }
    }
}
