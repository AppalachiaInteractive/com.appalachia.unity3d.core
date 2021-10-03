#region

using System;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class string_OVERRIDE : Overridable<string, string_OVERRIDE>
    { public string_OVERRIDE() : base(false, default){}
        public string_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, string value) : base(overrideEnabled, value)
        {
        }

        public string_OVERRIDE(Overridable<string, string_OVERRIDE> value) : base(value)
        {
        }
    }
}
