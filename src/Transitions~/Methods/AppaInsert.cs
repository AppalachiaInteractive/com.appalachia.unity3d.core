using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>This will submit all transitions you added before this one. Any transitions you perform after this will begin immediately.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix + "Insert" + AppaTransition.MethodsMenuSuffix + nameof(AppaInsert)
    )]
    public class AppaInsert : AppaMethod
    {
        #region Fields and Autoproperties

        public UnityEngine.Transform Target;

        #endregion

        public override void Register()
        {
            AppaTransition.InsertTransitions(Target);
        }
    }
}
