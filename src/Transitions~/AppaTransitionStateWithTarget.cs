using UnityEngine;

namespace Appalachia.Core.Transitions
{
    /// <summary>This class stores additional base data for transitions that modify a target UnityEngine.Object (most do).</summary>
    public abstract class AppaTransitionStateWithTarget<T> : AppaTransitionState
        where T : Object
    {
        #region Fields and Autoproperties

        /// <summary>This is the target of the transition. For most transition methods this will be the component that will be modified.</summary>
        public T Target;

        #endregion

        public override int CanFill => Target != null ? 1 : 0;

        public virtual void BeginWithTarget()
        {
        }

        public virtual void FillWithTarget()
        {
        }

        public virtual void UpdateWithTarget(float progress)
        {
        }

        public override void Begin()
        {
            if (Target != null)
            {
                BeginWithTarget();
            }
        }

        public override void Fill()
        {
            if (Target != null)
            {
                FillWithTarget();
            }
        }

        public override Object GetTarget()
        {
            return Target;
        }

        public override void Update(float progress)
        {
            if (Target != null)
            {
                UpdateWithTarget(progress);
            }
        }
    }
}
