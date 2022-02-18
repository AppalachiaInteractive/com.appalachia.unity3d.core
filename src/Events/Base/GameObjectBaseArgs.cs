using UnityEngine;

namespace Appalachia.Core.Events.Base
{
    public abstract class GameObjectBaseArgs<TD> : DelegateBaseArgs<TD>
        where TD : GameObjectBaseArgs<TD>, new()
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The <see cref="GameObject" /> that raised the event.
        /// </summary>
        public GameObject gameObject;

        #endregion

        /// <inheritdoc />
        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                base.OnReset();

                gameObject = null;
            }
        }
    }
}
