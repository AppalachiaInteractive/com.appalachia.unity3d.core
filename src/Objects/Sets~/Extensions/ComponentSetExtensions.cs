using System;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Sets2.Exceptions;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Sets.Extensions
{
    public static class ComponentSetDataExtensions
    {
        #region Static Fields and Autoproperties

        private static ReusableDelegateCollection<IOverridable, IComponentSet> _delegates;

        #endregion

        /// <summary>
        ///     Updates the set by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="set" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the set set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="set" />.</param>
        /// <param name="set">The set.</param>
        public static void UpdateComponentSet<TComponentSet, TComponentSetData>(
            this ComponentSetData<TComponentSet, TComponentSetData>.Override data,
            TComponentSet set)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (set == null)
                        {
                            return;
                        }

                        UpdateComponentSetInternal(data, set);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(data, set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="set" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the set set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="set" />.</param>
        /// <param name="set">The set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void UpdateComponentSet<TComponentSet, TComponentSetData>(
            this ComponentSetData<TComponentSet, TComponentSetData>.Optional data,
            TComponentSet set,
            Action<ComponentSetData<TComponentSet, TComponentSetData>.Optional, TComponentSet> before,
            Action<ComponentSetData<TComponentSet, TComponentSetData>.Optional, TComponentSet> after)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (set == null)
                        {
                            return;
                        }

                        before?.Invoke(data, set);
                        UpdateComponentSetInternal(data, set);
                        after?.Invoke(data, set);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(data, set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="set" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the set set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="set" />.</param>
        /// <param name="set">The set.</param>
        public static void UpdateComponentSet<TComponentSet, TComponentSetData>(
            this ComponentSetData<TComponentSet, TComponentSetData>.Optional data,
            TComponentSet set)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (set == null)
                        {
                            return;
                        }

                        UpdateComponentSetInternal(data, set);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(data, set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="set" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the set set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="set" />.</param>
        /// <param name="set">The set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void UpdateComponentSet<TComponentSet, TComponentSetData>(
            this ComponentSetData<TComponentSet, TComponentSetData>.Override data,
            TComponentSet set,
            Action<ComponentSetData<TComponentSet, TComponentSetData>.Override, TComponentSet> before,
            Action<ComponentSetData<TComponentSet, TComponentSetData>.Override, TComponentSet> after)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null)
                        {
                            return;
                        }

                        if (set == null)
                        {
                            return;
                        }

                        before?.Invoke(data, set);
                        UpdateComponentSetInternal(data, set);
                        after?.Invoke(data, set);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(data, set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="data">The data to apply to the set.</param>
        /// <param name="set">The set to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        private static void UpdateComponentSetAndSubscribe<TComponentSet, TComponentSetData, TO>(
            ComponentSetData<TComponentSet, TComponentSetData>.Overridable<TO> data,
            TComponentSet set,
            Func<Action> delegateCreator)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
            where TO : ComponentSetData<TComponentSet, TComponentSetData>.Overridable<TO>, new()
        {
            using (_PRF_UpdateComponentSetAndSubscribe.Auto())
            {
                if (set == null)
                {
                    throw new ComponentSetNotInitializedException(
                        typeof(TComponentSet),
                        $"You must initialize the {typeof(TComponentSet).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(data, set, ref data.Changed, delegateCreator);
            }
        }

        private static void UpdateComponentSetInternal<TComponentSet, TComponentSetData>(
            ComponentSetData<TComponentSet, TComponentSetData>.Optional data,
            TComponentSet set)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_UpdateComponentSetOptional.Auto())
            {
                //if (data.IsElected)
                {
                    var value = data.Value;

                    value.UpdateComponentSet(set, false);
                }

                if (data.IsElected)
                {
                    set.Enable(data);
                    set.GameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    set.Disable();
                    set.GameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        private static void UpdateComponentSetInternal<TComponentSet, TComponentSetData>(
            ComponentSetData<TComponentSet, TComponentSetData>.Override data,
            TComponentSet set)
            where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
            where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>
        {
            using (_PRF_UpdateComponentSetOverride.Auto())
            {
                if (!data.Overriding)
                {
                    return;
                }

                var value = data.Value;
                
                value.UpdateComponentSet(set, false);
               
                set.Enable(data.Value);
                set.GameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentSetDataExtensions) + ".";

        private static readonly ProfilerMarker _PRF_UpdateComponentSetOptional =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetInternal));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetOverride =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetInternal));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetAndSubscribe =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetAndSubscribe));

        #endregion
    }
}
