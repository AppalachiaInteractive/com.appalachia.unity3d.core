using System;
using System.Diagnostics;
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
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     Configuration data for a
    ///     <see cref="ComponentSubset{TComponentSubset,TComponentSubsetData}" />
    ///     .
    /// </summary>
    /// <typeparam name="TComponentSubset">The type of subset this configures.</typeparam>
    /// <typeparam name="TComponentSubsetData">This type.</typeparam>
    [Serializable]
    [DebuggerDisplay("{Name} (ComponentSubsetData)")]
    public abstract class ComponentSubsetData<TComponentSubset, TComponentSubsetData> :
        AppalachiaBase<TComponentSubsetData>,
        IComponentData<TComponentSubset, TComponentSubsetData>,
        IInspectorVisibility,
        IFieldLockable,
        IRemotelyEnabled
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        protected ComponentSubsetData()
        {
        }

        protected ComponentSubsetData(Object owner) : base(owner)
        {
        }

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

        private ReusableDelegateCollection<TComponentSubset> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        private Func<bool> _shouldEnable;

        private Func<bool> _shouldEnableFunction;

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        #endregion

        /// <summary>
        ///     Creates or refreshes the <see cref="subsetData" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="subsetData">The configuration data subset.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="subsetData" /> lives within.</param>
        public static void CreateOrRefresh(ref TComponentSubsetData subsetData, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                if (subsetData == null)
                {
                    subsetData = CreateWithOwner(owner);
                }
                else if (owner != null)
                {
                    subsetData.SetOwner(owner);
                }

                subsetData.CreateOrRefresh(owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetParent">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="subsetObjectName">The name of the subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref Override data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetParent,
            string subsetObjectName,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetParent,
                    subsetObjectName
                );
                data.UpdateComponentSubset(subset, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetParent">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="subsetObjectName">The name of the subset.</param>
        public static void RefreshAndApply(
            ref Override data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetParent,
            string subsetObjectName)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetParent,
                    subsetObjectName
                );
                data.UpdateComponentSubset(subset);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetParent">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="subsetObjectName">The name of the subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref Optional data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetParent,
            string subsetObjectName,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetParent,
                    subsetObjectName
                );
                data.UpdateComponentSubset(subset, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetParent">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="subsetObjectName">The name of the subset.</param>
        public static void RefreshAndApply(
            ref Optional data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetParent,
            string subsetObjectName)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetParent,
                    subsetObjectName
                );
                data.UpdateComponentSubset(subset);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetParent">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="subsetObjectName">The name of the subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref TComponentSubsetData data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetParent,
            string subsetObjectName,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetParent,
                    subsetObjectName
                );
                data.Apply(subset, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetParent">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="subsetObjectName">The name of the subset.</param>
        public static void RefreshAndApply(
            ref TComponentSubsetData data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetParent,
            string subsetObjectName)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetParent,
                    subsetObjectName
                );
                data.Apply(subset);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetObject">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref Override data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetObject,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetObject
                );
                data.UpdateComponentSubset(subset, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetObject">The <see cref="GameObject" /> that the subset should be a child of.</param>
        public static void RefreshAndApply(
            ref Override data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetObject)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetObject
                );
                data.UpdateComponentSubset(subset);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetObject">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref Optional data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetObject,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetObject
                );
                data.UpdateComponentSubset(subset, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetObject">The <see cref="GameObject" /> that the subset should be a child of.</param>
        public static void RefreshAndApply(
            ref Optional data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetObject)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.Value.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetObject
                );
                data.UpdateComponentSubset(subset);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetObject">The <see cref="GameObject" /> that the subset should be a child of.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref TComponentSubsetData data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetObject,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetObject
                );
                data.Apply(subset, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="subset" />.
        /// </summary>
        /// <remarks>The primary API for applying component subset data to component subsets.</remarks>
        /// <param name="data">The component subset data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="subset">The component subset to apply the <paramref name="data" /> to.</param>
        /// <param name="subsetObject">The <see cref="GameObject" /> that the subset should be a child of.</param>
        public static void RefreshAndApply(
            ref TComponentSubsetData data,
            Object owner,
            ref TComponentSubset subset,
            GameObject subsetObject)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, ref subset, owner);
                data.SetOwner(owner);
                ComponentSubset<TComponentSubset, TComponentSubsetData>.GetOrAddComponents(
                    ref subset,
                    data,
                    subsetObject
                );
                data.Apply(subset);
            }
        }

        public override void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
                base.InitializePreferences();

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
        /// <param name="subset">The subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        internal void Apply(
            TComponentSubset subset,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (subset == null)
                        {
                            return;
                        }

                        OnApplyInternal(subset, before, after);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(subset, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the subset by applying the configuration values to the subset fields.
        /// </summary>
        /// <param name="subset">The subset to update.</param>
        /// <param name="subscribe">Should the subset be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="subset" /> is null.</exception>
        internal void Apply(TComponentSubset subset, bool subscribe = true)
        {
            using (_PRF_Apply.Auto())
            {
                if (!subscribe)
                {
                    OnApplyInternal(subset);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (subset == null)
                        {
                            return;
                        }

                        OnApplyInternal(subset);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(subset, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Creates or refreshes this data subset to ensure it is ready to use.
        /// </summary>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this subset lives within.</param>
        protected abstract void CreateOrRefresh(Object owner);

        protected abstract void OnApply(TComponentSubset subset);

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="subsetData" /> and <paramref name="subset" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="subsetData">The configuration data subset.</param>
        /// <param name="subset">The subset.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="subsetData" /> lives within.</param>
        private static void CreateOrRefresh(ref Optional subsetData, ref TComponentSubset subset, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                var data = subsetData.Value;
                CreateOrRefresh(ref data, owner);
                subsetData.Value = data;
                CreateOrRefresh(ref subset, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="subsetData" /> and <paramref name="subset" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="subsetData">The configuration data subset.</param>
        /// <param name="subset">The subset.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="subsetData" /> lives within.</param>
        private static void CreateOrRefresh(ref Override subsetData, ref TComponentSubset subset, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                var data = subsetData.Value;
                CreateOrRefresh(ref data, owner);
                subsetData.Value = data;
                CreateOrRefresh(ref subset, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="subsetData" /> and <paramref name="subset" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="subsetData">The configuration data subset.</param>
        /// <param name="subset">The subset.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="subsetData" /> lives within.</param>
        private static void CreateOrRefresh(
            ref TComponentSubsetData subsetData,
            ref TComponentSubset subset,
            Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                CreateOrRefresh(ref subsetData, owner);
                CreateOrRefresh(ref subset,     owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <see cref="UnityEngine.Component" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="subset">The subset.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="subset" /> lives within.</param>
        private static void CreateOrRefresh(ref TComponentSubset subset, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                if (subset == null)
                {
                    subset = ComponentSubset<TComponentSubset, TComponentSubsetData>.CreateWithOwner(owner);
                }
                else
                {
                    subset.SetOwner(owner);
                }
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
        /// <param name="subset">The subset.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private void OnApplyInternal(
            TComponentSubset subset,
            Action<TComponentSubsetData, TComponentSubset> before,
            Action<TComponentSubsetData, TComponentSubset> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TComponentSubsetData, subset);
                OnApplyInternal(subset);
                after?.Invoke(this as TComponentSubsetData, subset);
            }
        }

        /// <summary>
        ///     Updates the subset by applying the configuration values to the subset fields.
        /// </summary>
        /// <param name="subset">The subset to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="subset" /> is null.</exception>
        private void OnApplyInternal(TComponentSubset subset)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                if (!HasBeenInitialized)
                {
                    return;
                }

                try
                {
                    OnApply(subset);
                }
                catch (ComponentInitializationException ex)
                {
                    Context.Log.Error(
                        string.Format(
                            UPDATE_FAILURE_LOG_FORMAT,
                            typeof(TComponentSubset).FormatForLogging(),
                            ex.Message
                        )
                    );

                    throw;
                }

                if (ShouldEnable)
                {
                    subset.Enable(this as TComponentSubsetData);
                }
                else
                {
                    subset.Disable();
                }

                subset.gameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        /// <summary>
        ///     Updates the subset by applying the configuration values to the subset fields.
        /// </summary>
        /// <param name="subset">The subset to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="subset" /> is null.</exception>
        private void SubscribeAndApply(TComponentSubset subset, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (subset == null)
                {
                    throw new ComponentSubsetNotInitializedException(
                        typeof(TComponentSubset),
                        $"You must initialize the {typeof(TComponentSubset).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(subset, ref Changed, delegateCreator);
            }
        }

        #region IComponentData<TComponentSubset,TComponentSubsetData> Members

        void IComponentData<TComponentSubset>.Apply(TComponentSubset comp)
        {
            using (_PRF_UpdateComponent.Auto())
            {
                OnApply(comp);
            }
        }

        void IComponentData.InitializeFields(Object owner)
        {
        }

        public virtual void ResetData()
        {
            using (_PRF_ResetData.Auto())
            {
            }
        }

        #endregion

        #region IFieldLockable Members

        public Color LockToggleColor => DisableAllFields ? LockToggleColorOn.v : LockToggleColorOff.v;

        public Color SuspendToggleColor => SuspendFieldApplication ? SuspendToggleColorOn.v : SuspendToggleColorOff.v;

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

        protected static readonly ProfilerMarker _PRF_ResetData = new ProfilerMarker(_PRF_PFX + nameof(ResetData));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        private static readonly ProfilerMarker _PRF_UpdateComponent =
            new ProfilerMarker(_PRF_PFX + nameof(IComponentData<TComponentSubset>.Apply));

        #endregion
    }
}
