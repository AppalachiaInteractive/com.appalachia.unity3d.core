using System.Diagnostics;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    [DebuggerDisplay("{Name} (AppalachiaObject)")]
    public partial class AppalachiaObject : INamed
    {
        #region Fields and Autoproperties

        [SerializeField]
        [HideInInspector]
        private string _niceName;

        #endregion

        #region INamed Members

        public string NiceName
        {
            get
            {
                if (_niceName.IsNullOrWhiteSpace())
                {
                    _niceName = Name.Nicify();
                    MarkAsModified();
                }

                return _niceName;
            }
            set => _niceName = value;
        }

        public string Name => name;

        #endregion
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    [DebuggerDisplay("{Name} (SingletonAppalachiaObject)")]
    public partial class SingletonAppalachiaObject<T>
    {
    }

    [DebuggerDisplay("{Name} (AppalachiaBehaviour)")]
    public partial class AppalachiaBehaviour : INamed
    {
        #region Fields and Autoproperties

        [SerializeField]
        [HideInInspector]
        private string _niceName;

        #endregion

        #region INamed Members

        public string NiceName
        {
            get
            {
                if (_niceName.IsNullOrWhiteSpace())
                {
                    _niceName =Name.Nicify();
                }

                return _niceName;
            }
        }

        public string Name => name;

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    [DebuggerDisplay("{Name} (SingletonAppalachiaBehaviour)")]
    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    [DebuggerDisplay("{Name} (AppalachiaSimpleBase)")]
    public partial class AppalachiaSimpleBase : INamed
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private string _name;

        [SerializeField]
        [HideInInspector]
        private string _niceName;

        #endregion

        #region INamed Members

        public string NiceName
        {
            get
            {
                if (_niceName.IsNullOrWhiteSpace())
                {
                    _niceName = Name.Nicify();
                }

                return _niceName;
            }
        }

        public virtual string Name
        {
            get
            {
                if (_name.IsNullOrWhiteSpace())
                {
                    _name = GetType().GetReadableName();
                }

                return _name;
            }
        }

        #endregion
    }

    [DebuggerDisplay("{Name} (AppalachiaBase)")]
    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    [DebuggerDisplay("{Name} (AppalachiaSimplePlayable)")]
    public partial class AppalachiaSimplePlayable : INamed
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private string _name;

        [SerializeField]
        [HideInInspector]
        private string _niceName;

        #endregion

        #region INamed Members

        public string NiceName
        {
            get
            {
                if (_niceName.IsNullOrWhiteSpace())
                {
                    _niceName = Name.Nicify();
                }

                return _niceName;
            }
        }

        public virtual string Name
        {
            get
            {
                if (_name.IsNullOrWhiteSpace())
                {
                    _name = GetType().GetReadableName();
                }

                return _name;
            }
        }

        #endregion
    }

    [DebuggerDisplay("{Name} (AppalachiaPlayable)")]
    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    [DebuggerDisplay("{Name} (AppalachiaSelectable)")]
    public partial class AppalachiaSelectable<T>
    {
        #region Constants and Static Readonly

        private const string COMPONENT_FORMAT_STRING = "{0}.{1}";

        #endregion

        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private string _name;

        [SerializeField]
        [HideInInspector]
        private string _niceName;

        #endregion

        #region IInitializable Members

        public string NiceName
        {
            get
            {
                if (_niceName.IsNullOrWhiteSpace())
                {
                    _niceName = Name.Nicify();
                    MarkAsModified();
                }

                return _niceName;
            }
        }

        public string Name
        {
            get
            {
                if (_name.IsNullOrWhiteSpace())
                {
                    _name = ZString.Format(COMPONENT_FORMAT_STRING, gameObject.name, typeof(T).Name);
                }

                return _name;
            }
        }

        #endregion
    }
}
