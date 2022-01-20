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

        public const string BASE = APPASTR.ASPECTS.BASE;

        public const string DEPENDENCY_TRACKER_NAME = "_dependencyTracker";
        public const string GROUP = APPASTR.ASPECTS.GROUP;
        public const string GROUP_BUTTONS = APPASTR.ASPECTS.GROUP_BUTTONS;
        public const string GROUP_BUTTONS2 = APPASTR.ASPECTS.GROUP_BUTTONS2;
        public const string GROUP_BUTTONS3 = APPASTR.ASPECTS.GROUP_BUTTONS3;
        public const string GROUP_BUTTONS4 = APPASTR.ASPECTS.GROUP_BUTTONS4;
        public const string GROUP_BUTTONS5 = APPASTR.ASPECTS.GROUP_BUTTONS5;
        public const string GROUP_WORKFLOW = APPASTR.ASPECTS.GROUP_WORKFLOW;
        public const string GROUP_WORKFLOW_PROD = APPASTR.ASPECTS.GROUP_WORKFLOW_PROD;
        public const string SHOW_WORKFLOW = APPASTR.ASPECTS.SHOW_WORKFLOW;

        #endregion
    }

    public partial class AppalachiaObject
    {
        #region Constants and Static Readonly

        protected const BindingFlags DEPENDENCY_TRACKER_FLAGS =
            AppalachiaRootConstants.DEPENDENCY_TRACKER_FLAGS;

        protected const string BASE = AppalachiaRootConstants.BASE;

        protected const string DEPENDENCY_TRACKER_NAME = AppalachiaRootConstants.DEPENDENCY_TRACKER_NAME;
        protected const string GROUP = AppalachiaRootConstants.GROUP;
        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_BUTTONS2 = APPASTR.ASPECTS.GROUP_BUTTONS2;
        protected const string GROUP_BUTTONS3 = APPASTR.ASPECTS.GROUP_BUTTONS3;
        protected const string GROUP_BUTTONS4 = APPASTR.ASPECTS.GROUP_BUTTONS4;
        protected const string GROUP_BUTTONS5 = APPASTR.ASPECTS.GROUP_BUTTONS5;
        protected const string GROUP_WORKFLOW = AppalachiaRootConstants.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = AppalachiaRootConstants.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = AppalachiaRootConstants.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
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

        protected const BindingFlags DEPENDENCY_TRACKER_FLAGS =
            AppalachiaRootConstants.DEPENDENCY_TRACKER_FLAGS;

        protected const string BASE = AppalachiaRootConstants.BASE;

        protected const string DEPENDENCY_TRACKER_NAME = AppalachiaRootConstants.DEPENDENCY_TRACKER_NAME;
        protected const string GROUP = AppalachiaRootConstants.GROUP;
        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = AppalachiaRootConstants.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = AppalachiaRootConstants.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = AppalachiaRootConstants.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
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

        protected const BindingFlags DEPENDENCY_TRACKER_FLAGS =
            AppalachiaRootConstants.DEPENDENCY_TRACKER_FLAGS;

        protected const string BASE = AppalachiaRootConstants.BASE;

        protected const string DEPENDENCY_TRACKER_NAME = AppalachiaRootConstants.DEPENDENCY_TRACKER_NAME;
        protected const string GROUP = AppalachiaRootConstants.GROUP;
        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_BUTTONS2 = APPASTR.ASPECTS.GROUP_BUTTONS2;
        protected const string GROUP_BUTTONS3 = APPASTR.ASPECTS.GROUP_BUTTONS3;
        protected const string GROUP_BUTTONS4 = APPASTR.ASPECTS.GROUP_BUTTONS4;
        protected const string GROUP_BUTTONS5 = APPASTR.ASPECTS.GROUP_BUTTONS5;
        protected const string GROUP_WORKFLOW = AppalachiaRootConstants.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = AppalachiaRootConstants.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = AppalachiaRootConstants.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
        #region Constants and Static Readonly

        protected const BindingFlags DEPENDENCY_TRACKER_FLAGS =
            AppalachiaRootConstants.DEPENDENCY_TRACKER_FLAGS;

        protected const string BASE = AppalachiaRootConstants.BASE;

        protected const string DEPENDENCY_TRACKER_NAME = AppalachiaRootConstants.DEPENDENCY_TRACKER_NAME;
        protected const string GROUP = AppalachiaRootConstants.GROUP;
        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = AppalachiaRootConstants.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = AppalachiaRootConstants.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = AppalachiaRootConstants.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
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

        protected const BindingFlags DEPENDENCY_TRACKER_FLAGS =
            AppalachiaRootConstants.DEPENDENCY_TRACKER_FLAGS;

        protected const string BASE = AppalachiaRootConstants.BASE;

        protected const string DEPENDENCY_TRACKER_NAME = AppalachiaRootConstants.DEPENDENCY_TRACKER_NAME;
        protected const string GROUP = AppalachiaRootConstants.GROUP;
        protected const string GROUP_BUTTONS = AppalachiaRootConstants.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = AppalachiaRootConstants.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = AppalachiaRootConstants.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = AppalachiaRootConstants.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
    }
}
