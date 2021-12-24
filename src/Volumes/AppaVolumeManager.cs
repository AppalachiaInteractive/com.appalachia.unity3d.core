using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Appalachia.Utility.Reflection.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Appalachia.Core.Volumes
{
    using UnityObject = UnityEngine.Object;

    /// <summary>
    ///     A global manager that tracks all the AppaVolumes in the currently loaded Scenes and does all the
    ///     interpolation work.
    /// </summary>
    public sealed class AppaVolumeManager
    {
        #region Constants and Static Readonly

        // Max amount of layers available in Unity
        private const int k_MaxLayerCount = 32;

        private static readonly Lazy<AppaVolumeManager> s_Instance =
            new Lazy<AppaVolumeManager>(() => new AppaVolumeManager());

        #endregion

        private AppaVolumeManager()
        {
            m_SortedAppaVolumes = new Dictionary<int, List<AppaVolume>>();
            m_AppaVolumes = new List<AppaVolume>();
            m_SortNeeded = new Dictionary<int, bool>();
            m_TempColliders = new List<Collider>(8);
            m_ComponentsDefaultState = new List<AppaVolumeComponent>();

            ReloadBaseTypes();

            m_DefaultStack = CreateStack();
            stack = m_DefaultStack;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     A reference to the main <see cref="AppaVolumeStack" />.
        /// </summary>
        /// <seealso cref="AppaVolumeStack" />
        public AppaVolumeStack stack { get; set; }

        /// <summary>
        ///     The current list of all available types that derive from <see cref="AppaVolumeComponent" />.
        /// </summary>
        public Type[] baseComponentTypeArray { get; private set; }

        // Keep track of sorting states for layer masks
        private readonly Dictionary<int, bool> m_SortNeeded;

        // Cached lists of all volumes (sorted by priority) by layer mask
        private readonly Dictionary<int, List<AppaVolume>> m_SortedAppaVolumes;

        // Holds all the registered volumes
        private readonly List<AppaVolume> m_AppaVolumes;

        // Internal list of default state for each component type - this is used to reset component
        // states on update instead of having to implement a Reset method on all components (which
        // would be error-prone)
        private readonly List<AppaVolumeComponent> m_ComponentsDefaultState;

        // Recycled list used for volume traversal
        private readonly List<Collider> m_TempColliders;

        // The default stack the volume manager uses.
        // We cache this as users able to change the stack through code and
        // we want to be able to switch to the default one through the ResetMainStack() function.
        private AppaVolumeStack m_DefaultStack;

        #endregion

        /// <summary>
        ///     The current singleton instance of <see cref="AppaVolumeManager" />.
        /// </summary>
        public static AppaVolumeManager instance => s_Instance.Value;

        /// <summary>
        ///     The current list of all available types that derive from <see cref="AppaVolumeComponent" />.
        /// </summary>
        [Obsolete("Please use baseComponentTypeArray instead.")]
        public IEnumerable<Type> baseComponentTypes
        {
            get => baseComponentTypeArray;
            private set => baseComponentTypeArray = value.ToArray();
        }

        /// <summary>
        ///     Checks the state of the base type library. This is only used in the editor to handle
        ///     entering and exiting of play mode and domain reload.
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public void CheckBaseTypes()
        {
            // Editor specific hack to work around serialization doing funky things when exiting
            if ((m_ComponentsDefaultState == null) ||
                ((m_ComponentsDefaultState.Count > 0) && (m_ComponentsDefaultState[0] == null)))
            {
                ReloadBaseTypes();
            }
        }

        /// <summary>
        ///     Checks the state of a given stack. This is only used in the editor to handle entering
        ///     and exiting of play mode and domain reload.
        /// </summary>
        /// <param name="stack">The stack to check.</param>
        [Conditional("UNITY_EDITOR")]
        public void CheckStack(AppaVolumeStack stack)
        {
            // The editor doesn't reload the domain when exiting play mode but still kills every
            // object created while in play mode, like stacks' component states
            var components = stack.components;

            if (components == null)
            {
                stack.Reload(baseComponentTypeArray);
                return;
            }

            foreach (var kvp in components)
            {
                if ((kvp.Key == null) || (kvp.Value == null))
                {
                    stack.Reload(baseComponentTypeArray);
                    return;
                }
            }
        }

        /// <summary>
        ///     Creates and returns a new <see cref="AppaVolumeStack" /> to use when you need to store
        ///     the result of the AppaVolume blending pass in a separate stack.
        /// </summary>
        /// <returns></returns>
        /// <seealso cref="AppaVolumeStack" />
        /// <seealso cref="Update(AppaVolumeStack,Transform,LayerMask)" />
        public AppaVolumeStack CreateStack()
        {
            var stack = new AppaVolumeStack();
            stack.Reload(baseComponentTypeArray);
            return stack;
        }

        /// <summary>
        ///     Destroy a AppaVolume Stack
        /// </summary>
        /// <param name="stack">AppaVolume Stack that needs to be destroyed.</param>
        public void DestroyStack(AppaVolumeStack stack)
        {
            stack.Dispose();
        }

        /// <summary>
        ///     Get all volumes on a given layer mask sorted by influence.
        /// </summary>
        /// <param name="layerMask">The LayerMask that Unity uses to filter AppaVolumes that it should consider.</param>
        /// <returns>An array of volume.</returns>
        public AppaVolume[] GetAppaVolumes(LayerMask layerMask)
        {
            var volumes = GrabAppaVolumes(layerMask);
            volumes.RemoveAll(v => v == null);
            return volumes.ToArray();
        }

        /// <summary>
        ///     Checks if a <see cref="AppaVolumeComponent" /> is active in a given LayerMask.
        /// </summary>
        /// <typeparam name="T">A type derived from <see cref="AppaVolumeComponent" /></typeparam>
        /// <param name="layerMask">The LayerMask to check against</param>
        /// <returns>
        ///     <c>true</c> if the component is active in the LayerMask, <c>false</c>
        ///     otherwise.
        /// </returns>
        public bool IsComponentActiveInMask<T>(LayerMask layerMask)
            where T : AppaVolumeComponent
        {
            var mask = layerMask.value;

            foreach (var kvp in m_SortedAppaVolumes)
            {
                if (kvp.Key != mask)
                {
                    continue;
                }

                foreach (var volume in kvp.Value)
                {
                    if (!volume.enabled || (volume.profileRef == null))
                    {
                        continue;
                    }

                    if (volume.profileRef.TryGet(out T component) && component.active)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Registers a new AppaVolume in the manager. Unity does this automatically when a new AppaVolume is
        ///     enabled, or its layer changes, but you can use this function to force-register a AppaVolume
        ///     that is currently disabled.
        /// </summary>
        /// <param name="volume">The volume to register.</param>
        /// <param name="layer">The LayerMask that this volume is in.</param>
        /// <seealso cref="Unregister" />
        public void Register(AppaVolume volume, int layer)
        {
            m_AppaVolumes.Add(volume);

            // Look for existing cached layer masks and add it there if needed
            foreach (var kvp in m_SortedAppaVolumes)
            {
                // We add the volume to sorted lists only if the layer match and if it doesn't contain the volume already.
                if (((kvp.Key & (1 << layer)) != 0) && !kvp.Value.Contains(volume))
                {
                    kvp.Value.Add(volume);
                }
            }

            SetLayerDirty(layer);
        }

        /// <summary>
        ///     Resets the main stack to be the default one.
        ///     Call this function if you've assigned the main stack to something other than the default one.
        /// </summary>
        public void ResetMainStack()
        {
            stack = m_DefaultStack;
        }

        /// <summary>
        ///     Unregisters a AppaVolume from the manager. Unity does this automatically when a AppaVolume is
        ///     disabled or goes out of scope, but you can use this function to force-unregister a AppaVolume
        ///     that you added manually while it was disabled.
        /// </summary>
        /// <param name="volume">The AppaVolume to unregister.</param>
        /// <param name="layer">The LayerMask that this AppaVolume is in.</param>
        /// <seealso cref="Register" />
        public void Unregister(AppaVolume volume, int layer)
        {
            m_AppaVolumes.Remove(volume);

            foreach (var kvp in m_SortedAppaVolumes)
            {
                // Skip layer masks this volume doesn't belong to
                if ((kvp.Key & (1 << layer)) == 0)
                {
                    continue;
                }

                kvp.Value.Remove(volume);
            }
        }

        /// <summary>
        ///     Updates the global state of the AppaVolume manager. Unity usually calls this once per Camera
        ///     in the Update loop before rendering happens.
        /// </summary>
        /// <param name="trigger">
        ///     A reference Transform to consider for positional AppaVolume blending
        /// </param>
        /// <param name="layerMask">
        ///     The LayerMask that the AppaVolume manager uses to filter AppaVolumes that it should consider
        ///     for blending.
        /// </param>
        public void Update(Transform trigger, LayerMask layerMask)
        {
            Update(stack, trigger, layerMask);
        }

        /// <summary>
        ///     Updates the AppaVolume manager and stores the result in a custom <see cref="AppaVolumeStack" />.
        /// </summary>
        /// <param name="stack">The stack to store the blending result into.</param>
        /// <param name="trigger">
        ///     A reference Transform to consider for positional AppaVolume blending.
        /// </param>
        /// <param name="layerMask">
        ///     The LayerMask that Unity uses to filter AppaVolumes that it should consider
        ///     for blending.
        /// </param>
        /// <seealso cref="AppaVolumeStack" />
        public void Update(AppaVolumeStack stack, Transform trigger, LayerMask layerMask)
        {
            Assert.IsNotNull(stack);

            CheckBaseTypes();
            CheckStack(stack);

            // Start by resetting the global state to default values
            ReplaceData(stack, m_ComponentsDefaultState);

            var onlyGlobal = trigger == null;
            var triggerPos = onlyGlobal ? Vector3.zero : trigger.position;

            // Sort the cached volume list(s) for the given layer mask if needed and return it
            var volumes = GrabAppaVolumes(layerMask);

            Camera camera = null;

            // Behavior should be fine even if camera is null
            if (!onlyGlobal)
            {
                trigger.TryGetComponent(out camera);
            }

            // Traverse all volumes
            foreach (var volume in volumes)
            {
                if (volume == null)
                {
                    continue;
                }

#if UNITY_EDITOR

                // Skip volumes that aren't in the scene currently displayed in the scene view
                if (!IsAppaVolumeRenderedByCamera(volume, camera))
                {
                    continue;
                }
#endif

                // Skip disabled volumes and volumes without any data or weight
                if (!volume.enabled || (volume.profileRef == null) || (volume.weight <= 0f))
                {
                    continue;
                }

                // Global volumes always have influence
                if (volume.isGlobal)
                {
                    OverrideData(stack, volume.profileRef.components, Mathf.Clamp01(volume.weight));
                    continue;
                }

                if (onlyGlobal)
                {
                    continue;
                }

                // If volume isn't global and has no collider, skip it as it's useless
                var colliders = m_TempColliders;
                volume.GetComponents(colliders);
                if (colliders.Count == 0)
                {
                    continue;
                }

                // Find closest distance to volume, 0 means it's inside it
                var closestDistanceSqr = float.PositiveInfinity;

                foreach (var collider in colliders)
                {
                    if (!collider.enabled)
                    {
                        continue;
                    }

                    var closestPoint = collider.ClosestPoint(triggerPos);
                    var d = (closestPoint - triggerPos).sqrMagnitude;

                    if (d < closestDistanceSqr)
                    {
                        closestDistanceSqr = d;
                    }
                }

                colliders.Clear();
                var blendDistSqr = volume.blendDistance * volume.blendDistance;

                // AppaVolume has no influence, ignore it
                // Note: AppaVolume doesn't do anything when `closestDistanceSqr = blendDistSqr` but we
                //       can't use a >= comparison as blendDistSqr could be set to 0 in which case
                //       volume would have total influence
                if (closestDistanceSqr > blendDistSqr)
                {
                    continue;
                }

                // AppaVolume has influence
                var interpFactor = 1f;

                if (blendDistSqr > 0f)
                {
                    interpFactor = 1f - (closestDistanceSqr / blendDistSqr);
                }

                // No need to clamp01 the interpolation factor as it'll always be in [0;1[ range
                OverrideData(
                    stack,
                    volume.profileRef.components,
                    interpFactor * Mathf.Clamp01(volume.weight)
                );
            }
        }

        internal void SetLayerDirty(int layer)
        {
            Assert.IsTrue((layer >= 0) && (layer <= k_MaxLayerCount), "Invalid layer bit");

            foreach (var kvp in m_SortedAppaVolumes)
            {
                var mask = kvp.Key;

                if ((mask & (1 << layer)) != 0)
                {
                    m_SortNeeded[mask] = true;
                }
            }
        }

        internal void UpdateAppaVolumeLayer(AppaVolume volume, int prevLayer, int newLayer)
        {
            Assert.IsTrue((prevLayer >= 0) && (prevLayer <= k_MaxLayerCount), "Invalid layer bit");
            Unregister(volume, prevLayer);
            Register(volume, newLayer);
        }

        private static bool IsAppaVolumeRenderedByCamera(AppaVolume volume, Camera camera)
        {
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR

            // IsGameObjectRenderedByCamera does not behave correctly when camera is null so we have to catch it here.
            return camera == null
                ? true
                : UnityEditor.SceneManagement.StageUtility.IsGameObjectRenderedByCamera(
                    volume.gameObject,
                    camera
                );
#else
            return true;
#endif
        }

        // Stable insertion sort. Faster than List<T>.Sort() for our needs.
        private static void SortByPriority(List<AppaVolume> volumes)
        {
            Assert.IsNotNull(volumes, "Trying to sort volumes of non-initialized layer");

            for (var i = 1; i < volumes.Count; i++)
            {
                var temp = volumes[i];
                var j = i - 1;

                // Sort order is ascending
                while ((j >= 0) && (volumes[j].priority > temp.priority))
                {
                    volumes[j + 1] = volumes[j];
                    j--;
                }

                volumes[j + 1] = temp;
            }
        }

        private List<AppaVolume> GrabAppaVolumes(LayerMask mask)
        {
            List<AppaVolume> list;

            if (!m_SortedAppaVolumes.TryGetValue(mask, out list))
            {
                // New layer mask detected, create a new list and cache all the volumes that belong
                // to this mask in it
                list = new List<AppaVolume>();

                foreach (var volume in m_AppaVolumes)
                {
                    if ((mask & (1 << volume.gameObject.layer)) == 0)
                    {
                        continue;
                    }

                    list.Add(volume);
                    m_SortNeeded[mask] = true;
                }

                m_SortedAppaVolumes.Add(mask, list);
            }

            // Check sorting state
            bool sortNeeded;
            if (m_SortNeeded.TryGetValue(mask, out sortNeeded) && sortNeeded)
            {
                m_SortNeeded[mask] = false;
                SortByPriority(list);
            }

            return list;
        }

        // Go through all listed components and lerp overridden values in the global state
        private void OverrideData(
            AppaVolumeStack stack,
            List<AppaVolumeComponent> components,
            float interpFactor)
        {
            foreach (var component in components)
            {
                if (!component.active)
                {
                    continue;
                }

                var state = stack.GetComponent(component.GetType());
                component.Override(state, interpFactor);
            }
        }

        // This will be called only once at runtime and everytime script reload kicks-in in the
        // editor as we need to keep track of any compatible component in the project
        private void ReloadBaseTypes()
        {
            m_ComponentsDefaultState.Clear();

            // Grab all the component types we can find
            baseComponentTypeArray = typeof(AppaVolumeComponent).GetAllConcreteInheritors().ToArray();

            var flags = System.Reflection.BindingFlags.Static |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.NonPublic;

            // Keep an instance of each type to be used in a virtual lowest priority global volume
            // so that we have a default state to fallback to when exiting volumes
            foreach (var type in baseComponentTypeArray)
            {
                type.GetMethod("Init", flags)?.Invoke(null, null);
                var inst = (AppaVolumeComponent)ScriptableObject.CreateInstance(type);
                m_ComponentsDefaultState.Add(inst);
            }
        }

        // Faster version of OverrideData to force replace values in the global state
        private void ReplaceData(AppaVolumeStack stack, List<AppaVolumeComponent> components)
        {
            foreach (var component in components)
            {
                var target = stack.GetComponent(component.GetType());
                var count = component.parameters.Count;

                for (var i = 0; i < count; i++)
                {
                    if (target.parameters[i] != null)
                    {
                        target.parameters[i].overrideState = false;
                        target.parameters[i].SetValue(component.parameters[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    ///     A scope in which a Camera filters a AppaVolume.
    /// </summary>
    [Obsolete("AppaVolumeIsolationScope is deprecated, it does not have any effect anymore.")]
    public struct AppaVolumeIsolationScope : IDisposable
    {
        /// <summary>
        ///     Constructs a scope in which a Camera filters a AppaVolume.
        /// </summary>
        /// <param name="unused">Unused parameter.</param>
        public AppaVolumeIsolationScope(bool unused)
        {
        }

        #region IDisposable Members

        /// <summary>
        ///     Stops the Camera from filtering a AppaVolume.
        /// </summary>
        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}
