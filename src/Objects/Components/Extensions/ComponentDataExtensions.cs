using System;
using Appalachia.Core.Objects.Components.Core;
using Appalachia.Core.Objects.Components.Exceptions;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Extensions
{
    public static class ComponentDataExtensions
    {
        #region Static Fields and Autoproperties

        private static ReusableDelegateCollection<IOverridable, Component> _delegates;

        #endregion

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the component.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        public static void Apply<TComponent, TComponentData>(
            this ComponentData<TComponent, TComponentData>.Override data,
            TComponent component)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (component == null)
                        {
                            return;
                        }

                        ApplyInternal(data, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(data, component, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the component.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TComponent, TComponentData>(
            this ComponentData<TComponent, TComponentData>.Optional data,
            TComponent component,
            Action<ComponentData<TComponent, TComponentData>.Optional, TComponent> before,
            Action<ComponentData<TComponent, TComponentData>.Optional, TComponent> after)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (component == null)
                        {
                            return;
                        }

                        before?.Invoke(data, component);
                        ApplyInternal(data, component);
                        after?.Invoke(data, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(data, component, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the component.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        public static void Apply<TComponent, TComponentData>(
            this ComponentData<TComponent, TComponentData>.Optional data,
            TComponent component)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (component == null)
                        {
                            return;
                        }

                        ApplyInternal(data, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(data, component, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the component.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TComponent, TComponentData>(
            this ComponentData<TComponent, TComponentData>.Override data,
            TComponent component,
            Action<ComponentData<TComponent, TComponentData>.Override, TComponent> before,
            Action<ComponentData<TComponent, TComponentData>.Override, TComponent> after)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (component == null)
                        {
                            return;
                        }

                        before?.Invoke(data, component);
                        ApplyInternal(data, component);
                        after?.Invoke(data, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(data, component, CreateSubscribableDelegate);
            }
        }

        private static void ApplyInternal<TComponent, TComponentData>(
            ComponentData<TComponent, TComponentData>.Optional data,
            TComponent component)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                var value = data.Value;

                value.Apply(component, false);

                if (data.IsElected)
                {
                    if (component is Behaviour behaviour)
                    {
                        behaviour.enabled = true;
                    }

                    component.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    component.MarkAsHideInHierarchyAndInspector();

                    if (component is Behaviour behaviour)
                    {
                        behaviour.enabled = false;
                    }
                }
            }
        }

        private static void ApplyInternal<TComponent, TComponentData>(
            ComponentData<TComponent, TComponentData>.Override data,
            TComponent component)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                if (!data.Overriding)
                {
                    return;
                }

                var value = data.Value;

                value.Apply(component, false);
            }
        }

        /// <summary>
        ///     Updates the component by applying the configuration values to the component fields.
        /// </summary>
        /// <param name="data">The data to apply to the component.</param>
        /// <param name="component">The component to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="component" /> is null.</exception>
        private static void SubscribeAndApply<TComponent, TComponentData, TO>(
            ComponentData<TComponent, TComponentData>.Overridable<TO> data,
            TComponent component,
            Func<Action> delegateCreator)
            where TComponent : Component
            where TComponentData : ComponentData<TComponent, TComponentData>, new()
            where TO : ComponentData<TComponent, TComponentData>.Overridable<TO>, new()
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (component == null)
                {
                    throw new ComponentNotInitializedException(
                        typeof(TComponent),
                        $"You must initialize the {typeof(TComponent).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(data, component, ref data.Changed, delegateCreator);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentDataExtensions) + ".";

        private static readonly ProfilerMarker _PRF_ApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyInternal));

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
