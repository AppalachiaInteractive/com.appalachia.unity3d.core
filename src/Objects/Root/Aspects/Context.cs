using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Logging;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : IContextualInstance
    {
        #region Fields and Autoproperties

        [NonSerialized] private AppaContext _context;

        #endregion

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }

        #region IContextualInstance Members

        AppaContext IContextualInstance.Context => Context;

        #endregion
    }

    public partial class AppalachiaRepository : IContextual
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static AppaContext _staticContext;

        #endregion

        private static AppaContext StaticContext
        {
            get
            {
                if (_staticContext == null)
                {
                    _staticContext = new AppaContext(typeof(AppalachiaRepository));
                }

                return _staticContext;
            }
        }

        private static AppaLogContext SingletonContext => AppaLog.Context.Singleton;

        #region IContextual Members

        AppaContext IContextualInstance.Context => Context;
        AppaContext IContextualStatic.StaticContext => StaticContext;

        #endregion
    }

    public partial class AppalachiaObject<T> : IContextual
    {
        #region Static Fields and Autoproperties

        [NonSerialized] protected static AppaContext _staticContext;

        #endregion

        protected static AppaContext StaticContext
        {
            get
            {
                if (_staticContext == null)
                {
                    _staticContext = new AppaContext(typeof(T));
                }

                return _staticContext;
            }
        }

        #region IContextual Members

        AppaContext IContextualStatic.StaticContext => StaticContext;

        #endregion
    }

    public partial class SingletonAppalachiaObject<T>
    {
        protected static AppaLogContext SingletonContext => AppaLog.Context.Singleton;
    }

    public partial class AppalachiaBehaviour : IContextualInstance
    {
        #region Fields and Autoproperties

        [NonSerialized] protected AppaContext _context;

        #endregion

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }

        #region IContextualInstance Members

        AppaContext IContextualInstance.Context => Context;

        #endregion
    }

    public partial class AppalachiaBehaviour<T> : IContextual
    {
        #region Static Fields and Autoproperties

        [NonSerialized] protected static AppaContext _staticContext;

        #endregion

        protected static AppaContext StaticContext
        {
            get
            {
                if (_staticContext == null)
                {
                    _staticContext = new AppaContext(typeof(T));
                }

                return _staticContext;
            }
        }

        #region IContextual Members

        AppaContext IContextualStatic.StaticContext => StaticContext;

        #endregion
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
        protected static AppaLogContext SingletonContext => AppaLog.Context.Singleton;
    }

    public partial class AppalachiaSimpleBase : IContextualInstance
    {
        #region Fields and Autoproperties

        [NonSerialized] protected AppaContext _context;

        #endregion

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }

        #region IContextualInstance Members

        AppaContext IContextualInstance.Context => Context;

        #endregion
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T> : IContextualStatic
    {
        #region Static Fields and Autoproperties

        [NonSerialized] protected static AppaContext _staticContext;

        #endregion

        protected static AppaContext StaticContext
        {
            get
            {
                if (_staticContext == null)
                {
                    _staticContext = new AppaContext(typeof(T));
                }

                return _staticContext;
            }
        }

        #region IContextualStatic Members

        AppaContext IContextualStatic.StaticContext => StaticContext;

        #endregion
    }

    public partial class AppalachiaSimplePlayable : IContextualInstance
    {
        #region Fields and Autoproperties

        [NonSerialized] private AppaContext _context;

        #endregion

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }

        #region IContextualInstance Members

        AppaContext IContextualInstance.Context => Context;

        #endregion
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T> : IContextual
    {
        #region Static Fields and Autoproperties

        [NonSerialized] protected static AppaContext _staticContext;

        #endregion

        protected static AppaContext StaticContext
        {
            get
            {
                if (_staticContext == null)
                {
                    _staticContext = new AppaContext(typeof(T));
                }

                return _staticContext;
            }
        }

        #region IContextual Members

        AppaContext IContextualStatic.StaticContext => StaticContext;

        #endregion
    }

    public partial class AppalachiaSelectable<T> : IContextual
    {
        #region Static Fields and Autoproperties

        [NonSerialized] protected static AppaContext _staticContext;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] protected AppaContext _context;

        #endregion

        protected static AppaContext StaticContext
        {
            get
            {
                if (_staticContext == null)
                {
                    _staticContext = new AppaContext(typeof(T));
                }

                return _staticContext;
            }
        }

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }

        #region IContextual Members

        AppaContext IContextualInstance.Context => Context;

        AppaContext IContextualStatic.StaticContext => StaticContext;

        #endregion
    }
}
