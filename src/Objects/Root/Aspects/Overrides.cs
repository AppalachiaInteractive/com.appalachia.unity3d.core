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
        #region Nested type: Optional

        [Serializable]
        public sealed class Optional : Overridable<Optional>
        {
            public Optional() : base(false, default)
            {
            }

            public Optional(bool isElected, T value) : base(isElected, value)
            {
            }

            public Optional(Optional value) : base(value)
            {
            }

            public bool IsElected => Overriding;

            /// <inheritdoc />
            protected override string DisabledColorPrefName => "Optional Disabled Color";

            /// <inheritdoc />
            protected override string EnabledColorPrefName => "Optional Enabled Color";

            /// <inheritdoc />
            protected override string ToggleLabel => "Optional";
        }

        #endregion

        #region Nested type: Overridable

        [Serializable]
        public abstract class Overridable<TO> : Overridable<T, TO>
            where TO : Overridable<TO>, new()
        {
            protected Overridable() : base(false, default)
            {
            }

            protected Overridable(bool overriding, T value) : base(overriding, value)
            {
            }

            protected Overridable(TO value) : base(value)
            {
            }
        }

        #endregion

        #region Nested type: Override

        [Serializable]
        public sealed class Override : Overridable<Override>
        {
            public Override() : base(false, default)
            {
            }

            public Override(bool overriding, T value) : base(overriding, value)
            {
            }

            public Override(Override value) : base(value)
            {
            }
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
