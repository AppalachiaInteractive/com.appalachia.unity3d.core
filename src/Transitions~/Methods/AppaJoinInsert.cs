using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>
    ///     This allows you to change where in the game loop transitions after this will update.
    ///     NOTE: Once you submit the previous transitions, this will be reset to default.
    /// </summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "JoinInsert" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaJoinInsert)
    )]
    public class AppaJoinInsert : AppaMethod
    {
        #region Fields and Autoproperties

        public UnityEngine.Transform Target;

        public float Speed = 1.0f;

        #endregion

        public override void Register()
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaTransition.InsertTransitions(Target);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
        }
    }
}
