#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Volumes.Components;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Volumes
{
    public sealed class VolumeStack : IDisposable
    {
        // Holds the state of _all_ component types you can possibly add on volumes
        public Dictionary<Type, VolumeComponent> components;

        internal VolumeStack()
        {
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
                components = new Dictionary<Type, VolumeComponent>();
            }
            else
            {
                components.Clear();
            }

            foreach (var type in baseTypes)
            {
                var inst = (VolumeComponent) ScriptableObject.CreateInstance(type);
                components.Add(type, inst);
            }
        }

        public T GetComponent<T>()
            where T : VolumeComponent
        {
            var comp = GetComponent(typeof(T));
            return (T) comp;
        }

        public VolumeComponent GetComponent(Type type)
        {
            VolumeComponent comp;
            components.TryGetValue(type, out comp);
            return comp;
        }
    }
}
