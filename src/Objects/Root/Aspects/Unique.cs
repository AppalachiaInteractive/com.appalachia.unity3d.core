using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Standards;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : IUnique
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        [FormerlySerializedAs("_objectId")]
        [HideInInspector]
        [SerializeField]
        private ObjectID _objectID;

        #endregion

        #region IUnique Members

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectID ObjectID
        {
            get
            {
                if (_objectID == null)
                {
                    _objectID = ObjectID.NewObjectID();
                    MarkAsModified();
                }
                else if (_objectID.IsEmpty)
                {
                    _objectID.EnsureNotEmpty();
                    MarkAsModified();
                }

                return _objectID;
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
        [FormerlySerializedAs("_objectId")]
        [HideInInspector]
        [SerializeField]
        private ObjectID _objectID;

        #endregion

        #region IUnique Members

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectID ObjectID
        {
            get
            {
                if (_objectID == null)
                {
                    _objectID = ObjectID.NewObjectID();
                    this.MarkAsModified();
                }
                else if (_objectID.IsEmpty)
                {
                    _objectID.EnsureNotEmpty();
                    this.MarkAsModified();
                }

                return _objectID;
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

    public partial class AppalachiaSimpleBase : IUnique
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        [FormerlySerializedAs("_objectId")]
        [HideInInspector]
        [SerializeField]
        protected ObjectID _objectID;

        #endregion

        #region IUnique Members

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public virtual ObjectID ObjectID
        {
            get
            {
                if (_objectID == null)
                {
                    _objectID = ObjectID.NewObjectID();
                }
                else if (_objectID.IsEmpty)
                {
                    _objectID.EnsureNotEmpty();
                }

                return _objectID;
            }
        }

        #endregion
    }

    public partial class AppalachiaBase
    {
        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public override ObjectID ObjectID
        {
            get
            {
                if (_objectID == null)
                {
                    _objectID = ObjectID.NewObjectID();
                    MarkAsModified();
                }
                else if (_objectID.IsEmpty)
                {
                    _objectID.EnsureNotEmpty();
                    MarkAsModified();
                }

                return _objectID;
            }
        }
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
        [FormerlySerializedAs("_objectId")]
        [HideInInspector]
        [SerializeField]
        private ObjectID _objectID;

        #endregion

        /// <summary>
        ///     A persistent unique identifier for this instance.
        /// </summary>
        public ObjectID ObjectID
        {
            get
            {
                if (_objectID == null)
                {
                    _objectID = ObjectID.NewObjectID();
                    MarkAsModified();
                }
                else if (_objectID.IsEmpty)
                {
                    _objectID.EnsureNotEmpty();
                    MarkAsModified();
                }

                return _objectID;
            }
        }
    }
}
