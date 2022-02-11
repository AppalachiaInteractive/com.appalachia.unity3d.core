#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableChar : Overridable<char, OverridableChar>
    {
        public OverridableChar() : base(false, default)
        {
        }

        public OverridableChar(bool overriding, char value) : base(overriding, value)
        {
        }

        public OverridableChar(Overridable<char, OverridableChar> value) : base(value)
        {
        }
    }
}
