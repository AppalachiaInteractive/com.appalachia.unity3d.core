using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>
    ///     This allows you to change where in the game loop transitions after this will update.
    ///     NOTE: Once you submit the previous transitions, this will be reset to default.
    /// </summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "JoinDelay" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaJoinDelay)
    )]
    public class AppaJoinDelay : AppaMethod
    {
        #region Fields and Autoproperties

        public float Duration;

        #endregion

        public override void Register()
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaDelay.Register(Duration);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
        }
    }
}
