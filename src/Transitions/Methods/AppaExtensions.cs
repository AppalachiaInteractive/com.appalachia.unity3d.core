using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    public static class AppaExtensions
    {
        /// <summary>
        ///     This allows you to change where in the game loop transitions after this will update.
        ///     NOTE: Once you submit the previous transitions, this will be reset to default.
        /// </summary>
        public static T TimeTransition<T>(this T target, AppaTiming update)
            where T : Component
        {
            AppaTransition.CurrentTiming = update;
            return target;
        }

        /// <summary>
        ///     This allows you to change where in the game loop transitions after this will update.
        ///     NOTE: Once you submit the previous transitions, this will be reset to default.
        /// </summary>
        public static UnityEngine.GameObject TimeTransition(
            this UnityEngine.GameObject target,
            AppaTiming update)
        {
            AppaTransition.CurrentTiming = update;
            return target;
        }
    }
}
