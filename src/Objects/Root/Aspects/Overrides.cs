using System;
using Appalachia.Core.Objects.Models;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject
    {
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
        #region Nested type: Override

        [Serializable]
        public sealed class Override : Overridable<T, Override>
        {
            public Override() : base(false, default)
            {
            }

            public Override(bool overriding, T value) : base(overriding, value)
            {
            }

            public Override(Overridable<T, Override> value) : base(value)
            {
            }
        }

        [Serializable]
        public sealed class Optional : Overridable<T, Optional>
        {
            public Optional() : base(false, default)
            {
            }

            public Optional(bool isElected, T value) : base(isElected, value)
            {
            }

            public Optional(Overridable<T, Optional> value) : base(value)
            {
            }

            public bool IsElected => Overriding;

            protected override string DisabledColorPrefName => "Optional Disabled Color";
            protected override string EnabledColorPrefName => "Optional Enabled Color";
            protected override string ToggleLabel => "Optional";
        }

        #endregion
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T>
    {
    }
}
