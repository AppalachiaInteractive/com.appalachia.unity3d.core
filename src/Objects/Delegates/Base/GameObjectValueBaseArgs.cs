namespace Appalachia.Core.Objects.Delegates.Base
{
    public abstract class GameObjectValueBaseArgs<TD, TV> : GameObjectBaseArgs<TD>
        where TD : GameObjectValueBaseArgs<TD, TV>, new()
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
