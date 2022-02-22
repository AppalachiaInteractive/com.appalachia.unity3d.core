using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Routing;

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

    public partial class AppalachiaBehaviour : IEnableNotifier
    {
        /// <summary>
        ///     Notify the <see cref="Appalachia.Core.Objects.Routing.ObjectEnableEventRouter" /> that this <see cref="AppalachiaBehaviour" />
        ///     has been enabled, so that any subscriber can be notified in turn.
        /// </summary>
        protected abstract void RaiseNotificationWhenEnabled();

        #region IEnableNotifier Members

        /// <summary>
        ///     Does this <see cref="AppalachiaBehaviour" /> require a subscriber to its <see cref="Appalachia.Core.Objects.Routing.ObjectEnableEventRouter" /> publishing?
        ///     If so, the notifications will be stored until a subscriber becomes available.
        /// </summary>
        public virtual bool GuaranteedEventRouting => false;

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
        /// <inheritdoc />
        protected override void RaiseNotificationWhenEnabled()
        {
            using (_PRF_NotifyWhenEnabled.Auto())
            {
                ObjectEnableEventRouter.Notify(GetType(), this as T);
            }
        }
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

    public partial class AppalachiaSelectable<T> : IEnableNotifier
    {
        #region IEnableNotifier Members

        /// <summary>
        ///     Does this <see cref="AppalachiaSelectable{T}" /> require a subscriber to its <see cref="Appalachia.Core.Objects.Routing.ObjectEnableEventRouter" /> publishing?
        ///     If so, the notifications will be stored until a subscriber becomes available.
        /// </summary>
        public virtual bool GuaranteedEventRouting => false;

        #endregion
    }
}
