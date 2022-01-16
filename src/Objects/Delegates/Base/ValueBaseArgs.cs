namespace Appalachia.Core.Objects.Delegates.Base
{
    public abstract class ValueBaseArgs<TD, TV> : DelegateBaseArgs<TD>
        where TD : ValueBaseArgs<TD, TV>, new()
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The current value.
        /// </summary>
        public TV value;

        #endregion

        protected override void OnInitialize()
        {
            using (_PRF_OnInitialize.Auto())
            {
                base.OnInitialize();
            }
        }

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
