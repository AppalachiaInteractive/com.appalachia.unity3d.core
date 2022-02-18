using UnityEngine;

namespace Appalachia.Core.Events.Base
{
    public abstract class ComponentValueChangedBaseArgs<TD, TC, TV> : ComponentValueBaseArgs<TD, TC, TV>
        where TD : ComponentValueChangedBaseArgs<TD, TC, TV>, new()
        where TC : Component
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The previous value.
        /// </summary>
        public TV previousValue;

        #endregion

        /// <inheritdoc />
        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                base.OnReset();
                previousValue = default;
            }
        }
    }
}
