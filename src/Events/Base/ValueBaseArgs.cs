namespace Appalachia.Core.Events.Base
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

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            using (_PRF_OnInitialize.Auto())
            {
                base.OnInitialize();
            }
        }

        /// <inheritdoc />
        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                base.OnReset();
                value = default;
            }
        }
    }

    public abstract class ValueBaseArgs<TD, TV1, TV2> : DelegateBaseArgs<TD>
        where TD : ValueBaseArgs<TD, TV1, TV2>, new()
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The current value 1.
        /// </summary>
        public TV1 value1;

        /// <summary>
        ///     The current value 2.
        /// </summary>
        public TV2 value2;

        #endregion

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            using (_PRF_OnInitialize.Auto())
            {
                base.OnInitialize();
            }
        }

        /// <inheritdoc />
        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                base.OnReset();
                value1 = default;
                value2 = default;
            }
        }
    }
}
