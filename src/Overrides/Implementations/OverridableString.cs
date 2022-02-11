#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableString : Overridable<string, OverridableString>
    {
        public OverridableString() : base(false, default)
        {
        }

        public OverridableString(bool overriding, string value) : base(overriding, value)
        {
        }

        public OverridableString(Overridable<string, OverridableString> value) : base(value)
        {
        }
    }
}
