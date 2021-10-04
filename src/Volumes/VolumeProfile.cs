#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Volumes.Components;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace Appalachia.Core.Volumes
{
    [CreateAssetMenu(menuName = "Internal/Volumes/VolumeProfile", order = 0)]
    public sealed class VolumeProfile : ScriptableObject
    {
        private const string _PRF_PFX = nameof(VolumeProfile) + ".";

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Add = new(_PRF_PFX + nameof(Add));

        private static readonly ProfilerMarker _PRF_Remove = new(_PRF_PFX + nameof(Remove));

        private static readonly ProfilerMarker _PRF_Has = new(_PRF_PFX + nameof(Has));

        private static readonly ProfilerMarker _PRF_HasSubclassOf =
            new(_PRF_PFX + nameof(HasSubclassOf));

        private static readonly ProfilerMarker _PRF_TryGet = new(_PRF_PFX + nameof(TryGet));

        private static readonly ProfilerMarker _PRF_TryGetSubclassOf =
            new(_PRF_PFX + nameof(TryGetSubclassOf));

        private static readonly ProfilerMarker _PRF_TryGetAllSubclassOf =
            new(_PRF_PFX + nameof(TryGetAllSubclassOf));

        public List<VolumeComponent> components = new();

        // Editor only, doesn't have any use outside of it
        [NonSerialized] public bool isDirty = true;

        public void Reset()
        {
            isDirty = true;
        }

        private void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                // Make sure every setting is valid. If a profile holds a script that doesn't exist
                // anymore, nuke it to keep the volume clean. Note that if you delete a script that is
                // currently in use in a volume you'll still get a one-time error in the console, it's
                // harmless and happens because Unity does a redraw of the editor (and thus the current
                // frame) before the recompilation step.
                components.RemoveAll(x => x == null);
            }
        }

        public T Add<T>(bool overrides = false)
            where T : VolumeComponent
        {
            return (T) Add(typeof(T), overrides);
        }

        public VolumeComponent Add(Type type, bool overrides = false)
        {
            using (_PRF_Add.Auto())
            {
                if (Has(type))
                {
                    throw new InvalidOperationException("Component already exists in the volume");
                }

                var component = (VolumeComponent) CreateInstance(type);
                component.SetAllOverridesTo(overrides);
                components.Add(component);
                isDirty = true;
                return component;
            }
        }

        public void Remove<T>()
            where T : VolumeComponent
        {
            Remove(typeof(T));
        }

        public void Remove(Type type)
        {
            using (_PRF_Remove.Auto())
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
        }

        public bool Has<T>()
            where T : VolumeComponent
        {
            return Has(typeof(T));
        }

        public bool Has(Type type)
        {
            using (_PRF_Has.Auto())
            {
                for (var index = 0; index < components.Count; index++)
                {
                    var component = components[index];
                    if (component.GetType() == type)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasSubclassOf(Type type)
        {
            using (_PRF_HasSubclassOf.Auto())
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
        }

        public bool TryGet<T>(out T component)
            where T : VolumeComponent
        {
            using (_PRF_TryGet.Auto())
            {
                var type = typeof(T);
                return TryGet(type, out component);
            }
        }

        public bool TryGet<T>(Type type, out T component)
            where T : VolumeComponent
        {
            using (_PRF_TryGet.Auto())
            {
                component = null;

                for (var index = 0; index < components.Count; index++)
                {
                    var comp = components[index];
                    if (comp.GetType() == type)
                    {
                        component = (T) comp;
                        return true;
                    }
                }

                return false;
            }
        }

        public bool TryGetSubclassOf<T>(Type type, out T component)
            where T : VolumeComponent
        {
            using (_PRF_TryGetSubclassOf.Auto())
            {
                component = null;

                for (var index = 0; index < components.Count; index++)
                {
                    var comp = components[index];
                    if (comp.GetType().IsSubclassOf(type))
                    {
                        component = (T) comp;
                        return true;
                    }
                }

                return false;
            }
        }

        public bool TryGetAllSubclassOf<T>(Type type, List<T> result)
            where T : VolumeComponent
        {
            using (_PRF_TryGetAllSubclassOf.Auto())
            {
                Assert.IsNotNull(components);
                var count = result.Count;

                for (var index = 0; index < components.Count; index++)
                {
                    var comp = components[index];
                    if (comp.GetType().IsSubclassOf(type))
                    {
                        result.Add((T) comp);
                    }
                }

                return count != result.Count;
            }
        }
    }
}
