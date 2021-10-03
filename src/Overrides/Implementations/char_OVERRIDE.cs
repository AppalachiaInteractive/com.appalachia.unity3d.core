#region

using System;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class char_OVERRIDE : Overridable<char, char_OVERRIDE>
    { public char_OVERRIDE() : base(false, default){}
        public char_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, char value) : base(overrideEnabled, value)
        {
        }

        public char_OVERRIDE(Overridable<char, char_OVERRIDE> value) : base(value)
        {
        }
    }
}
