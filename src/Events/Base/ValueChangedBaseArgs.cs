namespace Appalachia.Core.Events.Base
{
    public abstract class ValueChangedBaseArgs<TD, TV> : ValueBaseArgs<TD, TV>
        where TD : ValueChangedBaseArgs<TD, TV>, new()
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
