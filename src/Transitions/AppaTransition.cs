using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Transitions
{
    /// <summary>This component updates all active transition methods, both in game, and in the editor.</summary>
    [ExecuteInEditMode]
    [AddComponentMenu(PKG.Prefix + nameof(AppaTransition))]
    public sealed class AppaTransition : AppalachiaBehaviour<AppaTransition>
    {
        public static event System.Action<AppaTransitionState> OnFinished;

        public static event System.Action<AppaTransitionState> OnRegistered;

        #region Constants and Static Readonly

        public const string MethodsMenuPrefix = "Lean/Transition/Methods/";

        public const string MethodsMenuSuffix = " Transition ";

        #endregion

        #region Static Fields and Autoproperties

        /// <summary>This stores a list of all active and enabled <b>AppaTransition</b> instances in the scene.</summary>
        public static List<AppaTransition> Instances = new List<AppaTransition>();

        private static AppaTiming currentTiming;

        private static AppaTransitionState currentQueue;

        private static AppaTransitionState defaultQueue;

        private static AppaTransitionState previousState;

        private static bool started;

        private static Dictionary<string, Object> currentAliases = new Dictionary<string, Object>();

        private static Dictionary<string, System.Type> aliasTypePairs = new Dictionary<string, System.Type>();

        private static float currentSpeed = 1.0f;

        private static List<AppaMethod> baseMethodStack = new List<AppaMethod>();

        private static List<AppaMethod> tempBaseMethods = new List<AppaMethod>();

        private static List<AppaTransitionState> fixedUpdateStates = new List<AppaTransitionState>();

        private static List<AppaTransitionState> lateUpdateStates = new List<AppaTransitionState>();

        private static List<AppaTransitionState> unscaledFixedUpdateStates = new List<AppaTransitionState>();

        private static List<AppaTransitionState> unscaledLateUpdateStates = new List<AppaTransitionState>();

        private static List<AppaTransitionState> unscaledUpdateStates = new List<AppaTransitionState>();

        private static List<AppaTransitionState> updateStates = new List<AppaTransitionState>();

        #endregion

        #region Fields and Autoproperties

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("Timing")]
        private AppaTiming defaultTiming = AppaTiming.UnscaledUpdate;

        #endregion

        /// <summary>This property gives you the first <b>DefaultTiming</b> instance value.</summary>
        public static AppaTiming CurrentDefaultTiming
        {
            get
            {
                if (Instances.Count > 0)
                {
                    return Instances[0].defaultTiming;
                }

                return default(AppaTiming);
            }
        }

        /// <summary>After a transition state is registered, it will be stored here. This allows you to copy it out for later use.</summary>
        public static AppaTransitionState PreviousState => previousState;

        /// <summary>This allows you to change the alias name to UnityEngine.Object association of all future transitions in the current animation.</summary>
        public static Dictionary<string, Object> CurrentAliases => currentAliases;

        /// <summary>This tells you how many transitions are currently running.</summary>
        public static int Count =>
            unscaledUpdateStates.Count +
            unscaledLateUpdateStates.Count +
            unscaledFixedUpdateStates.Count +
            updateStates.Count +
            lateUpdateStates.Count +
            fixedUpdateStates.Count;

        /// <summary>This allows you to change where in the game loop all future transitions in the current animation will be updated.</summary>
        public static AppaTiming CurrentTiming
        {
            set => currentTiming = value;
        }

        /// <summary>If you want the next registered transition state to automatically begin after an existing transition state, then specify it here.</summary>
        public static AppaTransitionState CurrentQueue
        {
            set => currentQueue = value;
        }

        /// <summary>This allows you to change the transition speed multiplier of all future transitions in the current animation.</summary>
        public static float CurrentSpeed
        {
            set => currentSpeed = value;

            get => currentSpeed;
        }

        /// <summary>This allows you to set where in the game loop animations are updated when timing = AppaTime.Default.</summary>
        public AppaTiming DefaultTiming
        {
            set => defaultTiming = value;
            get => defaultTiming;
        }

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if ((this == Instances[0]) && Application.isPlaying && started)
                {
                    UpdateAll(unscaledUpdateStates, Time.unscaledDeltaTime);
                    UpdateAll(updateStates,         Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            using (_PRF_FixedUpdate.Auto())
            {
                if ((this == Instances[0]) && Application.isPlaying && started)
                {
                    UpdateAll(unscaledFixedUpdateStates, Time.fixedUnscaledDeltaTime);
                    UpdateAll(fixedUpdateStates,         Time.fixedDeltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            using (_PRF_LateUpdate.Auto())
            {
                if ((this == Instances[0]) && Application.isPlaying)
                {
                    if (started)
                    {
                        UpdateAll(unscaledLateUpdateStates, Time.unscaledDeltaTime);
                        UpdateAll(lateUpdateStates,         Time.deltaTime);
                    }
                    else
                    {
                        started = true;
                    }
                }
            }
        }

        #endregion

        public static void AddAlias(string key, Object obj)
        {
            currentAliases.Remove(key);
            currentAliases.Add(key, obj);
        }

        /// <summary>
        ///     This will begin all transitions on the specified GameObject, all its children, and then submit them.
        ///     If you failed to submit a previous transition then this will also throw an error.
        /// </summary>
        public static void BeginAllTransitions(Transform root, float speed = 1.0f)
        {
            ResetState();

            if (root != null)
            {
                RequireSubmitted();

                InsertTransitions(root, speed);

                Submit();
            }
        }

        /// <summary>This method returns all TargetAliases on all transitions on the specified Transform.</summary>
        public static Dictionary<string, System.Type> FindAllAliasTypePairs(Transform root)
        {
            aliasTypePairs.Clear();

            AddAliasTypePairs(root);

            return aliasTypePairs;
        }

        /// <summary>This method will return the specified timing, unless it's set to <b>Default</b>, then it will return <b>UnscaledTime</b>.</summary>
        public static AppaTiming GetTiming(AppaTiming current = AppaTiming.Default)
        {
            if (current == AppaTiming.Default)
            {
                current = AppaTiming.UnscaledUpdate;
            }

            return current;
        }

        /// <summary>This method works like <b>GetTiming</b>, but it won't return any unscaled times.</summary>
        public static AppaTiming GetTimingAbs(AppaTiming current)
        {
            return (AppaTiming)System.Math.Abs((int)current);
        }

        /// <summary>This will begin all transitions on the specified GameObject, and all its children.</summary>
        public static void InsertTransitions(
            GameObject root,
            float speed = 1.0f,
            AppaTransitionState parentHead = null)
        {
            if (root != null)
            {
                InsertTransitions(root.transform, speed);
            }
        }

        /// <summary>This will begin all transitions on the specified Transform, and all its children.</summary>
        public static void InsertTransitions(
            Transform root,
            float speed = 1.0f,
            AppaTransitionState parentHead = null)
        {
            if (root != null)
            {
                var spd = currentSpeed;
                var min = baseMethodStack.Count;
                root.GetComponents(tempBaseMethods);
                baseMethodStack.AddRange(tempBaseMethods);
                tempBaseMethods.Clear();
                var max = baseMethodStack.Count;

                currentSpeed *= speed;

                if (parentHead != null)
                {
                    previousState = parentHead;
                    currentQueue = parentHead;
                }

                defaultQueue = parentHead;

                for (var i = min; i < max; i++)
                {
                    baseMethodStack[i].Register();
                }

                baseMethodStack.RemoveRange(min, max - min);

                var childParentHead = previousState;

                for (var i = 0; i < root.childCount; i++)
                {
                    InsertTransitions(root.GetChild(i), 1.0f, childParentHead);
                }

                currentSpeed = spd;
            }
        }

        public static AppaTransitionState Register(AppaTransitionState state, float duration)
        {
            state.Duration = duration;

            // Execute immediately?
            if ((duration == 0.0f) && (state.Prev.Count == 0))
            {
                FinishState(state);

                if (previousState == state)
                {
                    previousState = null;
                }

                return null;
            }

            // Register for later execution?

            if (currentSpeed > 0.0f)
            {
                state.Duration /= currentSpeed;
            }

            // Convert currentTiming if it's set to default, then register the state in the correct list
            var finalUpdate = GetTiming(currentTiming);

            switch (finalUpdate)
            {
                case AppaTiming.UnscaledFixedUpdate:
                    unscaledFixedUpdateStates.Add(state);
                    break;
                case AppaTiming.UnscaledLateUpdate:
                    unscaledLateUpdateStates.Add(state);
                    break;
                case AppaTiming.UnscaledUpdate:
                    unscaledUpdateStates.Add(state);
                    break;
                case AppaTiming.Update:
                    updateStates.Add(state);
                    break;
                case AppaTiming.LateUpdate:
                    lateUpdateStates.Add(state);
                    break;
                case AppaTiming.FixedUpdate:
                    fixedUpdateStates.Add(state);
                    break;
            }

            if (OnRegistered != null)
            {
                OnRegistered(state);
            }

            return state;
        }

        /// <summary>If you failed to submit a previous transition then this will throw an error, and then submit them.</summary>
        public static void RequireSubmitted()
        {
            if (currentQueue != null)
            {
                Debug.LogError(
                    "You forgot to submit the last transition! " +
                    currentQueue.GetType() +
                    " - " +
                    currentQueue.GetTarget()
                );

                Submit();
            }

            if (baseMethodStack.Count > 0)
            {
                Debug.LogError("Failed to submit all methods.");

                Submit();
            }
        }

        /// <summary>This will reset any previously called <b>CurrentQueue</b> calls.</summary>
        public static void ResetQueue()
        {
            currentQueue = null;
        }

        /// <summary>This will reset any previously called <b>CurrentSpeed</b> calls.</summary>
        public static void ResetSpeed()
        {
            currentSpeed = 1.0f;
        }

        /// <summary>This will reset the <b>CurrentTiming</b>, <b>CurrentQueue</b>, and <b>CurrentSpeed</b> values.</summary>
        public static void ResetState()
        {
            defaultQueue = null;

            ResetTiming();

            ResetQueue();

            ResetSpeed();
        }

        /// <summary>This will reset any previously called <b>CurrentTiming</b> calls.</summary>
        public static void ResetTiming()
        {
            currentTiming = CurrentDefaultTiming;
        }

        public static T Spawn<T>(Stack<T> pool)
            where T : AppaTransitionState, new()
        {
            // Make sure the transition manager exists
            if (Instances.Count == 0)
            {
                new GameObject("AppaTransition").AddComponent<AppaTransition>();
            }

            // Setup initial data
            var state = pool.Count > 0 ? pool.Pop() : new T();

            state.Age = -1.0f;
            state.Ignore = false;

            state.Prev.Clear();
            state.Next.Clear();

            // Join to previous transition?
            if (currentQueue != null)
            {
                state.BeginAfter(currentQueue);

                currentQueue = defaultQueue;
            }

            // Make this the new head
            previousState = state;

            return state;
        }

        public static T SpawnWithTarget<T, U>(Stack<T> pool, U target)
            where T : AppaTransitionStateWithTarget<U>, new()
            where U : Object
        {
            var data = Spawn(pool);

            data.Target = target;

            return data;
        }

        /// <summary>This will submit any previously registered transitions, and reset the timing.</summary>
        public static void Submit()
        {
            ResetState();

            baseMethodStack.Clear();
        }

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                Instances.Remove(this);

                if (Instances.Count == 0)
                {
                    unscaledFixedUpdateStates.Clear();
                    unscaledLateUpdateStates.Clear();
                    unscaledUpdateStates.Clear();
                    updateStates.Clear();
                    lateUpdateStates.Clear();
                    fixedUpdateStates.Clear();
                }
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                Instances.Add(this);

                ResetState();
#if UNITY_EDITOR
                EditorApplication.update -= HandleUpdateInEditor;
                EditorApplication.update += HandleUpdateInEditor;
#endif
            }
        }

        private static void AddAliasTypePairs(Transform root)
        {
            if (root != null)
            {
                root.GetComponents(tempBaseMethods);

                for (var i = 0; i < tempBaseMethods.Count; i++)
                {
                    var baseMethod = tempBaseMethods[i] as AppaMethodWithStateAndTarget;

                    if (baseMethod != null)
                    {
                        var targetType = baseMethod.GetTargetType();
                        var alias = baseMethod.Alias;

                        if (string.IsNullOrEmpty(alias) == false)
                        {
                            var existingType = default(System.Type);

                            // Exists?
                            if (aliasTypePairs.TryGetValue(alias, out existingType))
                            {
                                // Clashing types?
                                if (existingType != targetType)
                                {
                                    // If both are components then the clash can be resolved by using GameObject
                                    if (targetType.IsSubclassOf(typeof(Component)))
                                    {
                                        // If it's already a GameObject, skip
                                        if (existingType == typeof(GameObject))
                                        {
                                            continue;
                                        }

                                        // Change existing type to GameObject?

                                        if (existingType.IsSubclassOf(typeof(Component)))
                                        {
                                            aliasTypePairs[alias] = typeof(GameObject);

                                            continue;
                                        }
                                    }

                                    // If the clash cannot be resolved, throw an error
                                    Debug.LogError(
                                        "The (" +
                                        root.name +
                                        ") GameObject contains multiple transitions that define a target alias of (" +
                                        alias +
                                        "), but these transitions use different types (" +
                                        existingType +
                                        ") + (" +
                                        targetType +
                                        "). You must give them different aliases.",
                                        root
                                    );
                                }
                            }

                            // Add new?
                            else
                            {
                                aliasTypePairs.Add(alias, targetType);
                            }
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        ///     If a transition is being animated in the editor, then the target object may not update, so this method will automatically dirty it so that
        ///     it will.
        /// </summary>
        private static void DirtyTarget(AppaTransitionState transition)
        {
            if (Application.isPlaying == false)
            {
                var targetField = transition.GetType().GetField("Target");

                if (targetField != null)
                {
                    var target = targetField.GetValue(transition) as Object;

                    if (target != null)
                    {
                        EditorUtility.SetDirty(target);
                    }
                }
            }
        }
#endif

        private static void FinishState(AppaTransitionState state)
        {
            // Activate all chained states and clear them
            for (var j = state.Next.Count - 1; j >= 0; j--)
            {
                state.Next[j].Prev.Remove(state);
            }

            state.Next.Clear();

            // Make sure we call update one final time with a progress value of exactly 1.0
            if (state.Ignore == false)
            {
                state.Update(1.0f);
            }

            if (OnFinished != null)
            {
                OnFinished(state);
            }

#if UNITY_EDITOR
            DirtyTarget(state);
#endif

            state.Despawn();
        }

#if UNITY_EDITOR
        private void HandleUpdateInEditor()
        {
            var delta = Time.deltaTime;

            if (Application.isPlaying == false)
            {
                UpdateAll(unscaledFixedUpdateStates, delta);
                UpdateAll(unscaledLateUpdateStates,  delta);
                UpdateAll(unscaledUpdateStates,      delta);
                UpdateAll(updateStates,              delta);
                UpdateAll(lateUpdateStates,          delta);
                UpdateAll(fixedUpdateStates,         delta);
            }
        }
#endif

        /// <summary>This method will mark all transitions as Skip = true if they match the transition type and target object of the specified transition.</summary>
        private void RemoveConflictsBefore(
            List<AppaTransitionState> states,
            AppaTransitionState currentState,
            int currentIndex)
        {
            var currentConflict = currentState.Conflict;

            if (currentConflict != AppaTransitionState.ConflictType.None)
            {
                var currentType = currentState.GetType();
                var currentTarget = currentState.GetTarget();

                for (var i = 0; i < currentIndex; i++)
                {
                    var transition = states[i];

                    if ((transition.Ignore == false) &&
                        (transition.GetType() == currentType) &&
                        (transition.GetTarget() == currentTarget))
                    {
                        transition.Ignore = true;

                        if (currentConflict == AppaTransitionState.ConflictType.Complete)
                        {
                            transition.Update(1.0f);
                        }
                    }
                }
            }
        }

        private void UpdateAll(List<AppaTransitionState> states, float delta)
        {
            ResetState();

            for (var i = states.Count - 1; i >= 0; i--)
            {
                var state = states[i];

                // If we have a negative duration, skip ahead of time?
                if ((state.Prev.Count > 0) && (state.Duration < 0.0f))
                {
                    var skip = -state.Duration;

                    for (var j = state.Prev.Count - 1; j >= 0; j--)
                    {
                        var prev = state.Prev[j];

                        if (prev.Remaining <= skip)
                        {
                            prev.Next.Remove(state);

                            state.Prev.RemoveAt(j);
                        }
                    }
                }

                // Only update if the previous transitions have finished
                if (state.Prev.Count == 0)
                {
                    // If the transition age is negative, it hasn't started yet
                    if (state.Age < 0.0f)
                    {
                        state.Age = 0.0f;

                        // If this newly beginning transition is identical to an already registered one, mark the existing one as conflicting so it doesn't get updated
                        RemoveConflictsBefore(states, state, i);

                        // Begin the transition (this will often copy the current state of the variable that is being transitioned)
                        state.Begin();
                    }

                    // Age
                    state.Age += delta;

                    // Finished?
                    if (state.Age >= state.Duration)
                    {
                        FinishState(state);

                        states.RemoveAt(i);
                    }

                    // Update
                    else
                    {
                        if (state.Ignore == false)
                        {
                            state.Update(state.Age / state.Duration);
                        }

#if UNITY_EDITOR
                        DirtyTarget(state);
#endif
                    }
                }
            }
        }
    }
}
