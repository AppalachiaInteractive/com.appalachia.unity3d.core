using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>
    ///     This allows you to change where in the game loop transitions after this will update.
    ///     NOTE: Once you submit the previous transitions, this will be reset to default.
    /// </summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix + "Join" + AppaTransition.MethodsMenuSuffix + nameof(AppaJoin)
    )]
    public class AppaJoin : AppaMethod
    {
        public override void Register()
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
        }
    }
}
