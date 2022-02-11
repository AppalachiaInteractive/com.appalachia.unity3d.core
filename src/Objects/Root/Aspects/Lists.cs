using System;
using Appalachia.Core.Collections;

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
        #region Nested type: List

        [Serializable]
        public sealed class List : AppaList<T>
        {
            public List()
            {
            }

            public List(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
                capacity,
                capacityIncreaseMultiplier,
                noTracking
            )
            {
            }

            public List(AppaList<T> list) : base(list)
            {
            }

            public List(T[] values) : base(values)
            {
            }
        }

        #endregion
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
        #region Nested type: List

        [Serializable]
        public sealed class List : AppaList<T>
        {
            public List()
            {
            }

            public List(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
                capacity,
                capacityIncreaseMultiplier,
                noTracking
            )
            {
            }

            public List(AppaList<T> list) : base(list)
            {
            }

            public List(T[] values) : base(values)
            {
            }
        }

        #endregion
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
        #region Nested type: List

        [Serializable]
        public sealed class List : AppaList<T>
        {
            public List()
            {
            }

            public List(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
                capacity,
                capacityIncreaseMultiplier,
                noTracking
            )
            {
            }

            public List(AppaList<T> list) : base(list)
            {
            }

            public List(T[] values) : base(values)
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
        #region Nested type: List

        [Serializable]
        public sealed class List : AppaList<T>
        {
            public List()
            {
            }

            public List(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
                capacity,
                capacityIncreaseMultiplier,
                noTracking
            )
            {
            }

            public List(AppaList<T> list) : base(list)
            {
            }

            public List(T[] values) : base(values)
            {
            }
        }

        #endregion
    }

    public partial class AppalachiaSelectable<T>
    {
        #region Nested type: List

        [Serializable]
        public sealed class List : AppaList<T>
        {
            public List()
            {
            }

            public List(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
                capacity,
                capacityIncreaseMultiplier,
                noTracking
            )
            {
            }

            public List(AppaList<T> list) : base(list)
            {
            }

            public List(T[] values) : base(values)
            {
            }
        }

        #endregion
    }
}
