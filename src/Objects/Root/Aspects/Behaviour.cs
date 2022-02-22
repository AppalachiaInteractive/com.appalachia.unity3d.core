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

        public Transform Transform
        {
            get
            {
                using (_PRF_Transform.Auto())
                {
                    if (_hasCachedTransform)
                    {
                        return _cachedTransform;
                    }

                    if (_cachedTransform == null)
                    {
                        _cachedTransform = transform;
                        _hasCachedTransform = true;
                    }

                    return _cachedTransform;
                }
            }
        }

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
        #region IBehaviour Members

        public GameObject GameObject => gameObject;

        public int InstanceID
        {
            get
            {
                if (_instanceId == default)
                {
                    _instanceId = GetInstanceID();
                }

                return _instanceId;
            }
        }

        public Transform Transform => rectTransform;

        #endregion
    }
}
