using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Reflection.Extensions;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : INamed
    {
        #region IInitializable Members

        public string Name => name;

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

    public partial class AppalachiaBehaviour : INamed
    {
        #region IInitializable Members

        public string Name => name;

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase : INamed
    {
        #region Fields and Autoproperties

        private string _name;

        #endregion

        #region INamed Members

        public virtual string Name
        {
            get
            {
                if (_name != null)
                {
                    return _name;
                }

                _name = GetType().GetReadableName();

                return _name;
            }
        }

        #endregion
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable : INamed
    {
        #region Fields and Autoproperties

        private string _name;

        #endregion

        #region IInitializable Members

        public virtual string Name
        {
            get
            {
                if (_name != null)
                {
                    return _name;
                }

                _name = GetType().GetReadableName();

                return _name;
            }
        }

        #endregion
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
