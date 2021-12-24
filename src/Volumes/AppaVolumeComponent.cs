using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    /// <summary>
    ///     This attribute allows you to add commands to the <strong>Add Override</strong> popup menu
    ///     on AppaVolumes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AppaVolumeComponentMenu : Attribute
    {
        // TODO: Add support for component icons

        /// <summary>
        ///     Creates a new <seealso cref="AppaVolumeComponentMenu" /> instance.
        /// </summary>
        /// <param name="menu">
        ///     The name of the entry in the override list. You can use slashes to
        ///     create sub-menus.
        /// </param>
        public AppaVolumeComponentMenu(string menu)
        {
            this.menu = menu;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The name of the entry in the override list. You can use slashes to create sub-menus.
        /// </summary>
        public readonly string menu;

        #endregion
    }

    /// <summary>
    ///     The base class for all the components that can be part of a <see cref="AppaVolumeProfile" />.
    ///     The AppaVolume framework automatically handles and interpolates any <see cref="AppaVolumeParameter" /> members found in this class.
    /// </summary>
    /// <example>
    ///     <code>
    /// using UnityEngine.Rendering;
    /// 
    /// [Serializable, AppaVolumeComponentMenuForRenderPipeline("Custom/Example Component")]
    /// public class ExampleComponent : AppaVolumeComponent
    /// {
    ///     public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    /// }
    /// </code>
    /// </example>
    [Serializable]
    public class AppaVolumeComponent : AppalachiaObject
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The active state of the set of parameters defined in this class. You can use this to
        ///     quickly turn on or off all the overrides at once.
        /// </summary>
        public bool active = true;

        /// <summary>
        ///     The name displayed in the component header. If you do not set a name, Unity generates one from
        ///     the class name automatically.
        /// </summary>
        public string displayName { get; protected set; } = "";

        /// <summary>
        ///     A read-only collection of all the <see cref="AppaVolumeParameter" />s defined in this class.
        /// </summary>
        public ReadOnlyCollection<AppaVolumeParameter> parameters { get; private set; }

        #endregion

        /// <summary>
        ///     Interpolates a <see cref="AppaVolumeComponent" /> with this component by an interpolation
        ///     factor and puts the result back into the given <see cref="AppaVolumeComponent" />.
        /// </summary>
        /// <remarks>
        ///     You can override this method to do your own blending. Either loop through the
        ///     <see cref="parameters" /> list or reference direct fields. You should only use
        ///     <see cref="AppaVolumeParameter.SetValue" /> to set parameter values and not assign
        ///     directly to the state object. you should also manually check
        ///     <see cref="AppaVolumeParameter.overrideState" /> before you set any values.
        /// </remarks>
        /// <param name="state">
        ///     The internal component to interpolate from. You must store
        ///     the result of the interpolation in this same component.
        /// </param>
        /// <param name="interpFactor">The interpolation factor in range [0,1].</param>
        /// <example>
        ///     Below is the default implementation for blending:
        ///     <code>
        /// public virtual void Override(AppaVolumeComponent state, float interpFactor)
        /// {
        ///     int count = parameters.Count;
        /// 
        ///     for (int i = 0; i &lt; count; i++)
        ///     {
        ///         var stateParam = state.parameters[i];
        ///         var toParam = parameters[i];
        /// 
        ///         // Keep track of the override state for debugging purpose
        ///         stateParam.overrideState = toParam.overrideState;
        /// 
        ///         if (toParam.overrideState)
        ///             stateParam.Interp(stateParam, toParam, interpFactor);
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual void Override(AppaVolumeComponent state, float interpFactor)
        {
            var count = parameters.Count;

            for (var i = 0; i < count; i++)
            {
                var stateParam = state.parameters[i];
                var toParam = parameters[i];

                if (toParam.overrideState)
                {
                    // Keep track of the override state for debugging purpose
                    stateParam.overrideState = toParam.overrideState;
                    stateParam.Interp(stateParam, toParam, interpFactor);
                }
            }
        }

        /// <summary>
        ///     A custom hashing function that Unity uses to compare the state of parameters.
        /// </summary>
        /// <returns>A computed hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                //return parameters.Aggregate(17, (i, p) => i * 23 + p.GetHash());

                var hash = 17;

                for (var i = 0; i < parameters.Count; i++)
                {
                    hash = (hash * 23) + parameters[i].GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        ///     Releases all the allocated resources.
        /// </summary>
        public void Release()
        {
            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] != null)
                {
                    parameters[i].Release();
                }
            }
        }

        /// <summary>
        ///     Sets the state of all the overrides on this component to a given value.
        /// </summary>
        /// <param name="state">The value to set the state of the overrides to.</param>
        public void SetAllOverridesTo(bool state)
        {
            SetOverridesTo(parameters, state);
        }

        /// <summary>
        ///     Extracts all the <see cref="AppaVolumeParameter" />s defined in this class and nested classes.
        /// </summary>
        /// <param name="o">The object to find the parameters</param>
        /// <param name="parameters">The list filled with the parameters.</param>
        /// <param name="filter">If you want to filter the parameters</param>
        internal static void FindParameters(
            object o,
            List<AppaVolumeParameter> parameters,
            Func<FieldInfo, bool> filter = null)
        {
            if (o == null)
            {
                return;
            }

            var fields = o.GetType()
                          .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                          .OrderBy(t => t.MetadataToken); // Guaranteed order

            foreach (var field in fields)
            {
                if (field.FieldType.IsSubclassOf(typeof(AppaVolumeParameter)))
                {
                    if (filter?.Invoke(field) ?? true)
                    {
                        parameters.Add((AppaVolumeParameter)field.GetValue(o));
                    }
                }
                else if (!field.FieldType.IsArray && field.FieldType.IsClass)
                {
                    FindParameters(field.GetValue(o), parameters, filter);
                }
            }
        }

        /// <summary>
        ///     Sets the override state of the given parameters on this component to a given value.
        /// </summary>
        /// <param name="state">The value to set the state of the overrides to.</param>
        internal void SetOverridesTo(IEnumerable<AppaVolumeParameter> enumerable, bool state)
        {
            foreach (var prop in enumerable)
            {
                prop.overrideState = state;
                var t = prop.GetType();

                if (AppaVolumeParameter.IsObjectParameter(t))
                {
                    // This method won't be called a lot but this is sub-optimal, fix me
                    var innerParams = (ReadOnlyCollection<AppaVolumeParameter>)t.GetProperty(
                            "parameters",
                            BindingFlags.NonPublic | BindingFlags.Instance
                        )
                       .GetValue(prop, null);

                    if (innerParams != null)
                    {
                        SetOverridesTo(innerParams, state);
                    }
                }
            }
        }

        /// <summary>
        ///     Unity calls this method before the object is destroyed.
        /// </summary>
        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();

            Release();
        }

        /// <summary>
        ///     Unity calls this method when the object goes out of scope.
        /// </summary>
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            if (parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                if (parameter != null)
                {
                    parameter.OnDisable();
                }
            }

            await AppaTask.CompletedTask;
        }

        /// <summary>
        ///     Unity calls this method when it loads the class.
        /// </summary>
        /// <remarks>
        ///     If you want to override this method, you must call <c>base.OnEnable()</c>.
        /// </remarks>
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            // Automatically grab all fields of type AppaVolumeParameter for this instance
            var fields = new List<AppaVolumeParameter>();
            FindParameters(this, fields);
            parameters = fields.AsReadOnly();

            foreach (var parameter in parameters)
            {
                if (parameter != null)
                {
                    parameter.OnEnable();
                }
                else
                {
                    Debug.LogWarning(
                        "AppaVolume Component " +
                        GetType().Name +
                        " contains a null parameter; please make sure all parameters are initialized to a default value. Until this is fixed the null parameters will not be considered by the system."
                    );
                }
            }

            await AppaTask.CompletedTask;
        }

        #region Nested type: Indent

        /// <summary>
        ///     Local attribute for AppaVolumeComponent fields only.
        ///     It handles relative indentation of a property for inspector.
        /// </summary>
        public sealed class Indent : PropertyAttribute
        {
            /// <summary> Constructor </summary>
            /// <param name="relativeAmount">Relative indent change to use</param>
            public Indent(int relativeAmount = 1)
            {
                this.relativeAmount = relativeAmount;
            }

            #region Fields and Autoproperties

            /// <summary> Relative indent amount registered in this atribute </summary>
            public readonly int relativeAmount;

            #endregion
        }

        #endregion
    }
}
