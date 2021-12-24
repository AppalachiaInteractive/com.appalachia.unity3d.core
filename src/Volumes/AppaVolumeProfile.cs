using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Assertions;

namespace Appalachia.Core.Volumes
{
    /// <summary>
    ///     An Asset which holds a set of settings to use with a <see cref="AppaVolume" />.
    /// </summary>
    public sealed class AppaVolumeProfile : AppalachiaObject<AppaVolumeProfile>
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     A list of every setting that this AppaVolume Profile stores.
        /// </summary>
        public List<AppaVolumeComponent> components = new List<AppaVolumeComponent>();

        /// <summary>
        ///     A dirty check used to redraw the profile inspector when something has changed. This is
        ///     currently only used in the editor.
        /// </summary>
        [NonSerialized]
        public bool isDirty = true; // Editor only, doesn't have any use outside of it

        #endregion

        #region Event Functions

        /// <summary>
        ///     Resets the dirty state of the AppaVolume Profile. Unity uses this to force-refresh and redraw the
        ///     AppaVolume Profile editor when you modify the Asset via script instead of the Inspector.
        /// </summary>
        protected override void Reset()
        {
            isDirty = true;
        }

        #endregion

        /// <summary>
        ///     A custom hashing function that Unity uses to compare the state of parameters.
        /// </summary>
        /// <returns>A computed hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                for (var i = 0; i < components.Count; i++)
                {
                    hash = (hash * 23) + components[i].GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        ///     Adds a <see cref="AppaVolumeComponent" /> to this AppaVolume Profile.
        /// </summary>
        /// <remarks>
        ///     You can only have a single component of the same type per AppaVolume Profile.
        /// </remarks>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <param name="overrides">
        ///     Specifies whether Unity should automatically override all the settings when
        ///     you add a <see cref="AppaVolumeComponent" /> to the AppaVolume Profile.
        /// </param>
        /// <returns>The instance for the given type that you added to the AppaVolume Profile</returns>
        /// <seealso cref="Add" />
        public T Add<T>(bool overrides = false)
            where T : AppaVolumeComponent
        {
            return (T)Add(typeof(T), overrides);
        }

        /// <summary>
        ///     Adds a <see cref="AppaVolumeComponent" /> to this AppaVolume Profile.
        /// </summary>
        /// <remarks>
        ///     You can only have a single component of the same type per AppaVolume Profile.
        /// </remarks>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <param name="overrides">
        ///     Specifies whether Unity should automatically override all the settings when
        ///     you add a <see cref="AppaVolumeComponent" /> to the AppaVolume Profile.
        /// </param>
        /// <returns>The instance created for the given type that has been added to the profile</returns>
        /// <see cref="Add{T}" />
        public AppaVolumeComponent Add(Type type, bool overrides = false)
        {
            if (Has(type))
            {
                throw new InvalidOperationException("Component already exists in the volume");
            }

            var component = (AppaVolumeComponent)CreateInstance(type);
#if UNITY_EDITOR
            component.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            component.name = type.Name;
#endif
            component.SetAllOverridesTo(overrides);
            components.Add(component);
            isDirty = true;
            return component;
        }

        /// <summary>
        ///     Checks if this AppaVolume Profile contains the <see cref="AppaVolumeComponent" /> you pass in.
        /// </summary>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <returns>
        ///     <c>true</c> if the <see cref="AppaVolumeComponent" /> exists in the AppaVolume Profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="Has" />
        /// <seealso cref="HasSubclassOf" />
        public bool Has<T>()
            where T : AppaVolumeComponent
        {
            return Has(typeof(T));
        }

        /// <summary>
        ///     Checks if this AppaVolume Profile contains the <see cref="AppaVolumeComponent" /> you pass in.
        /// </summary>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <returns>
        ///     <c>true</c> if the <see cref="AppaVolumeComponent" /> exists in the AppaVolume Profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="Has{T}" />
        /// <seealso cref="HasSubclassOf" />
        public bool Has(Type type)
        {
            foreach (var component in components)
            {
                if (component.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Checks if this AppaVolume Profile contains the <see cref="AppaVolumeComponent" />, which is a subclass of <paramref name="type" />,
        ///     that you pass in.
        /// </summary>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <returns>
        ///     <c>true</c> if the <see cref="AppaVolumeComponent" /> exists in the AppaVolume Profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="Has" />
        /// <seealso cref="Has{T}" />
        public bool HasSubclassOf(Type type)
        {
            foreach (var component in components)
            {
                if (component.GetType().IsSubclassOf(type))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Removes a <see cref="AppaVolumeComponent" /> from this AppaVolume Profile.
        /// </summary>
        /// <remarks>
        ///     This method does nothing if the type does not exist in the AppaVolume Profile.
        /// </remarks>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <seealso cref="Remove" />
        public void Remove<T>()
            where T : AppaVolumeComponent
        {
            Remove(typeof(T));
        }

        /// <summary>
        ///     Removes a <see cref="AppaVolumeComponent" /> from this AppaVolume Profile.
        /// </summary>
        /// <remarks>
        ///     This method does nothing if the type does not exist in the AppaVolume Profile.
        /// </remarks>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <seealso cref="Remove{T}" />
        public void Remove(Type type)
        {
            var toRemove = -1;

            for (var i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == type)
                {
                    toRemove = i;
                    break;
                }
            }

            if (toRemove >= 0)
            {
                components.RemoveAt(toRemove);
                isDirty = true;
            }
        }

        /// <summary>
        ///     Gets the <see cref="AppaVolumeComponent" /> of the specified type, if it exists.
        /// </summary>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <param name="component">
        ///     The output argument that contains the <see cref="AppaVolumeComponent" />
        ///     or <c>null</c>.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="AppaVolumeComponent" /> is in the AppaVolume Profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="TryGet{T}(Type, out T)" />
        /// <seealso cref="TryGetSubclassOf{T}" />
        /// <seealso cref="TryGetAllSubclassOf{T}" />
        public bool TryGet<T>(out T component)
            where T : AppaVolumeComponent
        {
            return TryGet(typeof(T), out component);
        }

        /// <summary>
        ///     Gets the <see cref="AppaVolumeComponent" /> of the specified type, if it exists.
        /// </summary>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" /></typeparam>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <param name="component">
        ///     The output argument that contains the <see cref="AppaVolumeComponent" />
        ///     or <c>null</c>.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="AppaVolumeComponent" /> is in the AppaVolume Profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="TryGet{T}(out T)" />
        /// <seealso cref="TryGetSubclassOf{T}" />
        /// <seealso cref="TryGetAllSubclassOf{T}" />
        public bool TryGet<T>(Type type, out T component)
            where T : AppaVolumeComponent
        {
            component = null;

            foreach (var comp in components)
            {
                if (comp.GetType() == type)
                {
                    component = (T)comp;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Gets all the <seealso cref="AppaVolumeComponent" /> that are subclasses of the specified type,
        ///     if there are any.
        /// </summary>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <param name="result">
        ///     The output list that contains all the <seealso cref="AppaVolumeComponent" />
        ///     if any. Note that Unity does not clear this list.
        /// </param>
        /// <returns>
        ///     <c>true</c> if any <see cref="AppaVolumeComponent" /> have been found in the profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="TryGet{T}(Type, out T)" />
        /// <seealso cref="TryGet{T}(out T)" />
        /// <seealso cref="TryGetSubclassOf{T}" />
        public bool TryGetAllSubclassOf<T>(Type type, List<T> result)
            where T : AppaVolumeComponent
        {
            Assert.IsNotNull(components);
            var count = result.Count;

            foreach (var comp in components)
            {
                if (comp.GetType().IsSubclassOf(type))
                {
                    result.Add((T)comp);
                }
            }

            return count != result.Count;
        }

        /// <summary>
        ///     Gets the <seealso cref="AppaVolumeComponent" />, which is a subclass of <paramref name="type" />, if
        ///     it exists.
        /// </summary>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <param name="type">A type that inherits from <see cref="AppaVolumeComponent" />.</param>
        /// <param name="component">
        ///     The output argument that contains the <see cref="AppaVolumeComponent" />
        ///     or <c>null</c>.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="AppaVolumeComponent" /> is in the AppaVolume Profile,
        ///     <c>false</c> otherwise.
        /// </returns>
        /// <seealso cref="TryGet{T}(Type, out T)" />
        /// <seealso cref="TryGet{T}(out T)" />
        /// <seealso cref="TryGetAllSubclassOf{T}" />
        public bool TryGetSubclassOf<T>(Type type, out T component)
            where T : AppaVolumeComponent
        {
            component = null;

            foreach (var comp in components)
            {
                if (comp.GetType().IsSubclassOf(type))
                {
                    component = (T)comp;
                    return true;
                }
            }

            return false;
        }

        internal int GetComponentListHashCode()
        {
            unchecked
            {
                var hash = 17;

                for (var i = 0; i < components.Count; i++)
                {
                    hash = (hash * 23) + components[i].GetType().GetHashCode();
                }

                return hash;
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            // Make sure every setting is valid. If a profile holds a script that doesn't exist
            // anymore, nuke it to keep the volume clean. Note that if you delete a script that is
            // currently in use in a volume you'll still get a one-time error in the console, it's
            // harmless and happens because Unity does a redraw of the editor (and thus the current
            // frame) before the recompilation step.
            components.RemoveAll(x => x == null);

            await AppaTask.CompletedTask;
        }
    }
}
