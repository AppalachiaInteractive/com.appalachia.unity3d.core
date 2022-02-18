using System;
using Appalachia.Core.Objects.Root.Contracts;
using UnityEngine;

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

    public partial class AppalachiaBehaviour : IBehaviour
    {
        #region Fields and Autoproperties

        [NonSerialized] private int _instanceID;

        #endregion

        #region IBehaviour Members

        public int InstanceID
        {
            get
            {
                if (_instanceID == 0)
                {
                    _instanceID = GetInstanceID();
                }

                return _instanceID;
            }
        }

        public GameObject GameObject => gameObject;

        #endregion
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
