namespace Appalachia.Core.Objects.Delegates.Base
{
    public abstract class GameObjectValueChangedBaseArgs<TD, TV> : GameObjectValueBaseArgs<TD, TV>
        where TD : GameObjectValueChangedBaseArgs<TD, TV>, new()
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The previous value.
        /// </summary>
        public TV previousValue;

        #endregion

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
