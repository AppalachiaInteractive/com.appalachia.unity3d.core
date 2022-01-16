namespace Appalachia.Core.Transitions
{
    public abstract class AppaMethodWithState : AppaMethod
    {
        #region Fields and Autoproperties

        /// <summary>Each time this transition method registers a new state, it will be stored here.</summary>
        public AppaTransitionState PreviousState;

        #endregion
    }
}
