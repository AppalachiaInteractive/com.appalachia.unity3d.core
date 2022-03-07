using System;
using Appalachia.Core.Objects.Components.Exceptions;
using Appalachia.Core.Objects.Components.Subsets;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Components.Extensions
{
    public static class ComponentSubsetDataExtensions
    {
        #region Static Fields and Autoproperties

        private static ReusableDelegateCollection<IOverridable, IComponentSubset> _delegates;

        #endregion

        /// <summary>
        ///     Updates the subset by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="subset" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the subset set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the subset.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="subset" />.</param>
        /// <param name="subset">The subset.</param>
        public static void UpdateComponentSubset<TComponentSubset, TComponentSubsetData>(
            this ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Override data,
            TComponentSubset subset)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
        {
            using (_PRF_UpdateComponentSubset.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (subset == null) return;
                        
                        UpdateComponentSubsetInternal(data, subset);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndInvoke(data, subset, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the subset by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="subset" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the subset set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the subset.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="subset" />.</param>
        /// <param name="subset">The subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void UpdateComponentSubset<TComponentSubset, TComponentSubsetData>(
            this ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Optional data,
            TComponentSubset subset,
            Action<TComponentSubsetData, TComponentSubset>
                before,
            Action<TComponentSubsetData, TComponentSubset>
                after)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
        {
            using (_PRF_UpdateComponentSubset.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (subset == null) return;
                        
                        before?.Invoke(data, subset);
                        UpdateComponentSubsetInternal(data, subset);
                        after?.Invoke(data, subset);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndInvoke(data, subset, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the subset by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="subset" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the subset set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the subset.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="subset" />.</param>
        /// <param name="subset">The subset.</param>
        public static void UpdateComponentSubset<TComponentSubset, TComponentSubsetData>(
            this ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Optional data,
            TComponentSubset subset)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
        {
            using (_PRF_UpdateComponentSubset.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (subset == null) return;
                        
                        UpdateComponentSubsetInternal(data, subset);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndInvoke(data, subset, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the subset by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="subset" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the subset set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the subset.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="data">The configuration data to apply to the <see cref="subset" />.</param>
        /// <param name="subset">The subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void UpdateComponentSubset<TComponentSubset, TComponentSubsetData>(
            this ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Override data,
            TComponentSubset subset,
            Action<TComponentSubsetData, TComponentSubset>
                before,
            Action<TComponentSubsetData, TComponentSubset>
                after)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
        {
            using (_PRF_UpdateComponentSubset.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (data == null) return;
                        if (subset == null) return;
                        
                        before?.Invoke(data, subset);
                        UpdateComponentSubsetInternal(data, subset);
                        after?.Invoke(data, subset);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndInvoke(data, subset, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the subset by applying the configuration values to the subset fields.
        /// </summary>
        /// <param name="data">The data to apply to the subset.</param>
        /// <param name="subset">The subset to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="subset" /> is null.</exception>
        private static void SubscribeAndInvoke<TComponentSubset, TComponentSubsetData, TO>(
            ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Overridable<TO> data,
            TComponentSubset subset,
            Func<Action> delegateCreator)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
            where TO : ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Overridable<TO>, new()
        {
            using (_PRF_SubscribeAndInvoke.Auto())
            {
                if (subset == null)
                {
                    throw new ComponentSubsetNotInitializedException(
                        typeof(TComponentSubset),
                        $"You must initialize the {typeof(TComponentSubset).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(data, subset, ref data.Changed, delegateCreator);
            }
        }

        private static void UpdateComponentSubsetInternal<TComponentSubset, TComponentSubsetData>(
            ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Optional data,
            TComponentSubset subset)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
        {
            using (_PRF_UpdateComponentSubsetOptional.Auto())
            {
                var value = data.Value;

                value.Apply(subset, false);

                if (data.IsElected)
                {
                    subset.Enable(data);
                    subset.gameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    subset.Disable();
                    subset.gameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        private static void UpdateComponentSubsetInternal<TComponentSubset, TComponentSubsetData>(
            ComponentSubsetData<TComponentSubset, TComponentSubsetData>.Override data,
            TComponentSubset subset)
            where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
            where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
        {
            using (_PRF_UpdateComponentSubsetOverride.Auto())
            {
                if (!data.Overriding)
                {
                    return;
                }

                var value = data.Value;

                value.Apply(subset, false);
                subset.Enable(data);
                subset.gameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentSubsetDataExtensions) + ".";

        private static readonly ProfilerMarker _PRF_UpdateComponentSubsetOptional =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSubsetInternal));

        private static readonly ProfilerMarker _PRF_UpdateComponentSubsetOverride =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSubsetInternal));

        private static readonly ProfilerMarker _PRF_UpdateComponentSubset =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSubset));

        private static readonly ProfilerMarker _PRF_SubscribeAndInvoke =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndInvoke));

        #endregion
    }
}
