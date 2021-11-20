#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Appalachia.Core.Scriptables;
using Appalachia.Core.Volumes.Parameters;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Volumes.Components
{
    [Serializable]
    public class AppaVolumeComponent : AppalachiaObject
    {
        #region Fields and Autoproperties

        // Used to control the state of this override - handy to quickly turn a volume override
        // on & off in the editor
        public bool active = true;

        internal ReadOnlyCollection<AppaVolumeParameter> parameters { get; private set; }

        #endregion

        #region Event Functions

        protected virtual void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                // Automatically grab all fields of type AppaVolumeParameter for this instance
                parameters = GetType()
                            .GetFields_CACHE(ReflectionExtensions.PublicInstance)
                            .Where(t => t.FieldType.IsSubclassOf(typeof(AppaVolumeParameter)))
                            .OrderBy(t => t.MetadataToken) // Guaranteed order
                            .Select(t => (AppaVolumeParameter) t.GetValue(this))
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

        #endregion

        // You can override this to do your own blending. Either loop through the `parameters` list
        // or reference direct fields (you'll need to cast `state` to your custom type and don't
        // forget to use `SetValue` on parameters, do not assign directly to the state object - and
        // of course you'll need to check for the `overrideState` manually).
        public virtual void Override(AppaVolumeComponent state, float interpFactor)
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

        // Custom hashing function used to compare the state of settings (it's not meant to be
        // unique but to be a quick way to check if two setting sets have the same state or not).
        // Hash collision rate should be pretty low.
        [DebuggerStepThrough]
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

        public void SetAllOverridesTo(bool state)
        {
            SetAllOverridesTo(parameters, state);
        }

        private void SetAllOverridesTo(IEnumerable<AppaVolumeParameter> enumerable, bool state)
        {
            using (_PRF_SetAllOverridesTo.Auto())
            {
                foreach (var prop in enumerable)
                {
                    prop.overrideState = state;
                    var t = prop.GetType();

                    if (AppaVolumeParameter.IsObjectParameter(t))
                    {
                        // This method won't be called a lot but this is sub-optimal, fix me
                        var innerParams = (ReadOnlyCollection<AppaVolumeParameter>) t.GetProperty(
                                "parameters",
                                ReflectionExtensions.PrivateInstance
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

        #region Profiling

        private const string _PRF_PFX = nameof(AppaVolumeComponent) + ".";
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_Override = new(_PRF_PFX + nameof(Override));

        private static readonly ProfilerMarker _PRF_SetAllOverridesTo =
            new(_PRF_PFX + nameof(SetAllOverridesTo));

        #endregion
    }
}
