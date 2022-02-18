using UnityEngine;

namespace Appalachia.Core.Events.Base
{
    public abstract class ComponentBaseArgs<TD, TC> : DelegateBaseArgs<TD>
        where TD : ComponentBaseArgs<TD, TC>, new()
        where TC : Component
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The <see cref="Component" /> that raised the event.
        /// </summary>
        public TC component;

        #endregion

        /// <inheritdoc />
        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                base.OnReset();

                component = null;
            }
        }
    }
}
