using System;
using System.Diagnostics;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Exceptions;
using Appalachia.Core.Objects.Components.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Sets
{
    [CallStaticConstructorInEditor]
    [Serializable]
    [DebuggerDisplay("{Name} (ComponentSetData)")]
    public abstract class ComponentSetData<TComponentSet, TComponentSetData> : AppalachiaBase<TComponentSetData>,
                                                                               IComponentSetData<TComponentSet,
                                                                                   TComponentSetData>,
                                                                               IInspectorVisibility,
                                                                               IFieldLockable,
                                                                               IRemotelyEnabled
        where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>,
        IComponentSetData<TComponentSet, TComponentSetData>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        #region Preferences

        [NonSerialized] private PREF<Color> _advancedToggleColorOff;
        [NonSerialized] private PREF<Color> _advancedToggleColorOn;

        [NonSerialized] private PREF<Color> _hideToggleColorOff;
        [NonSerialized] private PREF<Color> _hideToggleColorOn;
        [NonSerialized] private PREF<Color> _lockToggleColorOff;
        [NonSerialized] private PREF<Color> _lockToggleColorOn;
        [NonSerialized] private PREF<Color> _showToggleColorOff;
        [NonSerialized] private PREF<Color> _showToggleColorOn;
        [NonSerialized] private PREF<Color> _suspendToggleColorOff;
        [NonSerialized] private PREF<Color> _suspendToggleColorOn;

        private PREF<Color> AdvancedToggleColorOff
        {
            get
            {
                if (_advancedToggleColorOff == null)
                {
                    _advancedToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOff;
            }
        }

        private PREF<Color> AdvancedToggleColorOn
        {
            get
            {
                if (_advancedToggleColorOn == null)
                {
                    _advancedToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOn;
            }
        }

        private PREF<Color> HideToggleColorOff
        {
            get
            {
                if (_hideToggleColorOff == null)
                {
                    _hideToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOff;
            }
        }

        private PREF<Color> HideToggleColorOn
        {
            get
            {
                if (_hideToggleColorOn == null)
                {
                    _hideToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOn;
            }
        }

        private PREF<Color> LockToggleColorOff
        {
            get
            {
                if (_lockToggleColorOff == null)
                {
                    _lockToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOff;
            }
        }

        private PREF<Color> LockToggleColorOn
        {
            get
            {
                if (_lockToggleColorOn == null)
                {
                    _lockToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOn;
            }
        }

        private PREF<Color> ShowToggleColorOff
        {
            get
            {
                if (_showToggleColorOff == null)
                {
                    _showToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOff;
            }
        }

        private PREF<Color> ShowToggleColorOn
        {
            get
            {
                if (_showToggleColorOn == null)
                {
                    _showToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOn;
            }
        }

        private PREF<Color> SuspendToggleColorOff
        {
            get
            {
                if (_suspendToggleColorOff == null)
                {
                    _suspendToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOff;
            }
        }

        private PREF<Color> SuspendToggleColorOn
        {
            get
            {
                if (_suspendToggleColorOn == null)
                {
                    _suspendToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOn;
            }
        }

        #endregion

        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        private ReusableDelegateCollection<TComponentSet> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        private Func<bool> _shouldEnableFunction;

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
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<Optional, TComponentSet> before,
            Action<Optional, TComponentSet> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set, before, after);
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
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<Override, TComponentSet> before,
            Action<Override, TComponentSet> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set, before, after);
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
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set, before, after);
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
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set);
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
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set);
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
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, gameObjectNamePostfix);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set);
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
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setRoot,
            Action<Optional, TComponentSet> before,
            Action<Optional, TComponentSet> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set, before, after);
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
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setRoot,
            Action<Override, TComponentSet> before,
            Action<Override, TComponentSet> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set, before, after);
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
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setRoot,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set, before, after);
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
        public static void RefreshAndApply(ref Optional data, bool isElected, ref TComponentSet set, GameObject setRoot)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set);
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
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setRoot)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set);
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
        public static void RefreshAndApply(ref TComponentSetData data, ref TComponentSet set, GameObject setRoot)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, setRoot.name);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set);
            }
        }

        public override void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
                base.InitializePreferences();

                _advancedToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(AdvancedToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _advancedToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(AdvancedToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _hideToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(HideToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _hideToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(HideToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _lockToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(LockToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _lockToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(LockToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _showToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(ShowToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _showToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(ShowToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _suspendToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(SuspendToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _suspendToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(SuspendToggleColorOn),
                    Colors.WhiteSmokeGray96
                );
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
        internal void Apply(
            TComponentSet set,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null)
                        {
                            return;
                        }

                        OnApplyInternal(set, before, after);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="subscribe">Should the set be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        internal void Apply(TComponentSet set, bool subscribe = true)
        {
            using (_PRF_Apply.Auto())
            {
                if (!subscribe)
                {
                    OnApplyInternal(set);
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

                        OnApplyInternal(set);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(set, CreateSubscribableDelegate);
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

        private static void CreateOrRefresh(ref TComponentSetData data, string setName)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                data ??= new();

                data.CreateOrRefresh();
            }
        }

        private static void CreateOrRefresh(ref Optional data, bool isElected, string setName)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                data ??= new(isElected, new());
                data.Value ??= new();

                data.Value.CreateOrRefresh();
            }
        }

        private static void CreateOrRefresh(ref Override data, bool overriding, string setName)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                data ??= new(overriding, new());
                data.Value ??= new();

                data.Value.CreateOrRefresh();
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
        private void OnApplyInternal(
            TComponentSet set,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TComponentSetData, set);
                OnApplyInternal(set);
                after?.Invoke(this as TComponentSetData, set);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="set" /> is null.</exception>
        private void OnApplyInternal(TComponentSet set)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                try
                {
                    OnApply(set);
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

                if (ShouldEnable)
                {
                    set.Enable(this as TComponentSetData);
                    set.GameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    set.Disable();
                    set.GameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        private void SubscribeAndApply(TComponentSet set, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeAndApply.Auto())
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

        #region IComponentSetData<TComponentSet,TComponentSetData> Members

        public abstract void OnApply(TComponentSet componentSet);

        #endregion

        #region IFieldLockable Members

        public Color LockToggleColor => DisableAllFields ? LockToggleColorOn.v : LockToggleColorOff.v;

        public bool DisableAllFields
        {
            get => _disableAllFields;
            set => _disableAllFields = value;
        }

        public bool SuspendFieldApplication
        {
            get => _suspendFieldApplication;
            set => _suspendFieldApplication = value;
        }

        public Color SuspendToggleColor => SuspendFieldApplication ? SuspendToggleColorOn.v : SuspendToggleColorOff.v;

        #endregion

        #region IInspectorVisibility Members

        public bool ShowAdvancedOptions
        {
            get => _showAdvancedOptions || HideAllFields || ShowAllFields || SuspendFieldApplication || DisableAllFields;
            set => _showAdvancedOptions = value;
        }

        public Color AdvancedToggleColor => ShowAdvancedOptions ? AdvancedToggleColorOn.v : AdvancedToggleColorOff.v;

        public Color ShowToggleColor => ShowAllFields ? ShowToggleColorOn.v : ShowToggleColorOff.v;

        public Color HideToggleColor => HideAllFields ? HideToggleColorOn.v : HideToggleColorOff.v;

        public bool HideAllFields
        {
            get => _hideAllFields && !_showAllFields;
            set => _hideAllFields = value;
        }

        public bool ShowAllFields
        {
            get => _showAllFields;
            set => _showAllFields = value;
        }

        #endregion

        #region IRemotelyEnabled Members

        public Func<bool> ShouldEnableFunction
        {
            get => _shouldEnableFunction;
            set => _shouldEnableFunction = value;
        }

        public bool ShouldEnable => _shouldEnableFunction?.Invoke() ?? true;

        public void BindEnabledStateTo(IRemotelyEnabledController controller)
        {
            using (_PRF_BindEnabledStateTo.Auto())
            {
                ShouldEnableFunction = () => controller.ShouldEnable;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_BindEnabledStateTo =
            new ProfilerMarker(_PRF_PFX + nameof(BindEnabledStateTo));

        protected static readonly ProfilerMarker _PRF_CreateOrRefresh =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrRefresh));

        protected static readonly ProfilerMarker _PRF_OnApply = new ProfilerMarker(_PRF_PFX + nameof(OnApply));

        private static readonly ProfilerMarker _PRF_OnApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyInternal));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
