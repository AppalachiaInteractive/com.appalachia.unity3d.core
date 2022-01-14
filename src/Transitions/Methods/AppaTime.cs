using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>
    ///     This allows you to change where in the game loop transitions after this will update.
    ///     NOTE: Once you submit the previous transitions, this will be reset to default.
    /// </summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix + "Time" + AppaTransition.MethodsMenuSuffix + nameof(AppaTime)
    )]
    public class AppaTime : AppaMethod
    {
        #region Fields and Autoproperties

        public AppaTiming Update = AppaTiming.Default;

        #endregion

        public override void Register()
        {
            AppaTransition.CurrentTiming = Update;
        }
    }
}
