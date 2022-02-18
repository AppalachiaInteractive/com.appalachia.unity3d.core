using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Standards;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : IUnique
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private ObjectId _objectId;

        #endregion

        #region IUnique Members

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectId ObjectId
        {
            get
            {
                if ((_objectId != null) && (_objectId != ObjectId.Empty))
                {
                    return _objectId;
                }

                _objectId = ObjectId.NewObjectId();
                MarkAsModified();

                return _objectId;
            }
        }

        #endregion
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

    public partial class AppalachiaBehaviour : IUnique
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private ObjectId _objectId;

        #endregion

        #region IUnique Members

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectId ObjectId
        {
            get
            {
                if ((_objectId != null) && (_objectId != ObjectId.Empty))
                {
                    return _objectId;
                }

                _objectId = ObjectId.NewObjectId();
                this.MarkAsModified();

                return _objectId;
            }
        }

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

    public partial class AppalachiaBase : IUnique
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private ObjectId _objectId;

        #endregion

        #region IUnique Members

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectId ObjectId
        {
            get
            {
                if ((_objectId != null) && (_objectId != ObjectId.Empty))
                {
                    return _objectId;
                }

                _objectId = ObjectId.NewObjectId();
                MarkAsModified();

                return _objectId;
            }
        }

        #endregion
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
        #region Fields and Autoproperties

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private ObjectId _objectId;

        #endregion

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectId ObjectId
        {
            get
            {
                if ((_objectId != null) && (_objectId != ObjectId.Empty))
                {
                    return _objectId;
                }

                _objectId = ObjectId.NewObjectId();
                MarkAsModified();

                return _objectId;
            }
        }
    }
}
