using System;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events.Collections;
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
        public static void UpdateComponentSet<TSet, TSetData>(
            this ComponentSetData<TSet, TSetData>.Override data,
            TSet set)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (set == null) return;
                        
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
        public static void UpdateComponentSet<TSet, TSetData>(
            this ComponentSetData<TSet, TSetData>.Optional data,
            TSet set,
            Action<ComponentSetData<TSet, TSetData>.Optional, TSet> before,
            Action<ComponentSetData<TSet, TSetData>.Optional, TSet> after)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (set == null) return;
                        
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
        public static void UpdateComponentSet<TSet, TSetData>(
            this ComponentSetData<TSet, TSetData>.Optional data,
            TSet set)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (set == null) return;
                        
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
        public static void UpdateComponentSet<TSet, TSetData>(
            this ComponentSetData<TSet, TSetData>.Override data,
            TSet set,
            Action<ComponentSetData<TSet, TSetData>.Override, TSet> before,
            Action<ComponentSetData<TSet, TSetData>.Override, TSet> after)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (set == null) return;
                        
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
        private static void UpdateComponentSetAndSubscribe<TSet, TSetData, TO>(
            ComponentSetData<TSet, TSetData>.Overridable<TO> data,
            TSet set,
            Func<Action> delegateCreator)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
            where TO : ComponentSetData<TSet, TSetData>.Overridable<TO>, new()
        {
            using (_PRF_UpdateComponentSetAndSubscribe.Auto())
            {
                if (set == null)
                {
                    throw new ArgumentNullException(
                        nameof(set),
                        "You must initialize the set prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(data, set, ref data.Changed, delegateCreator);
            }
        }

        private static void UpdateComponentSetInternal<TSet, TSetData>(
            ComponentSetData<TSet, TSetData>.Optional data,
            TSet set)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSetOptional.Auto())
            {
                if (data.IsElected)
                {
                    var value = data.Value;

                    value.UpdateComponentSet(set, false);

                    set.EnableSet();
                }
                else
                {
                    set.DisableSet();
                }
            }
        }

        private static void UpdateComponentSetInternal<TSet, TSetData>(
            ComponentSetData<TSet, TSetData>.Override data,
            TSet set)
            where TSet : ComponentSet<TSet, TSetData>, new()
            where TSetData : ComponentSetData<TSet, TSetData>
        {
            using (_PRF_UpdateComponentSetOverride.Auto())
            {
                if (!data.Overriding)
                {
                    return;
                }

                var value = data.Value;

                value.UpdateComponentSet(set, false);
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
