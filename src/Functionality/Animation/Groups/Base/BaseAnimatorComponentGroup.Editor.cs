#if UNITY_EDITOR
using Appalachia.Core.Functionality.Animation.Components;
using UnityEngine;

namespace Appalachia.Core.Functionality.Animation.Groups.Base
{
    public abstract partial class BaseAnimatorComponentGroup<TGroup, TConfig>
    {
        #region Fields and Autoproperties

        [SerializeField] public AnimationRemapper animationRemapper;

        #endregion

        public AnimationRemapper AnimationRemapper => animationRemapper;
    }
}

#endif
