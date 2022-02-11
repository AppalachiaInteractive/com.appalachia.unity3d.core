using System.Reflection;
using Appalachia.CI.Constants;

namespace Appalachia.Core.Objects.Root
{
    internal static class AppalachiaRootConstants
    {
        #region Constants and Static Readonly

        public const BindingFlags DEPENDENCY_TRACKER_FLAGS = BindingFlags.Public |
                                                             BindingFlags.NonPublic |
                                                             BindingFlags.GetField |
                                                             BindingFlags.Static;

        public const string DEPENDENCY_TRACKER_NAME = "_dependencyTracker";
        public const string GROUP_BUTTONS = GROUP_INTERNAL + "/" + APPASTR.Buttons;
        public const string GROUP_INTERNAL = GROUP_ROOT + "/" + APPASTR.Internal;
        public const string GROUP_ROOT = "$ShowMetadata";
        public const string GROUP_WORKFLOW = GROUP_INTERNAL + "/" + APPASTR.Workflow;
        public const string GROUP_WORKFLOW_PROD = GROUP_WORKFLOW + "/" + APPASTR.Productivity;

        #endregion
    }

    public partial class AppalachiaObject
    {
        #region Constants and Static Readonly

        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_INTERNAL = AppalachiaRootConstants.GROUP_INTERNAL;

        protected const string GROUP_ROOT = AppalachiaRootConstants.GROUP_ROOT;
        protected const string GROUP_WORKFLOW = AppalachiaRootConstants.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = AppalachiaRootConstants.GROUP_WORKFLOW_PROD;

        #endregion

        protected virtual bool ShowMetadata => false;
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
        #region Constants and Static Readonly

        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_INTERNAL = AppalachiaRootConstants.GROUP_INTERNAL;

        protected const string GROUP_ROOT = AppalachiaRootConstants.GROUP_ROOT;

        #endregion

        protected virtual bool ShowMetadata => false;
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaBase
    {
        #region Constants and Static Readonly

        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_INTERNAL = AppalachiaRootConstants.GROUP_INTERNAL;

        protected const string GROUP_ROOT = AppalachiaRootConstants.GROUP_ROOT;

        #endregion

        protected virtual bool ShowMetadata => false;
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
        #region Constants and Static Readonly

        protected const string GROUP_INTERNAL = AppalachiaRootConstants.GROUP_INTERNAL;

        protected const string GROUP_ROOT = AppalachiaRootConstants.GROUP_ROOT;

        #endregion

        protected virtual bool ShowMetadata => false;
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T>
    {
        #region Constants and Static Readonly

        protected const string GROUP_INTERNAL = AppalachiaRootConstants.GROUP_INTERNAL;

        protected const string GROUP_ROOT = AppalachiaRootConstants.GROUP_ROOT;

        #endregion

        protected virtual bool ShowMetadata => false;
    }
}
