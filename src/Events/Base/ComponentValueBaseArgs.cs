using UnityEngine;

namespace Appalachia.Core.Events.Base
{
    public abstract class ComponentValueBaseArgs<TD, TC, TV> : ComponentBaseArgs<TD, TC>
        where TD : ComponentValueBaseArgs<TD, TC, TV>, new()
        where TC : Component
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The current value.
        /// </summary>
        public TV value;

        #endregion

        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                base.OnReset();
                value = default;
            }
        }
    }
}
