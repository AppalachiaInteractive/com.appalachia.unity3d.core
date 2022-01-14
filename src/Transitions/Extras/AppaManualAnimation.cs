using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Core.Transitions.Extras
{
    /// <summary>This component allows you to manually begin transitions from UI button events and other sources.</summary>
    [AddComponentMenu(PKG.Prefix + nameof(AppaManualAnimation))]
    public class AppaManualAnimation : AppalachiaBehaviour<AppaManualAnimation>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("Transitions")]
        private AppaTransitionPlayer transitions;

        [System.NonSerialized]
        private HashSet<AppaTransitionState> states = new HashSet<AppaTransitionState>();

        [System.NonSerialized] private bool registered;

        #endregion

        /// <summary>
        ///     This allows you to specify the transitions this component will begin.
        ///     You can create a new transition GameObject by right clicking the transition name, and selecting <b>Create</b>.
        ///     For example, the <b>Graphic.color Transition (AppaGraphicColor)</b> component can be used to change the color back to normal.
        /// </summary>
        public AppaTransitionPlayer Transitions
        {
            get
            {
                if (transitions == null)
                {
                    transitions = new AppaTransitionPlayer();
                }

                return transitions;
            }
        }

        /// <summary>This method will execute all transitions on the <b>Transform</b> specified in the <b>Transitions</b> setting.</summary>
        [ContextMenu("Begin Transitions")]
        public void BeginTransitions()
        {
            if (transitions != null)
            {
                if (registered == false)
                {
                    registered = true;

                    AppaTransition.OnFinished += HandleFinished;
                }

                AppaTransition.OnRegistered += HandleRegistered;

                transitions.Begin();

                AppaTransition.OnRegistered -= HandleRegistered;
            }
        }

        /// <summary>This method will skip all transitions that were begun from this component.</summary>
        [ContextMenu("Skip Transitions")]
        public void SkipTransitions()
        {
            foreach (var state in states)
            {
                state.Skip();
            }
        }

        /// <summary>This method will stop all transitions that were begun from this component.</summary>
        [ContextMenu("Stop Transitions")]
        public void StopTransitions()
        {
            foreach (var state in states)
            {
                state.Stop();
            }
        }

        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();

            using (_PRF_WhenDisabled.Auto())
            {
                if (registered)
                {
                    // Comment this out in case you call BeginTransitions after destruction?
                    //registered = false;

                    AppaTransition.OnFinished -= HandleFinished;
                }
            }
        }

        private void HandleFinished(AppaTransitionState state)
        {
            states.Remove(state);
        }

        private void HandleRegistered(AppaTransitionState state)
        {
            states.Add(state);
        }
    }
}
