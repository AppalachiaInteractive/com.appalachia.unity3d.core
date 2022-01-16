using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>This component allows you to specify which transition must finish before the next transition can begin.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix + "Queue" + AppaTransition.MethodsMenuSuffix + nameof(AppaQueue)
    )]
    public class AppaQueue : AppaMethod
    {
        #region Fields and Autoproperties

        [Tooltip("The next transition will only begin after this transition has finished.")]
        public AppaMethodWithState Target;

        #endregion

        public override void Register()
        {
            AppaTransition.CurrentQueue = Target != null ? Target.PreviousState : null;
        }
    }
}
