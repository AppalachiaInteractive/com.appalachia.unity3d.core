#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Appalachia.Core.Scriptables;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Volumes
{
    [Serializable]
    public class VolumeComponent : InternalScriptableObject<VolumeComponent>
    {
        private const string _PRF_PFX = nameof(VolumeComponent) + ".";

        private static readonly ProfilerMarker _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable = new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_Override = new ProfilerMarker(_PRF_PFX + nameof(Override));

        private static readonly ProfilerMarker _PRF_SetAllOverridesTo =
            new ProfilerMarker(_PRF_PFX + nameof(SetAllOverridesTo));

        // Used to control the state of this override - handy to quickly turn a volume override
        // on & off in the editor
        public bool active = true;

        internal ReadOnlyCollection<VolumeParameter> parameters { get; private set; }

        protected virtual void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                // Automatically grab all fields of type VolumeParameter for this instance
                parameters = GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.Instance)
                            .Where(t => t.FieldType.IsSubclassOf(typeof(VolumeParameter)))
                            .OrderBy(t => t.MetadataToken) // Guaranteed order
                            .Select(t => (VolumeParameter) t.GetValue(this))
                            .ToList()
                            .AsReadOnly();

                for (var index = 0; index < parameters.Count; index++)
                {
                    var parameter = parameters[index];
                    parameter.OnEnable();
                }
            }
        }

        protected virtual void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                if (parameters == null)
                {
                    return;
                }

                foreach (var parameter in parameters)
                {
                    parameter.OnDisable();
                }
            }
        }

        // You can override this to do your own blending. Either loop through the `parameters` list
        // or reference direct fields (you'll need to cast `state` to your custom type and don't
        // forget to use `SetValue` on parameters, do not assign directly to the state object - and
        // of course you'll need to check for the `overrideState` manually).
        public virtual void Override(VolumeComponent state, float interpFactor)
        {
            using (_PRF_Override.Auto())
            {
                var count = parameters.Count;

                for (var i = 0; i < count; i++)
                {
                    var stateParam = state.parameters[i];
                    var toParam = parameters[i];

                    // Keep track of the override state for debugging purpose
//custom-begin: malte: allow partial overrides from many volumes in priority order
                    if (toParam.overrideState)
                    {
                        stateParam.overrideState = toParam.overrideState;
                        stateParam.Interp(stateParam, toParam, interpFactor);
                    }

//custom-end

                    if (toParam.overrideState)
                    {
                        stateParam.Interp(stateParam, toParam, interpFactor);
                    }
                }
            }
        }

        public void SetAllOverridesTo(bool state)
        {
            SetAllOverridesTo(parameters, state);
        }

        private void SetAllOverridesTo(IEnumerable<VolumeParameter> enumerable, bool state)
        {
            using (_PRF_SetAllOverridesTo.Auto())
            {
                foreach (var prop in enumerable)
                {
                    prop.overrideState = state;
                    var t = prop.GetType();

                    if (VolumeParameter.IsObjectParameter(t))
                    {
                        // This method won't be called a lot but this is sub-optimal, fix me
                        var innerParams = (ReadOnlyCollection<VolumeParameter>) t
                                                                               .GetProperty(
                                                                                    "parameters",
                                                                                    BindingFlags.NonPublic |
                                                                                    BindingFlags.Instance
                                                                                )
                                                                               .GetValue(prop, null);

                        if (innerParams != null)
                        {
                            SetAllOverridesTo(innerParams, state);
                        }
                    }
                }
            }
        }

        // Custom hashing function used to compare the state of settings (it's not meant to be
        // unique but to be a quick way to check if two setting sets have the same state or not).
        // Hash collision rate should be pretty low.
        public override int GetHashCode()
        {
            unchecked
            {
                //return parameters.Aggregate(17, (i, p) => i * 23 + p.GetHash());

                var hash = 17;

                foreach (var p in parameters)
                {
                    hash = (hash * 23) + p.GetHashCode();
                }

                return hash;
            }
        }
    }
}