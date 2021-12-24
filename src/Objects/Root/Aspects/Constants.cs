using Appalachia.CI.Constants;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject
    {
        #region Constants and Static Readonly

        protected const string BASE = APPASTR.ASPECTS.BASE;
        protected const string GROUP = APPASTR.ASPECTS.GROUP;
        protected const string GROUP_BUTTONS = APPASTR.ASPECTS.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = APPASTR.ASPECTS.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = APPASTR.ASPECTS.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = APPASTR.ASPECTS.SHOW_WORKFLOW;

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

    public partial class AppalachiaBehaviour<T>
    {
        #region Constants and Static Readonly

        protected const string BASE = APPASTR.ASPECTS.BASE;
        protected const string GROUP = APPASTR.ASPECTS.GROUP;
        protected const string GROUP_BUTTONS = APPASTR.ASPECTS.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = APPASTR.ASPECTS.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = APPASTR.ASPECTS.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = APPASTR.ASPECTS.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaBase
    {
        #region Constants and Static Readonly

        protected const string BASE = APPASTR.ASPECTS.BASE;
        protected const string GROUP = APPASTR.ASPECTS.GROUP;
        protected const string GROUP_BUTTONS = APPASTR.ASPECTS.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = APPASTR.ASPECTS.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = APPASTR.ASPECTS.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = APPASTR.ASPECTS.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaPlayable
    {
        #region Constants and Static Readonly

        protected const string BASE = APPASTR.ASPECTS.BASE;
        protected const string GROUP = APPASTR.ASPECTS.GROUP;
        protected const string GROUP_BUTTONS = APPASTR.ASPECTS.GROUP_BUTTONS;
        protected const string GROUP_WORKFLOW = APPASTR.ASPECTS.GROUP_WORKFLOW;
        protected const string GROUP_WORKFLOW_PROD = APPASTR.ASPECTS.GROUP_WORKFLOW_PROD;
        protected const string SHOW_WORKFLOW = APPASTR.ASPECTS.SHOW_WORKFLOW;

        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
