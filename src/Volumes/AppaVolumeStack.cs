using System;
using System.Collections.Generic;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    /// <summary>
    ///     Holds the state of a AppaVolume blending update. A global stack is
    ///     available by default in <see cref="AppaVolumeManager" /> but you can also create your own using
    ///     <see cref="AppaVolumeManager.CreateStack" /> if you need to update the manager with specific
    ///     settings and store the results for later use.
    /// </summary>
    public sealed class AppaVolumeStack : IDisposable
    {
        internal AppaVolumeStack()
        {
        }

        #region Fields and Autoproperties

        // Holds the state of _all_ component types you can possibly add on volumes
        internal Dictionary<Type, AppaVolumeComponent> components;

        #endregion

        /// <summary>
        ///     Gets the current state of the <see cref="AppaVolumeComponent" /> of type <typeparamref name="T" />
        ///     in the stack.
        /// </summary>
        /// <typeparam name="T">A type of <see cref="AppaVolumeComponent" />.</typeparam>
        /// <returns>
        ///     The current state of the <see cref="AppaVolumeComponent" /> of type <typeparamref name="T" />
        ///     in the stack.
        /// </returns>
        public T GetComponent<T>()
            where T : AppaVolumeComponent
        {
            var comp = GetComponent(typeof(T));
            return (T)comp;
        }

        /// <summary>
        ///     Gets the current state of the <see cref="AppaVolumeComponent" /> of the specified type in the
        ///     stack.
        /// </summary>
        /// <param name="type">The type of <see cref="AppaVolumeComponent" /> to look for.</param>
        /// <returns>
        ///     The current state of the <see cref="AppaVolumeComponent" /> of the specified type,
        ///     or <c>null</c> if the type is invalid.
        /// </returns>
        public AppaVolumeComponent GetComponent(Type type)
        {
            components.TryGetValue(type, out var comp);
            return comp;
        }

        internal void Reload(Type[] baseTypes)
        {
            if (components == null)
            {
                components = new Dictionary<Type, AppaVolumeComponent>();
            }
            else
            {
                components.Clear();
            }

            foreach (var type in baseTypes)
            {
                var inst = (AppaVolumeComponent)ScriptableObject.CreateInstance(type);
                components.Add(type, inst);
            }
        }

        #region IDisposable Members

        /// <summary>
        ///     Cleans up the content of this stack. Once a <c>AppaVolumeStack</c> is disposed, it souldn't
        ///     be used anymore.
        /// </summary>
        public void Dispose()
        {
            foreach (var component in components)
            {
                component.Value.DestroySafely();
            }

            components.Clear();
        }

        #endregion
    }
}
