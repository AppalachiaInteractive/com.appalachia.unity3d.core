﻿using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions
{
    /// <summary>
    ///     This is the base class for all transition states. When you register a transition (e.g. AppaTransformLocalPosition), it will return an
    ///     instance of this class, allowing it to be updated by the transition manager.
    /// </summary>
    public abstract class AppaTransitionState
    {
        public enum ConflictType
        {
            None,
            Ignore,
            Complete
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     If this is enabled then the current transition will no longer update, but will otherwise act as normal and not be removed. This is so any
        ///     transition chain can still work as expected.
        /// </summary>
        [System.NonSerialized]
        public bool Ignore;

        /// <summary>The current amount of seconds this transition has been running (-1 for pending Begin call).</summary>
        [System.NonSerialized]
        public float Age;

        /// <summary>The transition will complete after this many seconds.</summary>
        public float Duration = 1.0f;

        /// <summary>If this transition is chained to another, then this tells you which will begin after this finishes.</summary>
        [System.NonSerialized]
        public List<AppaTransitionState> Next = new List<AppaTransitionState>();

        /// <summary>If this transition is chained to another, then this tells you which must finish before this can begin.</summary>
        [System.NonSerialized]
        public List<AppaTransitionState> Prev = new List<AppaTransitionState>();

        #endregion

        public virtual ConflictType Conflict => ConflictType.Ignore;

        public virtual int CanFill => -1;

        /// <summary>This tells you how many seconds remain until this state completes.</summary>
        public float Remaining
        {
            get
            {
                // Not started yet?
                if (Age < 0.0f)
                {
                    return float.PositiveInfinity;
                }

                return Duration - Age;
            }
        }

        public abstract void Begin();

        public abstract void Update(float progress);

        public virtual void Despawn()
        {
        }

        public virtual void Fill()
        {
        }

        public virtual Object GetTarget()
        {
            return default(Object);
        }

        /// <summary>If you want this transition to begin after another completes, then call this method.</summary>
        public void BeginAfter(AppaTransitionState previousState)
        {
            Prev.Add(previousState);
            previousState.Next.Add(this);
        }

        /// <summary>This will instantly skip this transition to its final state.</summary>
        public void Skip()
        {
            if ((Age < Duration) && (Ignore == false))
            {
                Stop();
                Update(1.0f);
            }
        }

        /// <summary>This will instantly skip this transition to its final state, and all others joined to it.</summary>
        public void SkipAll()
        {
            Skip();

            foreach (var state in Next)
            {
                state.SkipAll();
            }
        }

        /// <summary>This will stop this transition at its current position.</summary>
        public void Stop()
        {
            Ignore = true;
        }

        /// <summary>This will stop this transition at its current position, and all others joined to it.</summary>
        public void StopAll()
        {
            Stop();

            foreach (var state in Next)
            {
                state.StopAll();
            }
        }
    }
}
