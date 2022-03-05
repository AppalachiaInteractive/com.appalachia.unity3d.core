using System;
using System.Diagnostics;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Sets;
using Appalachia.Core.Objects.Sets.Exceptions;
using Appalachia.Core.Objects.Sets2.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets2
{
    [CallStaticConstructorInEditor]
    [Serializable]
    [DebuggerDisplay("{Name} (ComponentSetData)")]
    public abstract class ComponentSetData<TComponentSet, TComponentSetData> : AppalachiaBase<TComponentSetData>,
                                                                               IComponentSetData<TComponentSet,
                                                                                   TComponentSetData>
        where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>,
        IComponentSetData<TComponentSet, TComponentSetData>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        #region Fields and Autoproperties

        private ReusableDelegateCollection<TComponentSet> _delegates;

        [PropertyOrder(-500)]
        [SerializeField]
        protected bool showAll;

        #endregion

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The optional component set data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdate(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<Optional, TComponentSet> before,
            Action<Optional, TComponentSet> after)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, isElected, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The overridable component set data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdate(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<Override, TComponentSet> before,
            Action<Override, TComponentSet> after)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, overriding, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdate(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The optional component set data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        public static void RefreshAndUpdate(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, isElected, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The overridable component set data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        public static void RefreshAndUpdate(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, overriding, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        public static void RefreshAndUpdate(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The optional component set data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdate(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setRoot,
            Action<Optional, TComponentSet> before,
            Action<Optional, TComponentSet> after)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, isElected, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The overridable component set data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdate(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setRoot,
            Action<Override, TComponentSet> before,
            Action<Override, TComponentSet> after)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, overriding, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdate(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setRoot,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The optional component set data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        public static void RefreshAndUpdate(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setRoot)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, isElected, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The overridable component set data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        public static void RefreshAndUpdate(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setRoot)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, overriding, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        public static void RefreshAndUpdate(ref TComponentSetData data, ref TComponentSet set, GameObject setRoot)
        {
            using (_PRF_RefreshAndUpdate.Auto())
            {
                CreateOrRefreshSetData(ref data, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.UpdateComponentSet(set);
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
        ///             <description>Ensures the set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        internal void UpdateComponentSet(
            TComponentSet set,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null)
                        {
                            return;
                        }

                        UpdateComponentSetInternal(set, before, after);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="subscribe">Should the set be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        internal void UpdateComponentSet(TComponentSet set, bool subscribe = true)
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                if (!subscribe)
                {
                    UpdateComponentSetInternal(set);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null)
                        {
                            return;
                        }

                        UpdateComponentSetInternal(set);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(set, CreateSubscribableDelegate);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        protected virtual void CreateOrRefresh()
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                CreateOrRefresh();
            }
        }

        private static void CreateOrRefreshSetData(ref TComponentSetData data, string setName)
        {
            using (_PRF_CreateOrRefreshSetData.Auto())
            {
                data ??= new();

                data.CreateOrRefresh();
            }
        }

        private static void CreateOrRefreshSetData(ref Optional data, bool isElected, string setName)
        {
            using (_PRF_CreateOrRefreshSetData.Auto())
            {
                data ??= new(isElected, new());
                data.Value ??= new();

                data.Value.CreateOrRefresh();
            }
        }

        private static void CreateOrRefreshSetData(ref Override data, bool overriding, string setName)
        {
            using (_PRF_CreateOrRefreshSetData.Auto())
            {
                data ??= new(overriding, new());
                data.Value ??= new();

                data.Value.CreateOrRefresh();
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        private void UpdateComponentSetAndSubscribe(TComponentSet set, Func<Action> delegateCreator)
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
                _delegates.SubscribeAndInvoke(set, ref Changed, delegateCreator);
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
        /// <param name="set">The set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private void UpdateComponentSetInternal(
            TComponentSet set,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                before?.Invoke(this as TComponentSetData, set);
                UpdateComponentSetInternal(set);
                after?.Invoke(this as TComponentSetData, set);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="set" /> is null.</exception>
        private void UpdateComponentSetInternal(TComponentSet set)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                try
                {
                    ApplyToComponentSet(set);
                }
                catch (ComponentInitializationException setEx)
                {
                    Context.Log.Error(
                        string.Format(
                            UPDATE_FAILURE_LOG_FORMAT,
                            typeof(TComponentSet).FormatForLogging(),
                            setEx.Message
                        )
                    );

                    throw;
                }

                set.Enable(this as TComponentSetData);
                set.GameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        #region IComponentSetData<TComponentSet,TComponentSetData> Members

        public abstract void ApplyToComponentSet(TComponentSet componentSet);

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyToComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyToComponentSet));

        protected static readonly ProfilerMarker _PRF_CreateOrRefresh =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrRefresh));

        private static readonly ProfilerMarker _PRF_CreateOrRefreshSetData =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrRefreshSetData));

        private static readonly ProfilerMarker _PRF_RefreshAndUpdate =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndUpdate));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetAndSubscribe =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetAndSubscribe));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetInternal));

        #endregion
    }
}
