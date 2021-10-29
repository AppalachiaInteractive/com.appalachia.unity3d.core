#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Volumes.Components;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Volumes
{
    public sealed class AppaVolumeStack : IDisposable
    {
        internal AppaVolumeStack()
        {
        }

        // Holds the state of _all_ component types you can possibly add on volumes
        public Dictionary<Type, AppaVolumeComponent> components;

        public T GetComponent<T>()
            where T : AppaVolumeComponent
        {
            var comp = GetComponent(typeof(T));
            return (T) comp;
        }

        public AppaVolumeComponent GetComponent(Type type)
        {
            AppaVolumeComponent comp;
            components.TryGetValue(type, out comp);
            return comp;
        }

        public void Dispose()
        {
            foreach (var component in components)
            {
                Object.Destroy(component.Value);
            }

            components.Clear();
        }

        internal void Reload(IEnumerable<Type> baseTypes)
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
                var inst = (AppaVolumeComponent) ScriptableObject.CreateInstance(type);
                components.Add(type, inst);
            }
        }
    }
}
