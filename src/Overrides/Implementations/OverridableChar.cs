#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableChar : Overridable<char, OverridableChar>
    {
        public OverridableChar() : base(false, default)
        {
        }

        public OverridableChar(bool overrideEnabled, char value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableChar(Overridable<char, OverridableChar> value) : base(value)
        {
        }
    }
}
